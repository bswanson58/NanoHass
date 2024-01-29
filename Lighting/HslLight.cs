using NanoHass.Discovery;
using NanoHass.Models;
using NanoHass.Support;
using System.Collections;
using System;
using System.Drawing;
using Microsoft.Extensions.Logging;
using nanoFramework.Json;
using NanoHass.Hass;

namespace NanoHass.Lighting {
    public interface IHslLightEntity : IHassEntity {
        void        SetLightState( bool state );
        bool        GetLightState();

        void        SetBrightness( int brightness );
        int         GetBrightness();

        void        SetColor( int hue, int saturation );
        Color       GetColor();

        int         GetHue();
        int         GetSaturation();
    }

    public class HslLight : BaseLight, IHslLightEntity {
        private readonly LightConfiguration mConfiguration;
        private int                         mHue;
        private int                         mSaturation;

        public HslLight( LightConfiguration configuration ) :
            base( configuration ) {
            mConfiguration = configuration;

            mHue = 0;
            mSaturation = 100;
            mBrightness = 50;
        }

        protected override BaseDiscoveryModel CreateDiscoveryModel() {
            if( ClientContext == null ) {
                return null;
            }

            return new LightDiscoveryModel {
                availability_topic = ClientContext.DeviceAvailabilityTopic(),
                name = Name,
                icon = mConfiguration.Icon,
                unique_id = mConfiguration.UniqueIdentifier,
                object_id = $"{ClientContext.DeviceIdentifier}_{mConfiguration.EntityIdentifier}",
                device = ClientContext.DeviceConfiguration,
                enabled_by_default = true,
                state_topic = $"{ClientContext.EntityStateTopic( Domain, mConfiguration.EntityIdentifier )}/{Constants.Status}",
                command_topic = $"{ClientContext.EntityStateTopic( Domain, mConfiguration.EntityIdentifier )}/{Constants.Subscribe}",
                brightness_state_topic = $"{ClientContext.EntityTopic( Domain, mConfiguration.EntityIdentifier )}/{Constants.Brightness}/{Constants.Status}",
                brightness_command_topic = $"{ClientContext.EntityTopic( Domain, mConfiguration.EntityIdentifier )}/{Constants.Brightness}/{Constants.Subscribe}",
                hs_command_topic = $"{ClientContext.EntityTopic( Domain, mConfiguration.EntityIdentifier )}/{Constants.HslColor}/{Constants.Subscribe}",
                hs_state_topic = $"{ClientContext.EntityTopic( Domain, mConfiguration.EntityIdentifier )}/{Constants.HslColor}/{Constants.Status}",
                state_value_template = "{{value_json.value}}",
                brightness_value_template = "{{value_json.value}}",
                brightness_scale = 100
            };
        }

        public override IList GetStatesToPublish() {
            var retValue = new ArrayList();

            if( GetDiscoveryModel() is LightDiscoveryModel discoveryModel ) {
                if(!String.IsNullOrEmpty( discoveryModel.state_topic )) {
                    retValue.Add( new DeviceTopicState( discoveryModel.state_topic, GetStatePayload()));
                }
                if(!String.IsNullOrEmpty( discoveryModel.brightness_state_topic )) {
                    retValue.Add( new DeviceTopicState( discoveryModel.brightness_state_topic, GetBrightnessPayload()));
                }
                if(!String.IsNullOrEmpty( discoveryModel.hs_state_topic )) {
                    retValue.Add( new DeviceTopicState( discoveryModel.hs_state_topic, GetHsPayload()));
                }
            }

            return retValue;
        }

        public bool GetLightState() =>
            mState;

        public void SetLightState( bool state ) =>
            SetState( state );

        public int GetBrightness() =>
            mBrightness;

        public void SetColor( int hue, int saturation ) {
            mHue = hue;
            mSaturation = saturation;

            TriggerStateChange();
        }

        public Color GetColor() =>
            HslColor.FromHsl( mHue, mSaturation, GetBrightness());

        public int GetHue() =>
            mHue;

        public int GetSaturation() =>
            mSaturation;

        private string GetHsPayload() =>
            JsonSerializer.SerializeObject(
                new Hashtable {{ Constants.PayloadValue, $"{mHue},{mSaturation}" }});

        public override bool ProcessMessage( string topic, string payload ) {
            if( GetDiscoveryModel() is LightDiscoveryModel discoveryModel ) {
                if( topic.Equals( discoveryModel.hs_command_topic )) {
                    OnHsCommand( payload );

                    return true;
                }
            }

            return base.ProcessMessage( topic, payload );
        }

        private void OnHsCommand( string payload ) {
            if(!String.IsNullOrEmpty( payload )) {
                var colors = payload.Split( ',' );
                
                if( colors.Length == 2 ) {
                    try {
                        SetColor((int)float.Parse( colors[0]), (int)float.Parse( colors[1]));
                    }
                    catch( Exception ex ) {
                        ClientContext.Logger.Log( LogLevel.Error, ex, "Parsing HSL color payload" );
                    }
                }
            }
        }
    }
}
