using NanoHass.Context;
using NanoPlat.Mqtt;
using System;
using Microsoft.Extensions.Logging;
using NanoHass.Discovery;
using NanoHass.Models;
using NanoPlat.Logging;
using NanoPlat.Tasks;

namespace NanoHass.Hass {
    public interface IHassManager {
        bool    IsActive { get; }

        void    StartProcessing();
        void    StopProcessing();

        void    PublishDiscoveryConfiguration( AbstractDiscoverable discoverable );
        void    RevokeDiscoveryConfiguration( AbstractDiscoverable discoverable );
        void    PublishState( AbstractDiscoverable device, bool respectUpdateTime = true );

        IHassClientContext  ClientContext { get; }
    }

    public class HassMqttManager : IHassManager {
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
            mLastAvailableAnnouncementFailedLogged = DateTime.MinValue;

            mLog = log.SetLogLevel( nameof( HassMqttManager ));
        }


        public void StartProcessing() {
            mTaskScheduler.Start(() => AnnounceDeviceAvailability(), 
                (int)TimeSpan.FromSeconds( 30 ).TotalMilliseconds, 
                nameof( HassMqttManager ));
        }

        public void StopProcessing() {
            mTaskScheduler.Stop( nameof( HassMqttManager ));
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

        public void PublishDiscoveryConfiguration( AbstractDiscoverable discoverable ) =>
            AnnounceDiscoveryConfiguration( discoverable );

        public void RevokeDiscoveryConfiguration( AbstractDiscoverable discoverable ) =>
            AnnounceDiscoveryConfiguration( discoverable, true );

        private void AnnounceDiscoveryConfiguration( AbstractDiscoverable discoverable, bool clearConfig = false ) {
            if( discoverable == null ) {
                return;
            }

            if(!mMqttManager.CanSend ) {
                return;
            }

            try {
                var topic = ClientContext.EntityConfigurationTopic( discoverable.Domain, discoverable.EntityIdentifier );
                var payload = String.Empty;

                if(!clearConfig ) {
                    payload = discoverable.GetDiscoveryPayload();
                }

                // Publish discovery configuration
                mMqttManager.Publish( topic, payload );
            }
            catch( Exception ex ) {
                mLog.Log( LogLevel.Error, "Error while announcing auto discovery", ex );
            }
        }

        public void PublishState( AbstractDiscoverable device, bool respectUpdateTime = true ) {
            try {
                if( respectUpdateTime ) {
                    if( device.LastUpdated.AddSeconds( device.UpdateIntervalSeconds ) > DateTime.UtcNow ) {
                        return;
                    }
                }

                var deviceState = device.GetStatePayload();
                var attributes = device.GetAttributes();

                if(( device.PreviousPublishedState == deviceState ) &&
                   ( device.PreviousPublishedAttributes == attributes )) {
                    return;
                }

                foreach( var state in device.GetStatesToPublish()) {
                    if( state is DeviceTopicState topicState ) {
                        mMqttManager.Publish( topicState.Topic, topicState.State );
                    }
                }

//                if( device.UseAttributes ) {
//                    mMqttManager.Publish( autoDiscoConfig.json_attributes_topic, attributes );
//                }

                device.UpdatePublishedState( deviceState, attributes );
            }
            catch( Exception ex ) {
                mLog.Log( LogLevel.Error, $"Sensor '{device.Name}' - Error publishing state", ex );
            }
        }
    }
}
