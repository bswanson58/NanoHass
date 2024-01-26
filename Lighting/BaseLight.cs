using nanoFramework.Json;
using NanoHass.Discovery;
using NanoHass.Support;
using System;
using System.Collections;

namespace NanoHass.Lighting {
    public abstract class BaseLight : AbstractDiscoverable {
        private const string    cDeviceName = "Unknown Light";

        protected bool          mState;
        protected int           mBrightness;

        protected BaseLight( LightConfiguration configuration ) : 
            base( configuration.DisplayName ?? cDeviceName, Constants.LightDomain, configuration.EntityIdentifier,
                configuration.UpdateIntervalInSeconds) {
            mState = false;
            mBrightness = 0;
        }

        public override string GetDiscoveryPayload() {
            if( GetDiscoveryModel() is LightDiscoveryModel discoveryModel ) {
                return discoveryModel.AsJson();
            }

            return String.Empty;
        }

        public override string GetSubscriptionTopic() {
            var topic = ClientContext.DeviceMessageSubscriptionTopic();

            return topic;
        }

        public override bool ProcessMessage( string topic, string payload ) {
            if( GetDiscoveryModel() is LightDiscoveryModel discoveryModel ) {
                if( topic.Equals( discoveryModel.brightness_command_topic )) {
                    OnBrightnessCommand( payload );

                    return true;
                }
                if( topic.Equals( discoveryModel.command_topic )) {
                    OnCommand( payload );

                    return true;
                }
            }

            return false;
        }

        public override string GetState() =>
            JsonSerializer.SerializeObject(
                new Hashtable {{ Constants.PayloadValue, mState ? Constants.OnState : Constants.OffState }});

        public void SetState( bool state ) =>
            mState = state;

        protected string GetBrightness() =>
            JsonSerializer.SerializeObject(
                new Hashtable {{ Constants.PayloadValue, mBrightness }});
        
        public void SetBrightness( int value ) =>
            mBrightness = value;


        protected virtual void OnBrightnessCommand( string payload ) =>
            mBrightness = Int32.Parse( payload );

        protected virtual void OnCommand( string payload ) =>
            mState = payload.ToUpper().Equals( Constants.OnState.ToUpper());
    }
}
