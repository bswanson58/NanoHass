using NanoPlat.Configuration.Metadata;
using System;
using NanoHass.Support;

namespace NanoHass.Sensors {
    public class BinarySensorConfiguration : ConfigurationSection {
        public  string      DisplayName {  get; set;}
        public  string      UniqueIdentifier { get; set; }
        public  string      EntityIdentifier { get; set; }
        public  string      Icon { get; set; }
        public  string      DeviceClass { get; set; }
        public  string      OnPayload { get; set; }
        public  string      OffPayload { get; set; }
        public  int         UpdateIntervalInSeconds { get; set; }

        public BinarySensorConfiguration() {
            DisplayName = String.Empty;
            EntityIdentifier = String.Empty;
            UniqueIdentifier = String.Empty;
            Icon = String.Empty;
            OnPayload = Constants.OnState;
            OffPayload = Constants.OffState;
            UpdateIntervalInSeconds = 5;
        }

        public BinarySensorConfiguration( string displayName, string entityIdentifier, string uniqueIdentifier,
                                          string deviceClass = "", string measurementUnit = "", string icon = "",
                                          string onPayload = "", string offPayload = "",
                                          int updateInterval = 5 ) {
            DisplayName = displayName;
            EntityIdentifier = entityIdentifier;
            UniqueIdentifier = uniqueIdentifier;
            DeviceClass = deviceClass;
            OnPayload = String.IsNullOrEmpty( onPayload ) ? Constants.OnState : onPayload;
            OffPayload = String.IsNullOrEmpty( offPayload ) ? Constants.OffState : offPayload;
            UpdateIntervalInSeconds = updateInterval;
            Icon = icon;
        }

        public BinarySensorConfiguration( string displayName, string entityIdentifier, string uniqueIdentifier, 
                                          TimeSpan updateInterval,
                                          string deviceClass = "", string measurementUnit = "", string icon = "",
                                          string onPayload = "", string offPayload = "" ) {
            DisplayName = displayName;
            EntityIdentifier = entityIdentifier;
            UniqueIdentifier = uniqueIdentifier;
            DeviceClass = deviceClass;
            OnPayload = String.IsNullOrEmpty( onPayload ) ? Constants.OnState : onPayload;
            OffPayload = String.IsNullOrEmpty( offPayload ) ? Constants.OffState : offPayload;
            UpdateIntervalInSeconds = (int)updateInterval.TotalSeconds;
            Icon = icon;
        }

        public override ConfigurationDescription GetSectionDescription() =>
            new ( DisplayName, new [] {
                new ParameterDescription( nameof( DisplayName ), ParameterType.String, "Displayed name of this device", 1 ),
                new ParameterDescription( nameof( EntityIdentifier ), ParameterType.String, "Home Assistant entity identifier for this device", 2 ),
                new ParameterDescription( nameof( UniqueIdentifier ), ParameterType.String, "The globally unique identifier for this device", 3 ),
                new ParameterDescription( nameof( DeviceClass ), ParameterType.String, "Device class of the sensor", 4 ),
                new ParameterDescription( nameof( OnPayload ), ParameterType.String, "String indicating the sensor is on", 5 ),
                new ParameterDescription( nameof( OffPayload ), ParameterType.String, "String indicating the sensor is off", 6 ),
                new ParameterDescription( nameof( UpdateIntervalInSeconds ), ParameterType.Integer, "The interval (in seconds) to publish sensor value changes", 7 ),
                new ParameterDescription( nameof( Icon ), ParameterType.Integer, "The icon to be used for this sensor in Home Assistant", 8 ),
            });
    }
}
