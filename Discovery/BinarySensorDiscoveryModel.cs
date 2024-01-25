namespace NanoHass.Discovery {

    // from: https://www.home-assistant.io/integrations/binary_sensor.mqtt/
    // device classes: https://www.home-assistant.io/integrations/binary_sensor/
    // constants, units of measure: https://github.com/home-assistant/core/blob/d7ac4bd65379e11461c7ce0893d3533d8d8b8cbf/homeassistant/const.py

    public class BinarySensorDiscoveryModel : BaseDiscoveryModel {
        /// <summary>
        /// (Optional) The MQTT topic subscribed to receive availability (online/offline) updates.
        /// </summary>
        /// <value></value>
        public string availability_topic { get; set; }

        /// <summary>
        /// (Optional) When availability is configured, this controls the conditions needed to set the entity to available.
        /// Valid entries are all, any, and latest. If set to all, payload_available must be received on all
        /// configured availability topics before the entity is marked as online.
        /// If set to any, payload_available must be received on at least one configured availability topic
        /// before the entity is marked as online. If set to latest, the last payload_available or
        /// payload_not_available received on any configured availability topic controls the availability.
        /// </summary>
        public string availability_mode { get; set; }

        /// <summary>
        /// (Optional) Defines a template to extract device’s availability from the availability_topic.
        /// To determine the devices’s availability result of this template will be compared to payload_available
        /// and payload_not_available.
        /// </summary>
        public string availability_template { get; set; }

        /// <summary>
        /// (Optional) The type/class of the sensor to set the icon in the frontend.
        /// See https://www.home-assistant.io/integrations/sensor/#device-class for options.
        /// </summary>
        /// <value></value>
        public string device_class { get; set; }

        /// <summary>
        /// (Optional) Defines the number of seconds after the sensor’s state expires, if it’s not updated.
        /// After expiry, the sensor’s state becomes unavailable. Defaults to 0 in hass.
        /// </summary>
        /// <value></value>
        public int expire_after { get; set; }

        /// <summary>
        /// (Optional, default: false) Sends update events even if the value hasn't changed.
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
        /// (Optional) Defines a template to extract the value.
        /// </summary>
        /// <value></value>
        public string value_template { get; set; }

        /// <summary>
        /// (Optional, default: true) Flag which defines if the entity should be enabled when first added.
        /// </summary>
        public bool enabled_by_default { get; set; }

        /// <summary>
        /// (Optional, default: utf-8) The encoding of the payloads received.
        /// Set to "" to disable decoding of incoming payload.
        /// </summary>
        public string encoding { get; set; }

        /// <summary>
        /// (Optional) The category of the entity. When set, the entity category must be diagnostic for sensors.
        /// </summary>
        public string entity_category { get; set; }

        /// <summary>
        /// (Optional) For sensors that only send on state updates (like PIRs), this variable sets a delay
        /// in seconds after which the sensor’s state will be updated back to off.
        /// </summary>
        public int off_delay { get; set; }

        /// <summary>
        /// (Optional, default: online) The string that represents the online state.
        /// </summary>
        public string payload_available { get; set; }

        /// <summary>
        /// (Optional, default: offline) The string that represents the offline state.
        /// </summary>
        public string payload_not_available { get; set; }

        /// <summary>
        /// (Optional, default: OFF) The string that represents the off state.
        /// It will be compared to the message in the state_topic (see value_template for details)
        /// </summary>
        public string payload_off { get; set; }

        /// <summary>
        /// (Optional, default: ON) The string that represents the on state.
        /// It will be compared to the message in the state_topic (see value_template for details)
        /// </summary>
        public string payload_on { get; set; }
    }
}
