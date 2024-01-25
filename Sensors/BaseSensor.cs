using NanoHass.Discovery;
using NanoHass.Models;
using NanoHass.Support;
using System;
using System.Collections;
using nanoFramework.Json;

namespace NanoHass.Sensors {
    public abstract class BaseSensor : AbstractDiscoverable {
        private const string    cSensorName = "Unknown Sensor";

        private string          mState;

        protected BaseSensor( SensorConfiguration configuration ) :
            base( configuration.DisplayName ?? cSensorName, Constants.SensorDomain, configuration.EntityIdentifier,
                  configuration.UpdateIntervalInSeconds ) {
            mState = String.Empty;
        }

        protected BaseSensor( BinarySensorConfiguration configuration ) :
            base( configuration.DisplayName ?? cSensorName, Constants.BinarySensorDomain, configuration.EntityIdentifier,
                  configuration.UpdateIntervalInSeconds ) {
            mState = String.Empty;
        }

        public override IList GetStatesToPublish() {
            var retValue = new ArrayList();

            if( GetDiscoveryModel() is  { } discoveryModel ) {
                if(!String.IsNullOrEmpty( discoveryModel.state_topic )) {
                    retValue.Add( new DeviceTopicState( discoveryModel.state_topic, GetState()));
                }
            }

            return retValue;
        }

        public override string GetState() =>
            JsonSerializer.SerializeObject( new Hashtable {{ Constants.PayloadValue, mState }});

        public void SetState( string value ) =>
            mState = value;
    }
}
