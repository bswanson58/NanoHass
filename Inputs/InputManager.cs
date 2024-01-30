using Microsoft.Extensions.Logging;
using NanoHass.Hass;
using NanoPlat.Logging;
using NanoPlat.Tasks;
using System.Collections;
using System;
using NanoHass.Context;
using NanoHass.Discovery;
using NanoPlat.Mqtt;
using NanoHass.Support;

namespace NanoHass.Inputs {
    public interface IInputManager {
        void        AddInput( AbstractDiscoverable input );

        void        StartUpdating();
        void        StopUpdating();

        IHassEntity GetInput( string entityIdentifier );
    }

    public class InputManager : IInputManager {
        private readonly IHassManager       mHassManager;
        private readonly IHassClientContext mClientContext;
        private readonly IMqttManager       mMqttManager;
        private readonly ITaskScheduler     mTaskScheduler;
        private readonly ILogger            mLogger;
        private readonly ArrayList          mInputs;
        private DateTime                    mLastAutoDiscoPublish;

        public InputManager( IHassManager hassManager, IHassClientContext clientContext, IMqttManager mqttManager,
                             ITaskScheduler taskScheduler, HassDeviceOptions devices, ILoggerEx log ) {
            mHassManager = hassManager;
            mClientContext = clientContext;
            mTaskScheduler = taskScheduler;

            mMqttManager = mqttManager;
            mMqttManager.MessageReceived += OnMessageReceived;

            mInputs = new ArrayList();
            mLastAutoDiscoPublish = DateTime.MinValue;

            mLogger = log.SetLogLevel( nameof( InputManager ));

            foreach( var device in devices.GetInputs()) {
                if( device is IntegerNumberConfiguration input ) {
                    AddInput( new IntegerNumber( input ));
                }
            }
        }

        private void OnMessageReceived( object sender, EventArgs e ) {
            if( e is MqttMessageArgs message ) {
                try {
                    if( message.Topic.StartsWith( mClientContext.DeviceTopic( Constants.NumberDomain ))) {
                        mLogger.Log( LogLevel.Trace, message.Topic );

                        foreach( var device in mInputs ) {
                            if( device is BaseInput input ) {
                                if( input.ProcessMessage( message.Topic, message.Message )) {
                                    mHassManager.PublishState( input, false );
                                }
                            }
                        }
                    }
                }
                catch( Exception ex ) {
                    mLogger.Log( LogLevel.Error, ex, $"Processing message: '{message.Topic}'" );
                }
            }
        }

        public void AddInput( AbstractDiscoverable sensor ) {
            sensor.InitializeParameters( mHassManager.ClientContext );

            mInputs.Add( sensor );
        }

        public void StartUpdating() {
            mTaskScheduler.Start( UpdateInputs, (int)TimeSpan.FromSeconds( 3 ).TotalMilliseconds, nameof( InputManager ));
        }

        public void StopUpdating() {
            mTaskScheduler.Stop( nameof( InputManager ));
        }

        public IHassEntity GetInput( string entityIdentifier ) {
            foreach( var item in mInputs ) {
                if( item is BaseInput input ) {
                    if( input.EntityIdentifier.Equals( entityIdentifier )) {
                        return input;
                    }
                }
            }

            return null;
        }
        
        private void UpdateInputs() {
            try {
                if(!mHassManager.IsActive ) {
                    return;
                }

                // publish availability & sensor discovery every 30 sec
                if(( DateTime.UtcNow - mLastAutoDiscoPublish ).TotalSeconds > 30 ) {
                    foreach( var input in mInputs ) {
                        mHassManager.PublishDiscoveryConfiguration( input as AbstractDiscoverable );
                    }

                    mLastAutoDiscoPublish = DateTime.UtcNow;
                }

                foreach( var input in mInputs ) {
                    mHassManager.PublishState( input as AbstractDiscoverable );
                }
            }
            catch( Exception ex ) {
                mLogger.Log( LogLevel.Error, "Error while publishing input.", ex );
            }
        }
    }
}
