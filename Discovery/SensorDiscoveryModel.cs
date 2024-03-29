﻿// ReSharper disable InconsistentNaming
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace NanoHass.Discovery {
    // from: https://www.home-assistant.io/integrations/sensor.mqtt/
    // device classes: https://www.home-assistant.io/integrations/sensor/
    // constants, units of measure: https://github.com/home-assistant/core/blob/d7ac4bd65379e11461c7ce0893d3533d8d8b8cbf/homeassistant/const.py

    public class SensorDiscoveryModel : BaseDiscoveryModel {
        /// <summary>
        /// (Optional) The MQTT topic subscribed to receive availability (online/offline) updates.
        /// </summary>
        /// <value></value>
        public string availability_topic { get; set; }

        /// <summary>
        /// (Optional) The type/class of the sensor to set the icon in the frontend.
        /// See https://www.home-assistant.io/integrations/sensor/#device-class for options.
        /// </summary>
        /// <value></value>
        public string device_class { get; set; }

        /// <summary>
        /// (Optional, default: true) Flag which defines if the entity should be enabled when first added.
        /// </summary>
        public bool enabled_by_default { get; set; }

        /// <summary>
        /// (Optional) Defines the number of seconds after the sensor’s state expires, if it’s not updated.
        /// After expiry, the sensor’s state becomes unavailable. Defaults to 0 in hass.
        /// </summary>
        /// <value></value>
        public int expire_after { get; set; }

        /// <summary>
        /// Sends update events even if the value hasn't changed.
        /// Useful if you want to have meaningful value graphs in history.
        /// </summary>
        /// <value></value>
        public bool force_update { get; set; }

        /// <summary>
        /// (Optional) The icon for the sensor.
        /// </summary>
        /// <value></value>
        public string icon { get; set; }

        /// <summary>
        /// (Optional) Defines a template to extract the JSON dictionary from messages received on the json_attributes_topic.
        /// </summary>
        /// <value></value>
        public string json_attributes_template { get; set; }

        /// <summary>
        /// (Optional) The MQTT topic subscribed to receive a JSON dictionary payload and then set as sensor attributes.
        /// Implies force_update of the current sensor state when a message is received on this topic.
        /// </summary>
        /// <value></value>
        public string json_attributes_topic { get; set; }

        /// <summary>
        /// (Optional) The maximum QoS level of the state topic.
        /// </summary>
        /// <value></value>
        public int qos { get; set; }

        /// <summary>
        /// (Optional) An ID that uniquely identifies this sensor.
        /// Used instead of Name to generate the entity_id.
        /// If two sensors have the same unique ID, Home Assistant will raise an exception.
        /// </summary>
        /// <value></value>
        public string unique_id { get; set; }

        /// <summary>
        /// (Optional) Used instead of name for automatic generation of entity_id
        /// </summary>
        public string object_id { get; set; }

        /// <summary>
        /// (Optional) Defines the units of measurement of the sensor, if any.
        /// </summary>
        /// <value></value>
        public string unit_of_measurement { get; set; }

        /// <summary>
        /// (Optional) Defines a template to extract the value.
        /// </summary>
        /// <value></value>
        public string value_template { get; set; }
    }
}
