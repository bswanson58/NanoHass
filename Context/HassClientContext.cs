using NanoHass.Discovery;
using NanoHass.Hass;
using NanoPlat.Configuration;
using NanoPlat.Mqtt;

namespace NanoHass.Context {
    public interface IHassClientContext {
        DeviceConfigModel   DeviceConfiguration { get; }

        string              ServerAddress { get; }
        string              UserName { get; }
        string              Password { get; }
        bool                UseMqttRetainFlag { get; }

        string              OnlinePayload { get; }
        string              OfflinePayload { get; }
        string              LastWillTopic { get; }
        string              LastWillPayload { get; }

        string              DeviceAvailabilityTopic();
        string              DeviceMessageSubscriptionTopic();

        string              DeviceBaseTopic( string forDomain );
    }

    public class HassClientContext : IHassClientContext {
        private readonly MqttConfiguration  mMqttConfiguration;
        private readonly HassConfiguration  mHassConfiguration;

        public  DeviceConfigModel           DeviceConfiguration { get; }

        public  string                      ServerAddress => mMqttConfiguration.BrokerAddress;
        public  string                      UserName => mMqttConfiguration.Username;
        public  string                      Password => mMqttConfiguration.Password;
        public  bool                        UseMqttRetainFlag => mMqttConfiguration.PublishRetain;

        public  string                      OnlinePayload => mHassConfiguration.PayloadAvailable;
        public  string                      OfflinePayload => mHassConfiguration.PayloadNotAvailable;

        public HassClientContext( IConfigurationManager configuration ) {
            mMqttConfiguration =
                (MqttConfiguration)configuration.GetConfiguration( MqttConfiguration.ConfigurationName, typeof( MqttConfiguration ));
            mHassConfiguration = 
                (HassConfiguration)configuration.GetConfiguration( HassConfiguration.ConfigurationName, typeof( HassConfiguration ));

            DeviceConfiguration = new DeviceConfigModel {
                manufacturer = mHassConfiguration.Manufacturer,
                name = mHassConfiguration.DeviceName,
                identifiers = mHassConfiguration.ClientIdentifier,
                model = mHassConfiguration.Model,
                sw_version = mHassConfiguration.Version
            };
        }

        public string LastWillTopic =>
            $"{mHassConfiguration.DiscoveryPrefix}/{DeviceConfiguration.name}/{DeviceConfiguration.identifiers}/{mHassConfiguration.Availability}";

        public string LastWillPayload =>
            mHassConfiguration.PayloadNotAvailable;

        public string DeviceBaseTopic( string forDomain ) =>
            $"{mHassConfiguration.DiscoveryPrefix}/{forDomain}/{DeviceConfiguration.name}";

        public string DeviceAvailabilityTopic() =>
            $"{mHassConfiguration.DiscoveryPrefix}/{DeviceConfiguration.name}/{DeviceConfiguration.identifiers}/{mHassConfiguration.Availability}";

        public string DeviceMessageSubscriptionTopic() =>
            $"{mHassConfiguration.DiscoveryPrefix}/+/{DeviceConfiguration.name}/#";
    }
}
