using System.Collections;
using Microsoft.Extensions.DependencyInjection;
using NanoHass.Context;
using NanoHass.Sensors;
using NanoPlat.Builder;

namespace NanoHass.Hass {
    public class HassDeviceOptions {
        private readonly  ArrayList     mDevices;
        public  HassConfiguration       Configuration { get; }

        public HassDeviceOptions() {
            mDevices = new ArrayList();
            Configuration = new HassConfiguration();
        }

        public void AddSensor( SensorConfiguration configuration ) =>
            mDevices.Add( configuration );

        public ArrayList GetDevices() =>
            mDevices;
    }

    public delegate void ConfigurationHassDeviceDelegate( HassDeviceOptions options );

    public static class DeviceBuilderExtension {
        public static IDeviceBuilder AddHassIntegration( this IDeviceBuilder builder,
                                                         ConfigurationHassDeviceDelegate configurationAction) {
            var options = new HassDeviceOptions();

            configurationAction( options );

            builder.Services.AddSingleton( typeof( HassDeviceOptions ), options );
            builder.Services.AddSingleton( typeof( IHassManager ), typeof( HassMqttManager ));
            builder.Services.AddSingleton( typeof( IHassClientContext ), typeof( HassClientContext ));
            builder.Services.AddSingleton( typeof( ISensorManager ), typeof( SensorManager ));

            return builder;
        }
    }
}
