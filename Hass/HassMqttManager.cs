using NanoHass.Context;
using NanoPlat.Mqtt;
using System;
using Microsoft.Extensions.Logging;
using NanoPlat.Logging;
using NanoPlat.Tasks;

namespace NanoHass.Hass {
    public interface IHassManager {
        void    StartProcessing();
    }

    internal class HassMqttManager : IHassManager {
        private readonly IHassClientContext     mClientContext;
        private readonly IMqttManager           mMqttManager;
        private readonly ITaskScheduler         mTaskScheduler;
        private readonly ILogger                mLog;
        private DateTime                        mLastAvailableAnnouncementFailedLogged;

        public HassMqttManager( IHassClientContext clientContext, IMqttManager mqttManager, ITaskScheduler taskScheduler,
                                ILoggerEx log) {
            mClientContext = clientContext;
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
                        mClientContext.DeviceAvailabilityTopic(),
                        offline ? mClientContext.OfflinePayload : mClientContext.OnlinePayload );
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
    }
}
