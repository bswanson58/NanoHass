using NanoPlat.Configuration.Metadata;
using System;

namespace NanoHass.Inputs {
    public class IntegerNumberConfiguration : ConfigurationSection {
        public  string      DisplayName {  get; set;}
        public  string      UniqueIdentifier { get; set; }
        public  string      EntityIdentifier { get; set; }
        public  string      Icon { get; set; }
        public  string      DeviceClass { get; set; }
        public  int         UpdateIntervalInSeconds { get; set; }
        public  int         MaximumValue { get; set; }
        public  int         MinimumValue { get; set; }
        public  int         StepValue { get; set; }
        public  string      UnitOfMeasurement { get; set; }

        public IntegerNumberConfiguration() {
            DisplayName = String.Empty;
            EntityIdentifier = String.Empty;
            UniqueIdentifier = String.Empty;
            Icon = String.Empty;
            MinimumValue = 0;
            MaximumValue = 100;
            StepValue = 1;
            UnitOfMeasurement = String.Empty;
            UpdateIntervalInSeconds = 5;
        }

        public IntegerNumberConfiguration( string displayName, string entityIdentifier, string uniqueIdentifier,
                                          string deviceClass = "", string unitOfMeasurement = "", string icon = "",
                                          int minimum = 0, int maximum = 100, int step = 1,
                                          int updateInterval = 5 ) {
            DisplayName = displayName;
            EntityIdentifier = entityIdentifier;
            UniqueIdentifier = uniqueIdentifier;
            DeviceClass = deviceClass;
            UpdateIntervalInSeconds = updateInterval;
            Icon = icon;
            MinimumValue = minimum;
            MaximumValue = maximum;
            StepValue = step;
            UnitOfMeasurement = unitOfMeasurement;
        }

        public IntegerNumberConfiguration( string displayName, string entityIdentifier, string uniqueIdentifier, 
                                          TimeSpan updateInterval,
                                          string deviceClass = "", string unitOfMeasurement = "", string icon = "",
                                          int minimum = 0, int maximum = 100, int step = 1 ) {
            DisplayName = displayName;
            EntityIdentifier = entityIdentifier;
            UniqueIdentifier = uniqueIdentifier;
            DeviceClass = deviceClass;
            UpdateIntervalInSeconds = (int)updateInterval.TotalSeconds;
            Icon = icon;
            MinimumValue = minimum;
            MaximumValue = maximum;
            StepValue = step;
            UnitOfMeasurement = unitOfMeasurement;
        }

        public override ConfigurationDescription GetSectionDescription() =>
            new ( DisplayName, new [] {
                new ParameterDescription( nameof( DisplayName ), ParameterType.String, "Displayed name of this device", 1 ),
                new ParameterDescription( nameof( EntityIdentifier ), ParameterType.String, "Home Assistant entity identifier for this device", 2 ),
                new ParameterDescription( nameof( UniqueIdentifier ), ParameterType.String, "The globally unique identifier for this device", 3 ),
                new ParameterDescription( nameof( MinimumValue ), ParameterType.Integer, "The minimum valid value", 4 ),
                new ParameterDescription( nameof( MaximumValue ), ParameterType.Integer, "The maximum valid value", 5 ),
                new ParameterDescription( nameof( StepValue ), ParameterType.Integer, "The step value to increase/decrease", 6 ),
                new ParameterDescription( nameof( UnitOfMeasurement ), ParameterType.String, "The unit of measurement", 7 ),
                new ParameterDescription( nameof( DeviceClass ), ParameterType.String, "Device class of the sensor", 8 ),
                new ParameterDescription( nameof( UpdateIntervalInSeconds ), ParameterType.Integer, "The interval (in seconds) to publish sensor value changes", 9 ),
                new ParameterDescription( nameof( Icon ), ParameterType.Integer, "The icon to be used for this sensor in Home Assistant", 10 ),
            });
    }
}
