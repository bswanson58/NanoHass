using NanoHass.Discovery;
using NanoHass.Hass;
using NanoHass.Support;

namespace NanoHass.Context {
    public interface IHassClientContext {
        DeviceConfigModel   DeviceConfiguration { get; }
        string              DeviceIdentifier { get; }

        string              OnlinePayload { get; }
        string              OfflinePayload { get; }

        string              DeviceAvailabilityTopic();
        string              DeviceMessageSubscriptionTopic();

        string              EntityTopic( string forDomain, string entityIdentifier );
        string              EntityConfigurationTopic( string forDomain, string entityIdentifier );
        string              EntityStateTopic( string forDomain, string entityIdentifier );
    }

    public class HassClientContext : IHassClientContext {
        private readonly HassConfiguration  mHassConfiguration;

        public  DeviceConfigModel           DeviceConfiguration { get; }
        public  string                      DeviceIdentifier => mHassConfiguration.DeviceIdentifier;

        public  string                      OnlinePayload => mHassConfiguration.PayloadAvailable;
        public  string                      OfflinePayload => mHassConfiguration.PayloadNotAvailable;

        public HassClientContext( HassDeviceOptions options ) {
            mHassConfiguration = options.Configuration;

            DeviceConfiguration = new DeviceConfigModel {
                manufacturer = options.Configuration.Manufacturer,
                name = options.Configuration.DeviceName,
                identifiers = new []{ options.Configuration.DeviceIdentifier },
                model = options.Configuration.Model,
                sw_version = options.Configuration.SoftwareVersion,
                hw_version = options.Configuration.HardwareVersion,
                configuration_url = options.Configuration.ConfigurationUrl,
                suggested_area = options.Configuration.SuggestedArea
            };
        }

        public string LastWillTopic =>
            DeviceAvailabilityTopic();

        public string LastWillPayload =>
            mHassConfiguration.PayloadNotAvailable;

        private string DeviceTopic( string forDomain ) =>
            $"{mHassConfiguration.DiscoveryPrefix}/{forDomain}/{mHassConfiguration.DeviceIdentifier}";

        public string EntityTopic( string forDomain, string entityIdentifier ) =>
            $"{DeviceTopic( forDomain )}/{entityIdentifier}";

        public string EntityStateTopic( string forDomain, string entityIdentifier ) =>
            $"{EntityTopic( forDomain, entityIdentifier)}/{Constants.State}";

        public string EntityConfigurationTopic( string forDomain, string entityIdentifier ) =>
            $"{DeviceTopic( forDomain )}/{entityIdentifier}/{Constants.Configuration}";

        public string DeviceAvailabilityTopic() =>
            $"{mHassConfiguration.DiscoveryPrefix}/{mHassConfiguration.DeviceIdentifier}/{mHassConfiguration.Availability}";

        public string DeviceMessageSubscriptionTopic() =>
            $"{mHassConfiguration.DiscoveryPrefix}/+/{DeviceConfiguration.name}/#";
    }
}
