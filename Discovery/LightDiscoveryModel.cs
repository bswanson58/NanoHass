// ReSharper disable InconsistentNaming
namespace NanoHass.Discovery {
    public class LightDiscoveryModel : BaseDiscoveryModel {
        /// <summary>
        /// The MQTT topic subscribed to receive availability (online/offline) updates.
        /// Must not be used together with availability.
        /// </summary>
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
        /// Defines a template to extract device’s availability from the availability_topic.
        /// To determine the devices’s availability result of this template will be compared to payload_available
        /// and payload_not_available.
        /// </summary>
        public string availability_template  { get; set; }

        /// <summary>
        /// The MQTT topic to publish commands to change the light’s brightness.
        /// </summary>
        public string brightness_command_topic { get; set; }

        /// <summary>
        /// Defines a template to compose message which will be sent to brightness_command_topic. Available variables: value.
        /// </summary>
        public string brightness_command_template { get;set; }

        /// <summary>
        /// Defines the maximum brightness value (i.e., 100%) of the MQTT device. Defaults to 1255.
        /// </summary>
        public int brightness_scale { get; set; }

        /// <summary>
        /// The MQTT topic subscribed to receive brightness state updates.
        /// </summary>
        public string brightness_state_topic { get; set; }

        /// <summary>
        /// Defines a template to extract the brightness value.
        /// </summary>
        public string brightness_value_template { get; set; }

        /// <summary>
        /// The MQTT topic subscribed to receive color mode updates. If this is not configured,
        /// color_mode will be automatically set according to the last received valid color or color temperature
        /// </summary>
        public string color_mode_state_topic { get; set; }

        /// <summary>
        /// Defines a template to extract the color mode.
        /// </summary>
        public string color_mode_value_template { get; set;}

        /// <summary>
        /// Defines a template to compose message which will be sent to color_temp_command_topic. Available variables: value.
        /// </summary>
        public string color_temp_command_template { get; set;}

        /// <summary>
        /// The MQTT topic to publish commands to change the light’s color temperature state.
        /// The color temperature command slider has a range of 153 to 500 mireds (micro reciprocal degrees).
        /// </summary>
        public string color_temp_command_topic { get; set; }

        /// <summary>
        /// The MQTT topic subscribed to receive color temperature state updates.
        /// </summary>
        public string color_temp_state_topic { get; set; }

        /// <summary>
        /// Defines a template to extract the color temperature value.
        /// </summary>
        public string color_temp_value_template { get; set; }

        /// <summary>
        /// The MQTT topic to publish commands to change the switch state. Required!
        /// </summary>
        public string command_topic { get; set; }

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
        /// The MQTT topic to publish commands to change the light’s effect state.
        /// </summary>
        public string effect_command_topic { get; set; }

        /// <summary>
        /// Defines a template to compose message which will be sent to effect_command_topic. Available variables: value.
        /// </summary>
        public string effect_command_template { get; set; }

        /// <summary>
        /// The list of effects the light supports.
        /// </summary>
        public string effect_list { get; set; }

        /// <summary>
        /// The MQTT topic subscribed to receive effect state updates.
        /// </summary>
        public string effect_state_topic { get; set; }

        /// <summary>
        ///         Defines a template to extract the effect value.
        /// </summary>
        public string effect_value_template { get; set; }

        /// <summary>
        /// Defines a template to compose message which will be sent to hs_command_topic. Available variables: hue and sat.
        /// </summary>
        public string hs_command_template {get; set; }

        /// <summary>
        /// The MQTT topic to publish commands to change the light’s color state in HS format (Hue Saturation).
        /// Range for Hue: 0° .. 360°, Range of Saturation: 0..100. Note:
        /// Brightness is sent separately in the brightness_command_topic.
        /// </summary>
        public string hs_command_topic { get; set; }

        /// <summary>
        /// The MQTT topic subscribed to receive color state updates in HS format.
        /// The expected payload is the hue and saturation values separated by commas, for example, 359.5,100.0.
        /// Note: Brightness is received separately in the brightness_state_topic.
        /// </summary>
        public string hs_state_topic { get; set; }

        /// <summary>
        /// Defines a template to extract the HS value.
        /// </summary>
        public string hs_value_template { get; set; }

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
        /// The maximum color temperature in mireds.
        /// </summary>
        public int max_mireds { get; set; }

        /// <summary>
        /// The minimum color temperature in mireds.
        /// </summary>
        public int min_mireds { get; set; }

        /// <summary>
        /// Used instead of name for automatic generation of entity_id
        /// </summary>
        public string object_id { get; set; }

        /// <summary>
        /// Defines when on the payload_on is sent. Using last (the default) will send any
        /// style (brightness, color, etc) topics first and then a payload_on to the command_topic.
        /// Using first will send the payload_on and then any style topics. Using brightness will only
        /// send brightness commands instead of the payload_on to turn the light on.
        /// </summary>
        public string on_command_type { get; set; }

