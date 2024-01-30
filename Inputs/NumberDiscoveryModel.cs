using NanoHass.Discovery;

namespace NanoHass.Inputs {
    public class NumberDiscoveryModel : BaseDiscoveryModel {
        /// <summary>
        /// (Optional) The MQTT topic subscribed to receive availability (online/offline) updates.
        /// </summary>
        /// <value></value>
        public string availability_topic { get; set; }

        /// <summary>
        /// When availability is configured, this controls the conditions needed to set the entity to available.
        /// Valid entries are all, any, and latest. If set to all, payload_available must be received on all
        /// configured availability topics before the entity is marked as online.
        /// If set to any, payload_available must be received on at least one configured availability topic
        /// before the entity is marked as online. If set to latest, the last payload_available or
        /// payload_not_available received on any configured availability topic controls the availability.
        /// </summary>
        public string availability_mode { get; set; }

        /// <summary>
        /// (Optional) Defines a template to generate the payload to send to command_topic.
        /// </summary>
        public string command_template { get; set; }
        
        /// <summary>
        /// REQUIRED - The MQTT topic to publish commands to change the number.
        /// </summary>
        public string command_topic { get; set; }

        /// <summary>
        /// (Optional) The type/class of the number. The device_class can be null.
        /// 
        /// </summary>
        public string device_class { get; set; }

        /// <summary>
        /// Flag which defines if the entity should be enabled when first added. Default: true
        /// </summary>
        public bool enabled_by_default { get; set; }

        /// <summary>
        /// The encoding of the payloads received and published messages. Set to "" to disable decoding of incoming payload.
        /// Default: utf8
        /// </summary>
        public string encoding { get; set; }

        /// <summary>
        /// The category of the entity. Default: None
        /// </summary>
        public string entity_category { get; set; }
        /// <summary>
        /// Icon for the entity.
        /// </summary>
        public string icon { get; set; }

        /// <summary>
        /// Defines a template to extract the JSON dictionary from messages received on the json_attributes_topic.
        /// Usage example can be found in MQTT sensor documentation.
        /// </summary>
        public string json_attributes_template { get; set; }

        /// <summary>
        /// The MQTT topic subscribed to receive a JSON dictionary payload and then set as sensor attributes.
        /// Usage example can be found in MQTT sensor documentation.
        /// </summary>
        public string json_attributes_topic { get; set; }

        /// <summary>
        /// (Optional, default: 1) Minimum value.
        /// 
        /// </summary>
        public float min {get; set; }

        /// <summary>
        /// (Optional, default: 100) Maximum value.
        /// </summary>
        public float max {get; set; }

        /// <summary>
        /// (Optional, default: “auto”)
        /// Control how the number should be displayed in the UI. Can be set to box or slider to force a display mode.
        /// </summary>
        public string mode {get; set; }

        /// <summary>
        /// Used instead of name for automatic generation of entity_id
        /// </summary>
        public string object_id { get; set; }

        /// <summary>
        /// Flag that defines if switch works in optimistic mode.
        /// Default: true if no state topic defined, else false.
        /// </summary>
        public bool optimistic { get; set; }

        /// <summary>
        /// (Optional, default: “None”) A special payload that resets the state to unknown when received on the state_topic.
        /// 
        /// </summary>
        public string payload_reset {get; set; }

        /// <summary>
        /// The maximum QoS level to be used when receiving and publishing messages. Default: 0
        /// </summary>
        public int qos { get; set; }

        /// <summary>
        /// If the published message should have the retain flag on or not. Default: false
        /// </summary>
        public bool retain { get; set; }

        /// <summary>
        /// (Optional, default: 1) Step value. Smallest value 0.001.
        /// 
        /// </summary>
        public float step {get; set; }

        /// <summary>
        /// An ID that uniquely identifies this light. If two lights have the same unique ID,
        /// Home Assistant will raise an exception.
        /// </summary>
        public string unique_id { get; set; }

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
