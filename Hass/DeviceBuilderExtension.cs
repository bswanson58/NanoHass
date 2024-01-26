using System.Collections;
using Microsoft.Extensions.DependencyInjection;
using NanoHass.Context;
using NanoHass.Lighting;
using NanoHass.Sensors;
using NanoPlat.Builder;

namespace NanoHass.Hass {
    public class HassDeviceOptions {
        private readonly ArrayList      mSensors;
        private readonly ArrayList      mLights;
        public  HassConfiguration       Configuration { get; }

        public HassDeviceOptions() {
            mSensors = new ArrayList();
            mLights = new ArrayList();
            Configuration = new HassConfiguration();
        }

        public void AddSensor( SensorConfiguration configuration ) =>
            mSensors.Add( configuration );

        public void AddBinarySensor( BinarySensorConfiguration configuration ) =>
            mSensors.Add( configuration );

        public void AddLight( LightConfiguration configuration ) =>
            mLights.Add( configuration );

        public ArrayList GetSensors() =>
            mSensors;

        public ArrayList GetLights() =>
            mLights;
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
            builder.Services.AddSingleton( typeof( ILightManager ), typeof( LightManager ));
            builder.Services.AddSingleton( typeof( ISensorManager ), typeof( SensorManager ));

            return builder;
        }
    }
}
