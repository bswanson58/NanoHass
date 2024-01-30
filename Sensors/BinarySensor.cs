using NanoHass.Discovery;
using NanoHass.Support;
using System;

namespace NanoHass.Sensors {
    public class BinarySensor : BaseSensor {
        private readonly BinarySensorConfiguration  mConfiguration;

        public BinarySensor( BinarySensorConfiguration configuration )
            : base( configuration ) {
            mConfiguration = configuration;

            SetState( mConfiguration.OffPayload );
        }

        protected override BaseDiscoveryModel CreateDiscoveryModel() {
            if( ClientContext == null ) {
                return null;
            }

            return new BinarySensorDiscoveryModel {
                availability_topic = ClientContext.DeviceAvailabilityTopic(),
                state_topic = $"{ClientContext.EntityStateTopic( Domain, mConfiguration.EntityIdentifier )}/{Constants.Status}",
                name = Name,
                unique_id = mConfiguration.UniqueIdentifier,
                object_id = $"{ClientContext.DeviceIdentifier}_{mConfiguration.EntityIdentifier}",
                icon = mConfiguration.Icon,
                payload_on = mConfiguration.OnPayload,
                payload_off = mConfiguration.OffPayload,
                device = ClientContext.DeviceConfiguration,
                device_class = mConfiguration.DeviceClass,
                enabled_by_default = true,
                value_template = "{{value_json.value}}",
            };
        }

        public override string GetDiscoveryPayload() {
            if( GetDiscoveryModel() is BinarySensorDiscoveryModel discoveryModel ) {
                return discoveryModel.AsJson();
            }

            return String.Empty;
        }
    }
}
