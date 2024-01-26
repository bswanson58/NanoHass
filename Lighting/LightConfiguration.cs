using NanoPlat.Configuration.Metadata;
using System;

namespace NanoHass.Lighting {
    public class LightConfiguration : ConfigurationSection {
        public  string      DisplayName {  get; set;}
        public  string      UniqueIdentifier { get; set; }
        public  string      EntityIdentifier { get; set; }
        public  string      Icon { get; set; }
        public  int         UpdateIntervalInSeconds { get; set; }

        public LightConfiguration() {
            DisplayName = String.Empty;
            EntityIdentifier = String.Empty;
            UniqueIdentifier = String.Empty;
            Icon = String.Empty;
            UpdateIntervalInSeconds = 5;
        }

        public LightConfiguration( string displayName, string entityIdentifier, string uniqueIdentifier,
                                   string deviceClass = "", string icon = "", int updateInterval = 5 ) {
            DisplayName = displayName;
            EntityIdentifier = entityIdentifier;
            UniqueIdentifier = uniqueIdentifier;
            Icon = icon;
            UpdateIntervalInSeconds = updateInterval;
        }

        public override ConfigurationDescription GetSectionDescription() =>
            new ( DisplayName, new [] {
                new ParameterDescription( nameof( DisplayName ), ParameterType.String, "Displayed name of this device", 1 ),
                new ParameterDescription( nameof( EntityIdentifier ), ParameterType.String, "Home Assistant entity identifier for this device", 2 ),
                new ParameterDescription( nameof( UniqueIdentifier ), ParameterType.String, "The globally unique identifier for this device", 3 ),
                new ParameterDescription( nameof( Icon ), ParameterType.Integer, "The icon to be used for this sensor in Home Assistant", 4 ),
                new ParameterDescription( nameof( UpdateIntervalInSeconds ), ParameterType.Integer, "The interval (in seconds) to publish sensor value changes", 5 )
            });
    }
}
