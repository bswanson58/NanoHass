using NanoPlat.Configuration.Metadata;

namespace NanoHass.Hass {
    public class HassConfiguration : ConfigurationSection {
        public  static string ConfigurationName => "HassClient";

        public  string      DeviceName { get; set; }
        public  string      ClientIdentifier { get; set; }
        public  string      Manufacturer { get; set; }
        public  string      Model { get; set; }
        public  string      Version { get; set; }
        public  string      DiscoveryPrefix { get; set; }
        public  string      Availability { get; set; }
        public  string      PayloadAvailable { get; set; }
        public  string      PayloadNotAvailable { get; set; }

        public HassConfiguration() {
            DeviceName = string.Empty;
            ClientIdentifier = string.Empty;
            Manufacturer = string.Empty;
            Model = string.Empty;
            Version = string.Empty;
            // ReSharper disable once StringLiteralTypo
            DiscoveryPrefix = "homeassistant";
            Availability = "availability";
            PayloadAvailable = "online";
            PayloadNotAvailable = "offline";
        }

        public override ConfigurationDescription GetSectionDescription() =>
            new ( ConfigurationName, new [] {
                new ParameterDescription( nameof( DeviceName ), ParameterType.String, "Name of this device", 1 ),
                new ParameterDescription( nameof( ClientIdentifier ), ParameterType.String, "Client Identifier", 2 ),
                new ParameterDescription( nameof( Manufacturer ), ParameterType.String, "The manufacturer of this device", 3 ),
                new ParameterDescription( nameof( Model ), ParameterType.String, "The model of this device", 4 ),
                new ParameterDescription( nameof( Version ), ParameterType.String, "The version of this device", 5 ),
                new ParameterDescription( nameof( DiscoveryPrefix ), ParameterType.String, "Home Assistant discovery prefix", 6 ),
                new ParameterDescription( nameof( Availability ), ParameterType.String, "Topic string indicating device availability", 7 ),
                new ParameterDescription( nameof( PayloadAvailable ), ParameterType.String, "Payload indicating device available", 8 ),
                new ParameterDescription( nameof( PayloadNotAvailable ), ParameterType.String, "Payload indicating device not available", 9 ),
        });
    }
}
