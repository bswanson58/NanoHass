using NanoHass.Discovery;
using NanoHass.Models;
using NanoHass.Support;
using System;
using System.Collections;
using nanoFramework.Json;

namespace NanoHass.Sensors {
    public class BaseSensor : AbstractDiscoverable {
        private const string                    cSensorName = "Unknown Sensor";

        private readonly SensorConfiguration    mConfiguration;
        private string                          mValue;

        protected BaseSensor( SensorConfiguration configuration ) :
            base( configuration.DisplayName ?? cSensorName, Constants.SensorDomain, configuration.Identifier,
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
                unique_id = mConfiguration.UniqueIdentifier,
                object_id = $"{ClientContext.DeviceIdentifier}_{mConfiguration.Identifier}",
                icon = mConfiguration.Icon,
                device = ClientContext.DeviceConfiguration,
                device_class = mConfiguration.DeviceClass,
                unit_of_measurement = mConfiguration.MeasurementUnit,
                value_template = "{{ value_json.value|default(0)}}",
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

        protected virtual string GetState() {
            return JsonSerializer.SerializeObject( new Hashtable {{ "value", mValue }});
        }

        public void SetValue( string value ) =>
            mValue = value;
    }
}
