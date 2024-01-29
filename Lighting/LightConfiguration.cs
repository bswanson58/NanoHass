using NanoPlat.Configuration.Metadata;
using System;
using NanoHass.Support;

namespace NanoHass.Lighting {
    public class LightConfiguration : ConfigurationSection {
        public  string      DisplayName {  get; set;}
        public  string      UniqueIdentifier { get; set; }
        public  string      EntityIdentifier { get; set; }
        public  string      Icon { get; set; }
        public  int         UpdateIntervalInSeconds { get; set; }
        public  string      LightType { get; set; }

        public LightConfiguration() {
            DisplayName = String.Empty;
            EntityIdentifier = String.Empty;
            UniqueIdentifier = String.Empty;
            Icon = String.Empty;
            UpdateIntervalInSeconds = 5;
            LightType = Constants.RgbLightType;
        }

        public LightConfiguration( string displayName, string entityIdentifier, string uniqueIdentifier,
                                   string lightType = "rgb", string icon = "", int updateInterval = 5 ) {
            DisplayName = displayName;
            EntityIdentifier = entityIdentifier;
            UniqueIdentifier = uniqueIdentifier;
            Icon = icon;
            UpdateIntervalInSeconds = updateInterval;
            LightType = lightType;
        }

        public override ConfigurationDescription GetSectionDescription() =>
            new ( DisplayName, new [] {
                new ParameterDescription( nameof( DisplayName ), ParameterType.String, "Displayed name of this device", 1 ),
                new ParameterDescription( nameof( EntityIdentifier ), ParameterType.String, "Home Assistant entity identifier for this device", 2 ),
                new ParameterDescription( nameof( UniqueIdentifier ), ParameterType.String, "The globally unique identifier for this device", 3 ),
                new ParameterDescription( nameof( Icon ), ParameterType.Integer, "The icon to be used for this sensor in Home Assistant", 4 ),
                new ParameterDescription( nameof( UpdateIntervalInSeconds ), ParameterType.Integer, "The interval (in seconds) to publish sensor value changes", 5 ),
                new ParameterDescription( nameof( LightType ), ParameterType.String, "The type of light to be create (hsl/rgb)", 6 )
            });
    }
}
