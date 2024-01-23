using NanoHass.Discovery;
using NanoHass.Hass;
using NanoHass.Support;
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

        string              SensorStateTopic( string forDomain, string sensorName );
        string              SensorConfigurationTopic( string forDomain );
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

        public HassClientContext( IConfigurationManager configuration, HassDeviceOptions options ) {
            mMqttConfiguration =
                (MqttConfiguration)configuration.GetConfiguration( MqttConfiguration.ConfigurationName, typeof( MqttConfiguration ));
            mHassConfiguration = options.Configuration;

            DeviceConfiguration = new DeviceConfigModel {
                manufacturer = options.Configuration.Manufacturer,
                name = options.Configuration.DeviceIdentifier,
                identifiers = options.Configuration.DeviceIdentifier,
                model = options.Configuration.Model,
                sw_version = options.Configuration.Version
            };
        }

        public string LastWillTopic =>
            DeviceAvailabilityTopic();

        public string LastWillPayload =>
            mHassConfiguration.PayloadNotAvailable;

        private string SensorTopic( string forDomain ) =>
            $"{mHassConfiguration.DiscoveryPrefix}/{forDomain}/{mHassConfiguration.DeviceIdentifier}";

        public string SensorStateTopic( string forDomain, string sensorName ) =>
            $"{SensorTopic( forDomain )}/{sensorName}/{Constants.State}";

        public string SensorConfigurationTopic( string forDomain ) =>
            $"{SensorTopic( forDomain )}/{Constants.Configuration}";

        public string DeviceAvailabilityTopic() =>
            $"{mHassConfiguration.DiscoveryPrefix}/{mHassConfiguration.DeviceName}/{mHassConfiguration.DeviceIdentifier}/{mHassConfiguration.Availability}";

        public string DeviceMessageSubscriptionTopic() =>
            $"{mHassConfiguration.DiscoveryPrefix}/+/{DeviceConfiguration.name}/#";
    }
}
