using System;
using NanoPlat.Configuration.Metadata;

namespace NanoHass.Sensors {
    public class SensorConfiguration : ConfigurationSection {
        public  string      DisplayName {  get; set;}
        public  string      UniqueIdentifier { get; set; }
        public  string      Identifier { get; set; }
        public  string      Icon { get; set; }
        public  string      DeviceClass { get; set; }
        public  string      MeasurementUnit { get; set; }
        public  int         UpdateIntervalInSeconds { get; set; }

        public SensorConfiguration() {
            DisplayName = String.Empty;
            Identifier = String.Empty;
            UniqueIdentifier = String.Empty;
            Icon = String.Empty;
            UpdateIntervalInSeconds = 5;
        }

        public SensorConfiguration( string displayName, string identifier, string uniqueIdentifier,
                                    string deviceClass = "", string measurementUnit = "", string icon = "",
                                    int updateInterval = 5 ) {
            DisplayName = displayName;
            Identifier = identifier;
            UniqueIdentifier = uniqueIdentifier;
            DeviceClass = deviceClass;
            MeasurementUnit = measurementUnit;
            UpdateIntervalInSeconds = updateInterval;
            Icon = icon;
        }

        public SensorConfiguration( string displayName, string identifier, string uniqueIdentifier, TimeSpan updateInterval,
                                    string deviceClass = "", string measurementUnit = "", string icon = "" ) {
            DisplayName = displayName;
            Identifier = identifier;
            UniqueIdentifier = uniqueIdentifier;
            DeviceClass = deviceClass;
            MeasurementUnit = measurementUnit;
            UpdateIntervalInSeconds = (int)updateInterval.TotalSeconds;
            Icon = icon;
        }

        public override ConfigurationDescription GetSectionDescription() =>
            new ( DisplayName, new [] {
                new ParameterDescription( nameof( DisplayName ), ParameterType.String, "Displayed name of this device", 1 ),
                new ParameterDescription( nameof( Identifier ), ParameterType.String, "Identifier for this device", 1 ),
                new ParameterDescription( nameof( DeviceClass ), ParameterType.String, "Device class of the sensor", 3 ),
                new ParameterDescription( nameof( MeasurementUnit ), ParameterType.String, "The sensor values unit of measurement", 4 ),
                new ParameterDescription( nameof( UpdateIntervalInSeconds ), ParameterType.Integer, "The interval (in seconds) to publish sensor value changes", 5 ),
                new ParameterDescription( nameof( Icon ), ParameterType.Integer, "The icon to be used for this sensor in Home Assistant", 6 ),
            });
    }
}
