// ReSharper disable InconsistentNaming

namespace NanoHass.Discovery {
    /// <summary>
    /// This information will be used when announcing this device on the mqtt topic
    /// </summary>
    public class DeviceConfigModel {
        /// <summary>
        /// (Optional) A list of connections of the device to the outside world as a list of
        /// tuples [connection_type, connection_identifier]. For example the MAC address of a
        /// network interface: "connections": [["mac", "02:5b:26:a8:dc:12"]].
        /// </summary>
        /// <value></value>
//        public ICollection<Tuple<string, string>>? connections { get; set; }

        /// <summary>
        /// (Optional) An Id to identify the device. For example a serial number.
        /// </summary>
        /// <value></value>
        public string[] identifiers { get; set; }

        /// <summary>
        /// (Optional) The manufacturer of the device.
        /// </summary>
        /// <value></value>
        public string manufacturer { get; set; }

        /// <summary>
        /// (Optional) The model of the device.
        /// </summary>
        /// <value></value>
        public string model { get; set; }

        /// <summary>
        /// (Optional) The name of the device.
        /// </summary>
        /// <value></value>
        public string name { get; set; }

        /// <summary>
        /// (Optional) The firmware version of the device.
        /// </summary>
        /// <value></value>
        public string sw_version { get; set; }

        /// <summary>
        /// (Optional) Identifier of a device that routes messages between this device and Home Assistant.
        /// Examples of such devices are hubs, or parent devices of a sub-device.
        /// This is used to show device topology in Home Assistant.
        /// </summary>
        /// <value></value>
//        public string via_device { get; set; }
    }
}
