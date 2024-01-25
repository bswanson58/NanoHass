using System;
using System.Collections;
using System.Text;
using nanoFramework.Json;
using NanoHass.Discovery;

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
}
