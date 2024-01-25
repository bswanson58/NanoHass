using System;
using NanoPlat.Configuration.Metadata;

namespace NanoHass.Sensors {
    public class SensorConfiguration : ConfigurationSection {
        public  string      DisplayName {  get; set;}
        public  string      UniqueIdentifier { get; set; }
        public  string      EntityIdentifier { get; set; }
        public  string      Icon { get; set; }
        public  string      DeviceClass { get; set; }
        public  string      MeasurementUnit { get; set; }
        public  int         UpdateIntervalInSeconds { get; set; }

        public SensorConfiguration() {
            DisplayName = String.Empty;
            EntityIdentifier = String.Empty;
            UniqueIdentifier = String.Empty;
            Icon = String.Empty;
            UpdateIntervalInSeconds = 5;
        }

        public SensorConfiguration( string displayName, string entityIdentifier, string uniqueIdentifier,
                                    string deviceClass = "", string measurementUnit = "", string icon = "",
                                    int updateInterval = 5 ) {
            DisplayName = displayName;
            EntityIdentifier = entityIdentifier;
            UniqueIdentifier = uniqueIdentifier;
            DeviceClass = deviceClass;
            MeasurementUnit = measurementUnit;
            UpdateIntervalInSeconds = updateInterval;
            Icon = icon;
        }

        public SensorConfiguration( string displayName, string entityIdentifier, string uniqueIdentifier, TimeSpan updateInterval,
                                    string deviceClass = "", string measurementUnit = "", string icon = "" ) {
            DisplayName = displayName;
            EntityIdentifier = entityIdentifier;
            UniqueIdentifier = uniqueIdentifier;
            DeviceClass = deviceClass;
            MeasurementUnit = measurementUnit;
            UpdateIntervalInSeconds = (int)updateInterval.TotalSeconds;
            Icon = icon;
        }

        public override ConfigurationDescription GetSectionDescription() =>
            new ( DisplayName, new [] {
                new ParameterDescription( nameof( DisplayName ), ParameterType.String, "Displayed name of this device", 1 ),
                new ParameterDescription( nameof( EntityIdentifier ), ParameterType.String, "Home Assistant entity identifier for this device", 2 ),
                new ParameterDescription( nameof( UniqueIdentifier ), ParameterType.String, "The globally unique identifier for this device", 3 ),
                new ParameterDescription( nameof( DeviceClass ), ParameterType.String, "Device class of the sensor", 4 ),
                new ParameterDescription( nameof( MeasurementUnit ), ParameterType.String, "The sensor values unit of measurement", 5 ),
                new ParameterDescription( nameof( UpdateIntervalInSeconds ), ParameterType.Integer, "The interval (in seconds) to publish sensor value changes", 6 ),
                new ParameterDescription( nameof( Icon ), ParameterType.Integer, "The icon to be used for this sensor in Home Assistant", 7 ),
            });
    }
}