        /// <summary>
        /// Flag that defines if switch works in optimistic mode.
        /// Default: true if no state topic defined, else false.
        /// </summary>
        public bool optimistic { get; set; }

        /// <summary>
        /// The payload that represents the available state. Default: online
        /// </summary>
        public string payload_available { get; set; }

        /// <summary>
        /// The payload that represents the unavailable state.
        /// </summary>
        public string payload_not_available { get; set; }

        /// <summary>
        /// The payload that represents disabled state. Default: OFF
        /// </summary>
        public string payload_off { get; set; }

        /// <summary>
        /// The payload that represents enabled state. Default: ON
        /// </summary>
        public string payload_on { get; set; }

        /// <summary>
        /// The maximum QoS level to be used when receiving and publishing messages. Default: 0
        /// </summary>
        public int qos { get; set; }

        /// <summary>
        /// If the published message should have the retain flag on or not. Default: false
        /// </summary>
        public bool retain { get; set; }

        /// <summary>
        /// Defines a template to compose message which will be sent to rgb_command_topic.
        /// Available variables: red, green and blue.
        /// </summary>
        public string rgb_command_template { get; set; }

        /// <summary>
        /// The MQTT topic to publish commands to change the light’s RGB state.
        /// </summary>
        public string rgb_command_topic { get; set; }

        /// <summary>
        /// The MQTT topic subscribed to receive RGB state updates.
        /// The expected payload is the RGB values separated by commas, for example, 255,0,127.
        /// </summary>
        public string rgb_state_topic { get; set; }

        /// <summary>
        /// Defines a template to extract the RGB value.
        /// </summary>
        public string rgb_value_template { get; set; }

        /// <summary>
        /// Defines a template to compose message which will be sent to rgbw_command_topic.
        /// Available variables: red, green, blue and white.
        /// </summary>
        public string rgbw_command_template { get; set; }

        /// <summary>
        /// The MQTT topic to publish commands to change the light’s RGBW state.
        /// </summary>
        public string rgbw_command_topic { get; set; }

        /// <summary>
        /// The MQTT topic subscribed to receive RGBW state updates.
        /// The expected payload is the RGBW values separated by commas, for example, 255,0,127,64.
        /// </summary>
        public string rgbw_state_topic { get; set; }

        /// <summary>
        /// Defines a template to extract the RGBW value.
        /// </summary>
        public string rgbw_value_template { get; set; }

        /// <summary>
        /// Defines a template to compose message which will be sent to rgbww_command_topic.
        /// Available variables: red, green, blue, cold_white and warm_white.
        /// </summary>
        public string rgbww_command_template { get; set; }

        /// <summary>
        /// The MQTT topic to publish commands to change the light’s RGBWW state.
        /// </summary>
        public string rgbww_command_topic { get; set; }

        /// <summary>
        /// The MQTT topic subscribed to receive RGBWW state updates.
        /// The expected payload is the RGBWW values separated by commas, for example, 255,0,127,64,32.
        /// </summary>
        public string rgbww_state_topic { get; set; }

        /// <summary>
        /// Defines a template to extract the RGBWW value.
        /// </summary>
        public string rgbww_value_template { get; set; }

        /// <summary>
        /// The schema to use. Must be default or omitted to select the default schema. Default: default
        /// </summary>
        public string schema { get; set; }

        /// <summary>
        /// Defines a template to extract the state value.
        /// The template should match the payload on and off values, so if your light uses power on to turn on,
        /// your state_value_template string should return power on when the switch is on.
        /// For example if the message is just on, your state_value_template should be power .
        /// </summary>
        public string state_value_template { get; set; }

        /// <summary>
        /// An ID that uniquely identifies this light. If two lights have the same unique ID,
        /// Home Assistant will raise an exception.
        /// </summary>
        public string unique_id { get; set; }

        /// <summary>
        /// The MQTT topic to publish commands to change the light to white mode with a given brightness.
        /// </summary>
        public string white_command_topic { get; set; }

        /// <summary>
        /// Defines the maximum white level (i.e., 100%) of the MQTT device. Default: 255
        /// </summary>
        public int white_scale { get; set; }

        /// <summary>
        /// Defines a template to compose message which will be sent to xy_command_topic.
        /// Available variables: x and y.
        /// </summary>
        public string xy_command_template { get; set; }

        /// <summary>
        /// The MQTT topic to publish commands to change the light’s XY state.
        /// </summary>
        public string xy_command_topic { get; set; }

        /// <summary>
        /// The MQTT topic subscribed to receive XY state updates.
        /// The expected payload is the X and Y color values separated by commas, for example, 0.675,0.322.
        /// </summary>
        public string xy_state_topic { get; set; }

        /// <summary>
        /// Defines a template to extract the XY value.    }
        /// </summary>
        public string xy_value_template { get; set; }
    }
}
