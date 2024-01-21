namespace NanoHass.Models {
    public class DeviceTopicState {
        public  string      Topic { get; }
        public  string      State { get; }

        public DeviceTopicState( string topic, string state ) {
            Topic = topic;
            State = state;
        }
    }
}
