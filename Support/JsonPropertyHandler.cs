using System;
using System.Collections;
using System.Text;
using nanoFramework.Json;
using NanoHass.Discovery;
using NanoHass.Inputs;

namespace NanoHass.Support {
    /// <summary>
    /// Eliminates properties without values, either null or empty when translated to a string.
    /// </summary>
    internal class JsonPropertyHandler {
        private readonly Hashtable  mProperties;

        public JsonPropertyHandler() {
            mProperties = new Hashtable();
        }

        public JsonPropertyHandler AddProperty( string propertyName, object propertyValue ) {
            if( propertyValue != null ) {
                var strValue = propertyValue.ToString();

                if(!String.IsNullOrEmpty( strValue )) {
                    if( mProperties.Contains( propertyName )) {
                        mProperties[propertyName] = propertyValue;
                    }
                    else {
                        mProperties.Add( propertyName, propertyValue );
                    }
                }
            }

            return this;
        }

        public JsonPropertyHandler AddProperty( string propertyName, object propertyValue, object ifNotValue ) {
            if(!Object.Equals( propertyValue, ifNotValue )) {
                AddProperty( propertyName, propertyValue );
            }

            return this;
        }

        public string AsJson() =>
            JsonSerializer.SerializeObject( mProperties );
    }

    internal static class DeviceConfigModelEx {
        public static string AsJson( this DeviceConfigModel model ) {
            var retValue = new JsonPropertyHandler();

            retValue
                .AddProperty( nameof( model.name ), model.name )
                .AddProperty( nameof( model.configuration_url ), model.configuration_url )
                .AddProperty( nameof( model.hw_version ), model.hw_version )
                .AddProperty( nameof( model.identifiers ), model.identifiers )
                .AddProperty( nameof( model.manufacturer ), model.manufacturer )
                .AddProperty( nameof( model.model ), model.model )
                .AddProperty( nameof( model.suggested_area ), model.suggested_area )
                .AddProperty( nameof( model.sw_version ), model.sw_version )
                .AddProperty( nameof( model.via_device ), model.via_device );

            return retValue.AsJson();
        }
    }

    internal static class SensorDiscoveryModelEx {
        public static string AsJson( this SensorDiscoveryModel model ) {
            var retValue = new JsonPropertyHandler();

            retValue
                .AddProperty( nameof( model.availability_topic ), model.availability_topic )
                .AddProperty( nameof( model.device_class ), model.device_class )
                .AddProperty( nameof( model.enabled_by_default ), model.enabled_by_default, true )
                .AddProperty( nameof( model.icon ), model.icon )
                .AddProperty( nameof( model.json_attributes_template ), model.json_attributes_topic )
                .AddProperty( nameof( model.json_attributes_topic ), model.json_attributes_topic )
                .AddProperty( nameof( model.expire_after ), model.expire_after, 0 )
                .AddProperty( nameof( model.force_update ), model.force_update, false )
                .AddProperty( nameof( model.object_id ), model.object_id )
                .AddProperty( nameof( model.name ), model.name )
                .AddProperty( nameof( model.qos ), model.qos, 0 )
                .AddProperty( nameof( model.state_topic ), model.state_topic )
                .AddProperty( nameof( model.unique_id ), model.unique_id )
                .AddProperty( nameof( model.unit_of_measurement ), model.unit_of_measurement )
                .AddProperty( nameof( model.value_template ), model.value_template )
                .AddProperty( nameof( model.device ), "_____" );

            return new StringBuilder( retValue.AsJson())
                .Replace( "\"_____\"", model.device.AsJson())
                .ToString();
        }
    }

