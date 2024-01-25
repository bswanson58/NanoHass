// ReSharper disable InconsistentNaming
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace NanoHass.Discovery {
    /// <summary>
    /// Base configuration model for Home Assistant entities.
    /// </summary>
    public abstract class BaseDiscoveryModel {
        /// <summary>
        /// (Required) The MQTT topic subscribed to receive sensor values.
        /// </summary>
        /// <value></value>
        public string state_topic { get; set; }

        /// <summary>
        /// (Optional) Information about the device this sensor is a part of to tie it into the device registry.
        /// Only works through MQTT discovery and when unique_id is set.
        /// </summary>
        /// <value></value>
        public DeviceConfigModel device { get; set; }

        /// <summary>
        /// (Optional) The name of the MQTT sensor. Defaults to MQTT Sensor in hass.
        /// </summary>
        /// <value></value>
        public string name { get; set; }
    }
}
