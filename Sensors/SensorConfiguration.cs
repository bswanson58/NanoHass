using System;
using NanoPlat.Configuration.Metadata;

namespace NanoHass.Sensors {
    public class SensorConfiguration : ConfigurationSection {
        public  string      Name {  get; set;}
        public  string      SensorId { get; set; }
        public  string      DeviceClass { get; set; }
        public  string      MeasurementUnit { get; set; }
        public  int         UpdateIntervalInSeconds { get; set; }

        public SensorConfiguration() {
            Name = String.Empty;
            SensorId = String.Empty;
            UpdateIntervalInSeconds = 5;
        }

        public SensorConfiguration( string name, string id, string deviceClass, string measurementUnit, TimeSpan updateInterval ) {
            Name = name;
            SensorId = id;
            DeviceClass = deviceClass;
            MeasurementUnit = measurementUnit;
            UpdateIntervalInSeconds = (int)updateInterval.TotalSeconds;
        }

        public override ConfigurationDescription GetSectionDescription() =>
            new ( Name, new [] {
                new ParameterDescription( nameof( Name ), ParameterType.String, "Name of this device", 1 ),
                new ParameterDescription( nameof( SensorId ), ParameterType.String, "Sensor Identifier", 2 ),
                new ParameterDescription( nameof( DeviceClass ), ParameterType.String, "Device class of the sensor", 3 ),
                new ParameterDescription( nameof( MeasurementUnit ), ParameterType.String, "The sensor values unit of measurement", 4 ),
                new ParameterDescription( nameof( UpdateIntervalInSeconds ), ParameterType.Integer, "The interval (in seconds) to publish sensor value changes", 5 ),
            });
    }
}