    internal static class BinarySensorDiscoveryModelEx {
        public static string AsJson( this BinarySensorDiscoveryModel model ) {
            var retValue = new JsonPropertyHandler();

            retValue
                .AddProperty( nameof( model.availability_mode ), model.availability_mode )
                .AddProperty( nameof( model.availability_template ), model.availability_template )
                .AddProperty( nameof( model.availability_topic ), model.availability_topic )
                .AddProperty( nameof( model.device_class ), model.device_class )
                .AddProperty( nameof( model.icon ), model.icon )
                .AddProperty( nameof( model.json_attributes_template ), model.json_attributes_topic )
                .AddProperty( nameof( model.json_attributes_topic ), model.json_attributes_topic )
                .AddProperty( nameof( model.enabled_by_default ), model.enabled_by_default, true )
                .AddProperty( nameof( model.encoding ), model.encoding )
                .AddProperty( nameof( model.entity_category ), model.entity_category )
                .AddProperty( nameof( model.expire_after ), model.expire_after, 0 )
                .AddProperty( nameof( model.force_update ), model.force_update, false )
                .AddProperty( nameof( model.object_id ), model.object_id )
                .AddProperty( nameof( model.off_delay ), model.off_delay, 0 )
                .AddProperty( nameof( model.name ), model.name )
                .AddProperty( nameof( model.payload_available ), model.payload_available )
                .AddProperty( nameof( model.payload_not_available ), model.payload_not_available )
                .AddProperty( nameof( model.payload_off ), model.payload_off )
                .AddProperty( nameof( model.payload_on ), model.payload_on )
                .AddProperty( nameof( model.qos ), model.qos, 0 )
                .AddProperty( nameof( model.state_topic ), model.state_topic )
                .AddProperty( nameof( model.unique_id ), model.unique_id )
                .AddProperty( nameof( model.value_template ), model.value_template )
                .AddProperty( nameof( model.device ), "_____" );

            return new StringBuilder( retValue.AsJson())
                .Replace( "\"_____\"", model.device.AsJson())
                .ToString();
        }
    }

    internal static class LightDiscoveryModelEx {
        public static string AsJson( this LightDiscoveryModel model ) {
            var retValue = new JsonPropertyHandler();

            retValue
                .AddProperty( nameof( model.availability_mode ), model.availability_mode )
                .AddProperty( nameof( model.availability_template ), model.availability_template )
                .AddProperty( nameof( model.availability_topic ), model.availability_topic )
                .AddProperty( nameof( model.brightness_command_template ), model.brightness_command_template )
                .AddProperty( nameof( model.brightness_command_topic ), model.brightness_command_topic )
                .AddProperty( nameof( model.brightness_scale ), model.brightness_scale, 0 )
                .AddProperty( nameof( model.brightness_state_topic ), model.brightness_state_topic )
                .AddProperty( nameof( model.brightness_value_template ), model.brightness_value_template )
                .AddProperty( nameof( model.color_mode_state_topic ), model.color_mode_state_topic )
                .AddProperty( nameof( model.color_mode_value_template ), model.color_mode_value_template )
                .AddProperty( nameof( model.color_temp_command_template ), model.color_temp_command_template )
                .AddProperty( nameof( model.color_temp_command_topic ), model.color_temp_command_topic )
                .AddProperty( nameof( model.color_temp_state_topic ), model.color_temp_state_topic )
                .AddProperty( nameof( model.color_temp_value_template ), model.color_temp_value_template )
                .AddProperty( nameof( model.command_topic ), model.command_topic )
                .AddProperty( nameof( model.enabled_by_default ), model.enabled_by_default, true )
                .AddProperty( nameof( model.encoding ), model.encoding )
                .AddProperty( nameof( model.entity_category ), model.entity_category )
                .AddProperty( nameof( model.effect_command_template ), model.effect_command_template )
                .AddProperty( nameof( model.effect_command_topic ), model.effect_command_topic )
                .AddProperty( nameof( model.effect_list ), model.effect_list )
                .AddProperty( nameof( model.effect_state_topic ), model.effect_state_topic )
                .AddProperty( nameof( model.effect_value_template ), model.effect_value_template )
                .AddProperty( nameof( model.hs_command_template ), model.hs_command_template )
                .AddProperty( nameof( model.hs_command_topic ), model.hs_command_topic )
                .AddProperty( nameof( model.hs_state_topic ), model.hs_state_topic )
                .AddProperty( nameof( model.hs_value_template ), model.hs_value_template )
                .AddProperty( nameof( model.icon ), model.icon )
                .AddProperty( nameof( model.json_attributes_template ), model.json_attributes_topic )
                .AddProperty( nameof( model.json_attributes_topic ), model.json_attributes_topic )
                .AddProperty( nameof( model.max_mireds ), model.max_mireds, 0 )
                .AddProperty( nameof( model.min_mireds ), model.min_mireds, 0 )
                .AddProperty( nameof( model.object_id ), model.object_id )
                .AddProperty( nameof( model.on_command_type ), model.on_command_type )
                .AddProperty( nameof( model.optimistic ), model.optimistic, true )
                .AddProperty( nameof( model.name ), model.name )
                .AddProperty( nameof( model.payload_available ), model.payload_available )
                .AddProperty( nameof( model.payload_not_available ), model.payload_not_available )
                .AddProperty( nameof( model.payload_off ), model.payload_off )
                .AddProperty( nameof( model.payload_on ), model.payload_on )
                .AddProperty( nameof( model.qos ), model.qos, 0 )
                .AddProperty( nameof( model.retain ), model.retain, false )
                .AddProperty( nameof( model.rgb_command_template ), model.rgb_command_template )
                .AddProperty( nameof( model.rgb_command_topic ), model.rgb_command_topic )
                .AddProperty( nameof( model.rgb_state_topic ), model.rgb_state_topic )
                .AddProperty( nameof( model.rgb_value_template ), model.rgb_value_template )
                .AddProperty( nameof( model.rgbw_command_template ), model.rgbw_command_template )
                .AddProperty( nameof( model.rgbw_command_topic ), model.rgbw_command_topic )
                .AddProperty( nameof( model.rgbw_state_topic ), model.rgbw_state_topic )
                .AddProperty( nameof( model.rgbw_value_template ), model.rgbw_value_template )
                .AddProperty( nameof( model.rgbww_command_template ), model.rgbww_command_template )
                .AddProperty( nameof( model.rgbww_command_topic ), model.rgbww_command_topic )
                .AddProperty( nameof( model.rgbww_state_topic ), model.rgbww_state_topic )
                .AddProperty( nameof( model.rgbww_value_template ), model.rgbww_value_template )
                .AddProperty( nameof( model.schema ), model.schema )
                .AddProperty( nameof( model.state_value_template ), model.state_value_template )
                .AddProperty( nameof( model.state_topic ), model.state_topic )
                .AddProperty( nameof( model.unique_id ), model.unique_id )
                .AddProperty( nameof( model.white_command_topic ), model.white_command_topic )
                .AddProperty( nameof( model.white_scale ), model.white_scale, 0 )
                .AddProperty( nameof( model.xy_command_template ), model.xy_command_template )
                .AddProperty( nameof( model.xy_command_topic ), model.xy_command_topic )
                .AddProperty( nameof( model.xy_state_topic ), model.xy_state_topic )
                .AddProperty( nameof( model.xy_value_template ), model.xy_value_template )
                .AddProperty( nameof( model.device ), "_____" );

            return new StringBuilder( retValue.AsJson())
                .Replace( "\"_____\"", model.device.AsJson())
                .ToString();
        }
    }

