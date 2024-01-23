using NanoHass.Discovery;
using NanoHass.Models;
using NanoHass.Support;
using System;
using System.Collections;

namespace NanoHass.Sensors {
    public class BaseSensor : AbstractDiscoverable {
        private const string                    cSensorName = "unknown sensor";

        private readonly SensorConfiguration    mConfiguration;
        private string                          mValue;

        protected BaseSensor( SensorConfiguration configuration ) :
            base( configuration.Name ?? cSensorName, Constants.SensorDomain, configuration.Identifier,
                  configuration.UpdateIntervalInSeconds ) {
            mConfiguration = configuration;
            mValue = String.Empty;
        }

        protected override BaseDiscoveryModel CreateDiscoveryModel() {
            if( ClientContext == null ) {
                return null;
            }

            return new SensorDiscoveryModel {
                availability_topic = ClientContext.DeviceAvailabilityTopic(),
                state_topic = $"{ClientContext.SensorStateTopic( Domain, mConfiguration.Identifier )}",
                name = Name,
                unique_id = Id,
                icon = mConfiguration.Icon,
                device = ClientContext.DeviceConfiguration,
                device_class = mConfiguration.DeviceClass,
                unit_of_measurement = mConfiguration.MeasurementUnit,
                json_attributes_topic = String.Empty,
                json_attributes_template = String.Empty,
                value_template = String.Empty, // {{ value_json.humidity}}
            };
        }

        public override IList GetStatesToPublish() {
            var retValue = new ArrayList();

            if( GetDiscoveryModel() is SensorDiscoveryModel discoveryModel ) {
                if(!String.IsNullOrEmpty( discoveryModel.state_topic )) {
                    retValue.Add( new DeviceTopicState( discoveryModel.state_topic, GetState()));
                }
            }

            return retValue;
        }

        public override string GetCombinedState() =>
            GetState();

        protected virtual string GetState() =>
            mValue;

        public void SetValue( string value ) =>
            mValue = value;
    }
}
