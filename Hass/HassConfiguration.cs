using System;
using NanoHass.Support;
using NanoPlat.Configuration.Metadata;

namespace NanoHass.Hass {
    public class HassConfiguration : ConfigurationSection {
        public  static string ConfigurationName => "HassClient";

        public  string      DeviceName { get; set; }
        public  string      DeviceIdentifier { get; set; }
        public  string      Manufacturer { get; set; }
        public  string      Model { get; set; }
        public  string      SoftwareVersion { get; set; }
        public  string      HardwareVersion { get; set; }
        public  string      DiscoveryPrefix { get; set; }
        public  string      Availability { get; set; }
        public  string      PayloadAvailable { get; set; }
        public  string      PayloadNotAvailable { get; set; }
        public  string      ConfigurationUrl { get; set; }
        public  string      SuggestedArea { get; set; }

        public HassConfiguration() {
            ConfigurationUrl = string.Empty;
            DeviceIdentifier = string.Empty;
            DeviceName = string.Empty;
            Manufacturer = string.Empty;
            Model = string.Empty;
            SoftwareVersion = string.Empty;
            HardwareVersion = string.Empty;
            DiscoveryPrefix = Constants.DiscoveryPrefix;
            Availability = Constants.Availability;
            PayloadAvailable = Constants.Online;
            PayloadNotAvailable = Constants.Offline;
            SuggestedArea = string.Empty;
        }

        public override ConfigurationDescription GetSectionDescription() =>
            new ( ConfigurationName, new [] {
                new ParameterDescription( nameof( DeviceName ), ParameterType.String, "The name of this device", 1 ),
                new ParameterDescription( nameof( DeviceIdentifier ), ParameterType.String, "The identifier for this device", 2 ),
                new ParameterDescription( nameof( Manufacturer ), ParameterType.String, "The manufacturer of this device", 4 ),
                new ParameterDescription( nameof( Model ), ParameterType.String, "The model of this device", 5 ),
                new ParameterDescription( nameof( SoftwareVersion ), ParameterType.String, "The software version of this device", 6 ),
                new ParameterDescription( nameof( DiscoveryPrefix ), ParameterType.String, "Home Assistant discovery prefix", 7 ),
                new ParameterDescription( nameof( Availability ), ParameterType.String, "Topic string indicating device availability", 8 ),
                new ParameterDescription( nameof( PayloadAvailable ), ParameterType.String, "Payload indicating device available", 9 ),
                new ParameterDescription( nameof( PayloadNotAvailable ), ParameterType.String, "Payload indicating device not available", 10 ),
                new ParameterDescription( nameof( ConfigurationUrl ), ParameterType.String, "Link to configure this device", 11 ),
                new ParameterDescription( nameof( SuggestedArea ), ParameterType.String, "The suggested area to associate this device with", 12 )
        });
    }
}
