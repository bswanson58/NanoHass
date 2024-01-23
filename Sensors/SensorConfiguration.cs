using System;
using NanoPlat.Configuration.Metadata;

namespace NanoHass.Sensors {
    public class SensorConfiguration : ConfigurationSection {
        public  string      Name {  get; set;}
        public  string      Icon { get; set; }
        public  string      DeviceClass { get; set; }
        public  string      MeasurementUnit { get; set; }
        public  int         UpdateIntervalInSeconds { get; set; }

        public SensorConfiguration() {
            Name = String.Empty;
            Icon = String.Empty;
            UpdateIntervalInSeconds = 5;
        }

        public SensorConfiguration( string name, 
                                    string deviceClass = "", string measurementUnit = "", string icon = "",
                                    int updateInterval = 5 ) {
            Name = name;
            DeviceClass = deviceClass;
            MeasurementUnit = measurementUnit;
            UpdateIntervalInSeconds = updateInterval;
            Icon = icon;
        }

        public SensorConfiguration( string name, TimeSpan updateInterval,
                                    string deviceClass = "", string measurementUnit = "", string icon = "" ) {
            Name = name;
            DeviceClass = deviceClass;
            MeasurementUnit = measurementUnit;
            UpdateIntervalInSeconds = (int)updateInterval.TotalSeconds;
            Icon = icon;
        }

        public override ConfigurationDescription GetSectionDescription() =>
            new ( Name, new [] {
                new ParameterDescription( nameof( Name ), ParameterType.String, "Name of this device", 1 ),
                new ParameterDescription( nameof( DeviceClass ), ParameterType.String, "Device class of the sensor", 3 ),
                new ParameterDescription( nameof( MeasurementUnit ), ParameterType.String, "The sensor values unit of measurement", 4 ),
                new ParameterDescription( nameof( UpdateIntervalInSeconds ), ParameterType.Integer, "The interval (in seconds) to publish sensor value changes", 5 ),
                new ParameterDescription( nameof( Icon ), ParameterType.Integer, "The icon to be used for this sensor in Home Assistant", 6 ),
            });
    }
}
