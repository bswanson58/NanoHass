using System;
using System.Collections;
using Microsoft.Extensions.Logging;
using NanoHass.Hass;
using NanoPlat.Logging;
using NanoPlat.Tasks;

namespace NanoHass.Sensors {
    public interface ISensorManager {
        void    AddSensor( BaseSensor sensor );
    }

    internal class SensorManager : ISensorManager {
        private readonly IHassManager   mHassManager;
        private readonly ITaskScheduler mTaskScheduler;
        private readonly ILogger        mLogger;
        private readonly ArrayList      mSensors;
        private DateTime                mLastAutoDiscoPublish;

        public SensorManager( IHassManager hassManager, ITaskScheduler taskScheduler, HassDeviceOptions devices,
                              ILoggerEx log ) {
            mHassManager = hassManager;
            mTaskScheduler = taskScheduler;

            mSensors = new ArrayList();
            mLastAutoDiscoPublish = DateTime.MinValue;

            mLogger = log.SetLogLevel( nameof( SensorManager ));

            foreach( var device in devices.Devices ) {
                if( device is SensorConfiguration sensor ) {
                    AddSensor( new Sensor( sensor ));
                }
            }

            mTaskScheduler.Start( UpdateSensors, (int)TimeSpan.FromSeconds( 5 ).TotalMilliseconds, nameof( SensorManager ));
        }

        public void AddSensor( BaseSensor sensor ) {
            sensor.InitializeParameters( mHassManager.ClientContext );

            mSensors.Add( sensor );
        }

        private void UpdateSensors() {
            try {
                if(!mHassManager.IsActive ) {
                    return;
                }

                // publish availability & sensor discovery every 30 sec
                if(( DateTime.UtcNow - mLastAutoDiscoPublish ).TotalSeconds > 30 ) {
                    // publish the auto discovery
                    foreach( var sensor in mSensors ) {
                        mHassManager.PublishAutoDiscoveryConfiguration( sensor as BaseSensor );
                    }

                    mLastAutoDiscoPublish = DateTime.UtcNow;
                }

                // publish sensor states (they have their own time-based scheduling)
                foreach( var sensor in mSensors ) {
                    mHassManager.PublishState( sensor as BaseSensor );
                }
            }
            catch( Exception ex ) {
                mLogger.Log( LogLevel.Error, "Error while publishing sensor.", ex );
            }
        }
    }
}
