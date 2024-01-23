using System;
using System.Collections;
using Microsoft.Extensions.Logging;
using NanoHass.Hass;
using NanoPlat.Logging;
using NanoPlat.Tasks;

namespace NanoHass.Sensors {
    public interface ISensorManager {
        void    AddSensor( BaseSensor sensor );

        void    StartUpdating();
        void    StopUpdating();
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

            foreach( var device in devices.GetDevices()) {
                if( device is SensorConfiguration sensor ) {
                    AddSensor( new Sensor( sensor ));
                }
            }
        }

        public void AddSensor( BaseSensor sensor ) {
            sensor.InitializeParameters( mHassManager.ClientContext );

            mSensors.Add( sensor );
        }

        public void StartUpdating() {
            mTaskScheduler.Start( UpdateSensors, (int)TimeSpan.FromSeconds( 3 ).TotalMilliseconds, nameof( SensorManager ));
        }

        public void StopUpdating() {
            mTaskScheduler.Stop( nameof( SensorManager ));
        }

        private void UpdateSensors() {
            try {
                if(!mHassManager.IsActive ) {
                    return;
                }

                // publish availability & sensor discovery every 30 sec
                if(( DateTime.UtcNow - mLastAutoDiscoPublish ).TotalSeconds > 30 ) {
                    foreach( var sensor in mSensors ) {
                        mHassManager.PublishDiscoveryConfiguration( sensor as BaseSensor );
                    }

                    mLastAutoDiscoPublish = DateTime.UtcNow;
                }

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
