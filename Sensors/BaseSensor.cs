using NanoHass.Discovery;
using NanoHass.Models;
using NanoHass.Support;
using System;
using System.Collections;

namespace NanoHass.Sensors {
    public class BaseSensor : AbstractDiscoverable {
        private const string   cSensorName = "unknown sensor";

        public BaseSensor( string name, 
                           int updateIntervalSeconds = 10, string id = default, bool useAttributes = false ) :
            base( name ?? cSensorName, Constants.SensorDomain, updateIntervalSeconds, id, useAttributes ) { }

        protected override BaseDiscoveryModel CreateDiscoveryModel() {
            if( ClientContext == null ) {
                return null;
            }

            return new SensorDiscoveryModel {
                availability_topic = ClientContext.DeviceAvailabilityTopic(),
                name = Name,
                unique_id = Id,
                device = ClientContext.DeviceConfiguration,
                state_topic = $"{ClientContext.DeviceBaseTopic( Domain )}/{ObjectId}/{Constants.State}/{Constants.Status}"
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
            String.Empty;
    }
}
