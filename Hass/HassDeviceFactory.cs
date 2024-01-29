using NanoHass.Lighting;
using NanoHass.Support;

namespace NanoHass.Hass {
    public interface IHassDeviceFactory {
        BaseLight   CreateLight( LightConfiguration configuration );
    }

    public class HassDeviceFactory : IHassDeviceFactory {
        public virtual BaseLight CreateLight( LightConfiguration configuration ) {
            switch ( configuration.LightType ) {
                case var value when value == Constants.HslLightType:
                    return new HslLight( configuration );

                case var value when value == Constants.RgbLightType:
                    return new RgbLight( configuration );
            }

            return null;
        }
    }
}
