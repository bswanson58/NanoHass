using System.Collections;
using Microsoft.Extensions.DependencyInjection;
using NanoHass.Sensors;
using NanoPlat.Builder;

namespace NanoHass.Hass {
    public class HassDeviceOptions {
        public  ArrayList   Devices { get; }

        public HassDeviceOptions() {
            Devices = new ArrayList();
        }

        public void AddDevice( SensorConfiguration configuration ) =>
            Devices.Add( configuration );
    }

    public delegate void ConfigurationHassDeviceDelegate( HassDeviceOptions options );

    public static class DeviceBuilderExtension {
        public static IDeviceBuilder AddHassIntegration( this IDeviceBuilder builder,
                                                         ConfigurationHassDeviceDelegate configurationAction) {
            var options = new HassDeviceOptions();

            configurationAction( options );

            builder.Services.AddSingleton( typeof( HassDeviceOptions ), options );
            builder.Services.AddSingleton( typeof( IHassManager ), typeof( HassMqttManager ));
            builder.Services.AddSingleton( typeof( ISensorManager ), typeof( SensorManager ));

            return builder;
        }
    }
}
