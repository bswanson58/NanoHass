using Microsoft.Extensions.Logging;
using NanoHass.Discovery;
using NanoHass.Hass;
using NanoHass.Support;
using NanoPlat.Configuration;

namespace NanoHass.Context {
    public interface IHassClientContext {
        DeviceConfigModel   DeviceConfiguration { get; }
        string              DeviceIdentifier { get; }

        string              OnlinePayload { get; }
        string              OfflinePayload { get; }

        string              DeviceTopic( string forDomain );
        string              DeviceAvailabilityTopic();
        string              DeviceMessageSubscriptionTopic();

        string              EntityTopic( string forDomain, string entityIdentifier );
        string              EntityConfigurationTopic( string forDomain, string entityIdentifier );
        string              EntityStateTopic( string forDomain, string entityIdentifier );

        ILogger             Logger { get; }
    }

    public class HassClientContext : IHassClientContext {
        private readonly HassConfiguration  mHassConfiguration;

        public  DeviceConfigModel           DeviceConfiguration { get; }
        public  string                      DeviceIdentifier => mHassConfiguration.DeviceIdentifier;

        public  string                      OnlinePayload => mHassConfiguration.PayloadAvailable;
        public  string                      OfflinePayload => mHassConfiguration.PayloadNotAvailable;

        public  ILogger                     Logger { get; }

        public HassClientContext( IConfigurationManager configuration, ILogger logger ) {
            Logger = logger;

            mHassConfiguration =
                (HassConfiguration)configuration.GetConfiguration( HassConfiguration.ConfigurationName, typeof( HassConfiguration ));

            DeviceConfiguration = new DeviceConfigModel {
                manufacturer = mHassConfiguration.Manufacturer,
                name = mHassConfiguration.DeviceName,
                identifiers = new []{ mHassConfiguration.DeviceIdentifier },
                model = mHassConfiguration.Model,
                sw_version = mHassConfiguration.SoftwareVersion,
                hw_version = mHassConfiguration.HardwareVersion,
                configuration_url = mHassConfiguration.ConfigurationUrl,
                suggested_area = mHassConfiguration.SuggestedArea
            };
        }

        public string LastWillTopic =>
            DeviceAvailabilityTopic();

        public string LastWillPayload =>
            mHassConfiguration.PayloadNotAvailable;

        public string DeviceTopic( string forDomain ) =>
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
            $"{mHassConfiguration.DiscoveryPrefix}/+/{mHassConfiguration.DeviceIdentifier}/#";
    }
}
