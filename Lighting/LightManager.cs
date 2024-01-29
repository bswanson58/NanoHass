using Microsoft.Extensions.Logging;
using NanoHass.Hass;
using NanoHass.Sensors;
using NanoPlat.Logging;
using NanoPlat.Tasks;
using System.Collections;
using System;
using NanoHass.Context;
using NanoHass.Support;
using NanoPlat.Mqtt;

namespace NanoHass.Lighting {
    public interface ILightManager {
        void        AddLight( LightConfiguration configuration );
        void        AddLight( BaseLight sensor );
        IHassEntity GetLight( string entityIdentifier );

        void        StartUpdating();
        void        StopUpdating();
    }

    public class LightManager : ILightManager {
        private readonly IMqttManager       mMqttManager;
        private readonly IHassManager       mHassManager;
        private readonly IHassClientContext mClientContext;
        private readonly IHassDeviceFactory mDeviceFactory;
        private readonly ITaskScheduler     mTaskScheduler;
        private readonly ILogger            mLogger;
        private readonly ArrayList          mLights;
        private DateTime                    mLastAutoDiscoPublish;

        public LightManager( IHassManager hassManager, HassDeviceOptions devices, IHassClientContext clientContext,
                             IHassDeviceFactory deviceFactory, ITaskScheduler taskScheduler, IMqttManager mqttManager,
                             ILoggerEx log ) {
            mHassManager = hassManager;
            mDeviceFactory = deviceFactory;
            mClientContext = clientContext;
            mTaskScheduler = taskScheduler;

            mMqttManager = mqttManager;
            mMqttManager.MessageReceived += OnMessageReceived;

            mLights = new ArrayList();
            mLastAutoDiscoPublish = DateTime.MinValue;

            mLogger = log.SetLogLevel( nameof( LightManager ));

            foreach( var device in devices.GetLights()) {
                if( device is LightConfiguration configuration ) {
                    AddLight( configuration );
                }
            }
        }

        public void AddLight( LightConfiguration configuration ) {
            var light = mDeviceFactory.CreateLight( configuration );

            if( light != null ) {
                AddLight( light );
            }
        }

        public void AddLight( BaseLight light ) {
            light.InitializeParameters( mHassManager.ClientContext );
            mMqttManager.Subscribe( light.GetSubscriptionTopic());

            mLights.Add( light );
        }

        public void StartUpdating() {
            mTaskScheduler.Start( UpdateLights, (int)TimeSpan.FromSeconds( 3 ).TotalMilliseconds, nameof( LightManager ));
        }

        public void StopUpdating() {
            mTaskScheduler.Stop( nameof( SensorManager ));
        }

        private void OnMessageReceived( object sender, EventArgs e ) {
            if( e is MqttMessageArgs message ) {
                try {
                    if( message.Topic.StartsWith( mClientContext.DeviceTopic( Constants.LightDomain ))) {
                        mLogger.Log( LogLevel.Trace, message.Topic );

                        foreach( var device in mLights ) {
                            if( device is BaseLight light ) {
                                light.ProcessMessage( message.Topic, message.Message );
                            }
                        }
                    }
                }
                catch( Exception ex ) {
                    mLogger.Log( LogLevel.Error, ex, $"Processing message: '{message.Topic}'" );
                }
            }
        }

        public IHassEntity GetLight( string entityIdentifier ) {
            foreach( var item in mLights ) {
                if( item is BaseLight light ) {
                    if( light.EntityIdentifier.Equals( entityIdentifier )) {
                        return light as IHassEntity;
                    }
                }
            }

            return null;
        }

        public void UpdateLightState( string entityIdentifier, bool state ) {
            foreach( var item in mLights ) {
                if( item is BaseLight light ) {
                    if( light.EntityIdentifier.Equals( entityIdentifier )) {
                        light.SetState( state );
                    }
                }
            }
        }

        public void UpdateLightBrightness( string entityIdentifier, int value ) {
            foreach( var item in mLights ) {
                if( item is BaseLight light ) {
                    if( light.EntityIdentifier.Equals( entityIdentifier )) {
                        light.SetBrightness( value );
                    }
                }
            }
        }

        private void UpdateLights() {
            try {
                if(!mHassManager.IsActive ) {
                    return;
                }

                // publish availability & sensor discovery every 30 sec
                if(( DateTime.UtcNow - mLastAutoDiscoPublish ).TotalSeconds > 30 ) {
                    foreach( var light in mLights ) {
                        mHassManager.PublishDiscoveryConfiguration( light as BaseLight );
                    }

                    mLastAutoDiscoPublish = DateTime.UtcNow;
                }

                foreach( var light in mLights ) {
                    mHassManager.PublishState( light as BaseLight );
                }
            }
            catch( Exception ex ) {
                mLogger.Log( LogLevel.Error, "Error while updating light.", ex );
            }
        }
    }
}
