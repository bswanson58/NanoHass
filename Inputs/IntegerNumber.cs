using Microsoft.Extensions.Logging;
using nanoFramework.Json;
using NanoHass.Discovery;
using NanoHass.Models;
using NanoHass.Support;
using System;
using System.Collections;

namespace NanoHass.Inputs {
    public class IntegerNumber : BaseInput {
        private readonly IntegerNumberConfiguration mConfiguration;
        private int                                 mValue;

        public IntegerNumber( IntegerNumberConfiguration configuration )
            : base( configuration.DisplayName, Constants.NumberDomain, configuration.EntityIdentifier, 
                    configuration.UpdateIntervalInSeconds ) {
            mConfiguration = configuration;

            SetState( mConfiguration.MinimumValue );
        }

        protected override BaseDiscoveryModel CreateDiscoveryModel() {
            if( ClientContext == null ) {
                return null;
            }

            return new NumberDiscoveryModel {
                availability_topic = ClientContext.DeviceAvailabilityTopic(),
                state_topic = $"{ClientContext.EntityStateTopic( Domain, mConfiguration.EntityIdentifier )}/{Constants.Status}",
                command_topic = $"{ClientContext.EntityStateTopic( Domain, mConfiguration.EntityIdentifier )}/{Constants.Subscribe}",
                name = Name,
                unique_id = mConfiguration.UniqueIdentifier,
                object_id = $"{ClientContext.DeviceIdentifier}_{mConfiguration.EntityIdentifier}",
                icon = mConfiguration.Icon,
                device = ClientContext.DeviceConfiguration,
                device_class = mConfiguration.DeviceClass,
                enabled_by_default = true,
                min = mConfiguration.MinimumValue,
                max = mConfiguration.MaximumValue,
                step = mConfiguration.StepValue,
                unit_of_measurement = mConfiguration.UnitOfMeasurement,
                value_template = "{{value_json.value}}",
            };
        }

        public override string GetDiscoveryPayload() {
            if( GetDiscoveryModel() is NumberDiscoveryModel discoveryModel ) {
                return discoveryModel.AsJson();
            }

            return String.Empty;
        }

        public override IList GetStatesToPublish() {
            var retValue = new ArrayList();

            if( GetDiscoveryModel() is  { } discoveryModel ) {
                if(!String.IsNullOrEmpty( discoveryModel.state_topic )) {
                    retValue.Add( new DeviceTopicState( discoveryModel.state_topic, GetStatePayload()));
                }
            }

            return retValue;
        }

        public override string GetStatePayload() =>
            JsonSerializer.SerializeObject( new Hashtable {{ Constants.PayloadValue, mValue }});

        public void SetState( int value ) =>
            mValue = value;

        public override bool ProcessMessage( string topic, string payload ) {
            if( GetDiscoveryModel() is NumberDiscoveryModel discoveryModel ) {
                if( topic.Equals( discoveryModel.command_topic )) {
                    OnStateCommand( payload );

                    return true;
                }
            }

            return false;
        }

        private void OnStateCommand( string payload ) {
            if(!String.IsNullOrEmpty( payload )) {
                try {
                    SetState( Int32.Parse( payload ));
                }
                catch( Exception ex ) {
                    ClientContext.Logger.Log( LogLevel.Error, ex, "Parsing integer state payload" );
                }
            }
        }

        public override object GetValue() =>
            mValue;

        public override void SetValue( object value ) {
            if( value is int integer ) {
                mValue = integer;

                TriggerStateChange();
            }
        }
    }
}
