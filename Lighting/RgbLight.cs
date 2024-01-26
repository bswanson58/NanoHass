using NanoHass.Discovery;
using NanoHass.Models;
using NanoHass.Support;
using System.Collections;
using System;

namespace NanoHass.Lighting {
    public class RgbLight : BaseLight {
        private readonly LightConfiguration mConfiguration;

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
                state_value_template = "{{value_json.value}}",
                brightness_value_template = "{{value_json.value}}",
                brightness_scale = 100
            };
        }

        public override IList GetStatesToPublish() {
            var retValue = new ArrayList();

            if( GetDiscoveryModel() is LightDiscoveryModel discoveryModel ) {
                if(!String.IsNullOrEmpty( discoveryModel.state_topic )) {
                    retValue.Add( new DeviceTopicState( discoveryModel.state_topic, GetState()));
                }
                if(!String.IsNullOrEmpty( discoveryModel.brightness_state_topic )) {
                    retValue.Add( new DeviceTopicState( discoveryModel.brightness_state_topic, GetBrightness()));
                }
            }

            return retValue;
        }
    }
}