    internal static class NumberDiscoveryModelEx {
        public static string AsJson( this NumberDiscoveryModel model ) {
            var retValue = new JsonPropertyHandler();

            retValue
                .AddProperty( nameof( model.availability_mode ), model.availability_mode )
                .AddProperty( nameof( model.availability_topic ), model.availability_topic )
                .AddProperty( nameof( model.command_template ), model.command_template )
                .AddProperty( nameof( model.command_topic ), model.command_topic )
                .AddProperty( nameof( model.device_class ), model.device_class )
                .AddProperty( nameof( model.enabled_by_default ), model.enabled_by_default, true )
                .AddProperty( nameof( model.encoding ), model.encoding )
                .AddProperty( nameof( model.entity_category ), model.entity_category )
                .AddProperty( nameof( model.icon ), model.icon )
                .AddProperty( nameof( model.json_attributes_template ), model.json_attributes_topic )
                .AddProperty( nameof( model.json_attributes_topic ), model.json_attributes_topic )
                .AddProperty( nameof( model.mode ), model.mode, "auto" )
                .AddProperty( nameof( model.max ), model.max, 100 )
                .AddProperty( nameof( model.min ), model.min, 1 )
                .AddProperty( nameof( model.name ), model.name )
                .AddProperty( nameof( model.object_id ), model.object_id )
                .AddProperty( nameof( model.optimistic ), model.optimistic, true )
                .AddProperty( nameof( model.payload_reset ), model.payload_reset, "None" )
                .AddProperty( nameof( model.qos ), model.qos, 0 )
                .AddProperty( nameof( model.retain ), model.retain, false )
                .AddProperty( nameof( model.state_topic ), model.state_topic )
                .AddProperty( nameof( model.step ), model.step, 1 )
                .AddProperty( nameof( model.unique_id ), model.unique_id )
                .AddProperty( nameof( model.unit_of_measurement ), model.unit_of_measurement )
                .AddProperty( nameof( model.value_template ), model.value_template )
                .AddProperty( nameof( model.device ), "_____" );

            return new StringBuilder( retValue.AsJson())
                .Replace( "\"_____\"", model.device.AsJson())
                .ToString();
        }
    }
}
