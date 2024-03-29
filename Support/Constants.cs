﻿namespace NanoHass.Support {
    public static class Constants {
        // ReSharper disable once StringLiteralTypo
        public  static readonly string      DiscoveryPrefix = "homeassistant";

        public  static readonly string      SensorDomain = "sensor";
        public  static readonly string      BinarySensorDomain = "binary_sensor";
        public  static readonly string      LightDomain = "light";
        public  static readonly string      NumberDomain = "number";

        public  static readonly string      Availability = "availability";
        public  static readonly string      Configuration = "config";
        public  static readonly string      Brightness = "brightness";
        public  static readonly string      HslColor = "hsl_color";
        public  static readonly string      RgbColor = "rgb_color";

        public  static readonly string      State = "state";
        public  static readonly string      Online = "online";
        public  static readonly string      Offline = "offline";

        public  static readonly string      Status = "get";
        public  static readonly string      Subscribe = "set";

        public  static readonly string      OnState = "ON";
        public  static readonly string      OffState = "OFF";

        public  static readonly string      PayloadValue = "value";

        public  static readonly string      HslLightType = "hsl";
        public  static readonly string      RgbLightType = "rgb";
    }
}
