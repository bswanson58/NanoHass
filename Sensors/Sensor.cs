using NanoHass.Discovery;
using NanoHass.Support;
using System;

namespace NanoHass.Sensors {
    public class Sensor : BaseSensor {
        private readonly SensorConfiguration    mConfiguration;

        public Sensor( SensorConfiguration configuration )
            : base( configuration ) {
            mConfiguration = configuration;
        }

        protected override BaseDiscoveryModel CreateDiscoveryModel() {
            if( ClientContext == null ) {
                return null;
            }

            return new SensorDiscoveryModel {
                availability_topic = ClientContext.DeviceAvailabilityTopic(),
                state_topic = $"{ClientContext.EntityStateTopic( Domain, mConfiguration.EntityIdentifier )}/{Constants.Status}",
                name = Name,
                unique_id = mConfiguration.UniqueIdentifier,
                object_id = $"{ClientContext.DeviceIdentifier}_{mConfiguration.EntityIdentifier}",
                icon = mConfiguration.Icon,
                device = ClientContext.DeviceConfiguration,
                device_class = mConfiguration.DeviceClass,
                enabled_by_default = true,
                unit_of_measurement = mConfiguration.MeasurementUnit,
                value_template = "{{value_json.value}}",
            };
        }

        public override string GetDiscoveryPayload() {
            if( GetDiscoveryModel() is SensorDiscoveryModel discoveryModel ) {
                return discoveryModel.AsJson();
            }

            return String.Empty;
        }
    }
}
