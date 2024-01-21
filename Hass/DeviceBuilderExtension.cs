using Microsoft.Extensions.DependencyInjection;
using NanoPlat.Builder;

namespace NanoHass.Hass {
    public static class DeviceBuilderExtension {
        public static IDeviceBuilder AddHassIntegration( this IDeviceBuilder builder ) {
            builder.Services.AddSingleton( typeof( IHassManager ), typeof( HassMqttManager ));

            return builder;
        }
    }
}
