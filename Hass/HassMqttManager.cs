using NanoHass.Context;
using NanoPlat.Mqtt;
using System;
using Microsoft.Extensions.Logging;
using nanoFramework.Json;
using NanoHass.Discovery;
using NanoHass.Models;
using NanoHass.Support;
using NanoPlat.Logging;
using NanoPlat.Tasks;

namespace NanoHass.Hass {
    public interface IHassManager {
        bool    IsActive { get; }

        void    StartProcessing();
        void    PublishAutoDiscoveryConfiguration( AbstractDiscoverable discoverable );
        void    PublishState( AbstractDiscoverable device, bool respectChecks = true );

        IHassClientContext  ClientContext { get; }
    }

    internal class HassMqttManager : IHassManager {
        private readonly IMqttManager           mMqttManager;
        private readonly ITaskScheduler         mTaskScheduler;
        private readonly ILogger                mLog;
        private DateTime                        mLastAvailableAnnouncementFailedLogged;

        public  IHassClientContext              ClientContext { get; }
        public  bool                            IsActive => mMqttManager.CanSend;

        public HassMqttManager( IHassClientContext clientContext, IMqttManager mqttManager, ITaskScheduler taskScheduler,
                                ILoggerEx log) {
            ClientContext = clientContext;
            mMqttManager = mqttManager;
            mTaskScheduler = taskScheduler;

            mLog = log.SetLogLevel( nameof( HassMqttManager ));
        }

        public void StartProcessing() {
            mTaskScheduler.Start(() => AnnounceDeviceAvailability(), 
                (int)TimeSpan.FromSeconds( 60 ).TotalMilliseconds, 
                nameof( AnnounceDeviceAvailability ));
        }

        private void AnnounceDeviceAvailability( bool offline = false ) {
            try {
                if( mMqttManager.CanSend ) {
                    mMqttManager.Publish( 
                        ClientContext.DeviceAvailabilityTopic(),
                        offline ? ClientContext.OfflinePayload : ClientContext.OnlinePayload );
                }
                else {
                    // only log failures once every 5 minutes to minimize log growth
                    if(( DateTime.UtcNow - mLastAvailableAnnouncementFailedLogged ).TotalMinutes < 5 ) {
                        return;
                    }

                    mLastAvailableAnnouncementFailedLogged = DateTime.UtcNow;

                    mLog.Log( LogLevel.Trace, "MQTT is not connected, availability announcement was not published" );
                }
            }
            catch( Exception ex ) {
                mLog.Log( LogLevel.Error, "Error while announcing availability", ex );
            }
        }

        public void PublishAutoDiscoveryConfiguration( AbstractDiscoverable discoverable ) =>
            AnnounceAutoDiscoveryConfigAsync( discoverable );

        public void RevokeAutoDiscoveryConfiguration( AbstractDiscoverable discoverable ) =>
            AnnounceAutoDiscoveryConfigAsync( discoverable, true );

        private void AnnounceAutoDiscoveryConfigAsync( AbstractDiscoverable discoverable, bool clearConfig = false ) {
            if( discoverable == null ) {
                return;
            }

            if(!mMqttManager.CanSend ) {
                return;
            }

            try {
                var topic =
                    $"{ClientContext.DeviceBaseTopic(discoverable.Domain)}/{discoverable.ObjectId}/{Constants.Configuration}";
                var payload = String.Empty;

                if(!clearConfig ) {
                    var discoveryConfig = discoverable.GetDiscoveryModel();

                    if( discoveryConfig != null ) {
                        payload = JsonSerializer.SerializeObject( discoveryConfig );
                    }
                }

                // Publish discovery configuration
                mMqttManager.Publish( topic, payload );
            }
            catch( Exception ex ) {
                mLog.Log( LogLevel.Error, "Error while announcing auto discovery", ex );
            }
        }

        public void PublishState( AbstractDiscoverable device, bool respectChecks = true ) {
            try {
                // are we asked to check elapsed time?
                if( respectChecks ) {
                    if( device.LastUpdated.AddSeconds( device.UpdateIntervalSeconds ) > DateTime.UtcNow ) {
                        return;
                    }
                }

                // get the current state/attributes
                var combinedState = device.GetCombinedState();
                var attributes = device.GetAttributes();

                // are we asked to check state changes?
                if( respectChecks ) {
                    if(( device.PreviousPublishedState == combinedState ) &&
                       ( device.PreviousPublishedAttributes == attributes )) {
                        return;
                    }
                }

                // fetch the auto discovery config
                if( device.GetDiscoveryModel() is not SensorDiscoveryModel autoDiscoConfig ) {
                    return;
                }

                foreach( var state in device.GetStatesToPublish()) {
                    if( state is DeviceTopicState topicState ) {
                        mMqttManager.Publish( topicState.Topic, topicState.State );
                    }
                }

                // optionally prepare and send attributes
                if( device.UseAttributes ) {
                    mMqttManager.Publish( autoDiscoConfig.json_attributes_topic, attributes );
                }

                // only store the values if the checks are respected
                // otherwise, we might stay in 'unknown' state until the value changes
                if(!respectChecks ) {
                    return;
                }

                device.UpdatePublishedState( combinedState, attributes );
            }
            catch( Exception ex ) {
                mLog.Log( LogLevel.Error, $"Sensor '{device.Name}' - Error publishing state", ex );
            }
        }
    }
}
