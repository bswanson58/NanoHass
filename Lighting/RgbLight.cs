﻿using NanoHass.Discovery;
using NanoHass.Models;
using NanoHass.Support;
using System.Collections;
using System;
using System.Drawing;
using Microsoft.Extensions.Logging;
using nanoFramework.Json;
using NanoHass.Hass;

namespace NanoHass.Lighting {
    public interface IRgbLightEntity : IHassEntity {
        void        SetLightState( bool state );
        bool        GetLightState();

        void        SetBrightness( int brightness );
        int         GetBrightness();

        void        SetColor( Color color );
        Color       GetColor();
    }

    public class RgbLight : BaseLight, IRgbLightEntity {
        private readonly LightConfiguration mConfiguration;
        private Color                       mColor;

        public RgbLight( LightConfiguration configuration ) :
            base( configuration ) {
            mConfiguration = configuration;
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
                rgb_state_topic = $"{ClientContext.EntityTopic( Domain, mConfiguration.EntityIdentifier )}/{Constants.RgbColor}/{Constants.Status}",
                rgb_command_topic = $"{ClientContext.EntityTopic( Domain, mConfiguration.EntityIdentifier )}/{Constants.RgbColor}/{Constants.Subscribe}",
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
                if(!String.IsNullOrEmpty( discoveryModel.rgb_state_topic )) {
                    retValue.Add( new DeviceTopicState( discoveryModel.rgb_state_topic, GetColorPayload()));
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

        public void SetColor( Color color ) {
            mColor = color;

            TriggerStateChange();
        }

        private string GetColorPayload() =>
            JsonSerializer.SerializeObject(
                new Hashtable {{ Constants.PayloadValue, $"{mColor.R},{mColor.G},{mColor.B}" }});

        public override bool ProcessMessage( string topic, string payload ) {
            if( GetDiscoveryModel() is LightDiscoveryModel discoveryModel ) {
                if( topic.Equals( discoveryModel.rgb_command_topic )) {
                    OnRgbCommand( payload );

                    return true;
                }
            }

            return base.ProcessMessage( topic, payload );
        }

        private void OnRgbCommand( string payload ) {
            if(!String.IsNullOrEmpty( payload )) {
                var colors = payload.Split( ',' );
                
                if( colors.Length == 3 ) {
                    try {
                        var red = Int16.Parse( colors[0]);
                        var green = Int16.Parse( colors[1]);
                        var blue = Int16.Parse( colors[2]);

                        mColor = Color.FromArgb( GetBrightness(), red, green, blue );

                        TriggerStateChange();
                    }
                    catch( Exception ex ) {
                        ClientContext.Logger.Log( LogLevel.Error, ex, "Parsing RGB color payload" );
                    }
                }
            }
        }

        public Color GetColor() =>
            mColor;
    }
}
