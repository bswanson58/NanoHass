using System;
using System.Collections;
using NanoHass.Context;

namespace NanoHass.Discovery {
    /// <summary>
    /// Abstract discoverable object from which all Home Assistant entities are derived
    /// </summary>
    public abstract class AbstractDiscoverable {
        private BaseDiscoveryModel          mDeviceDiscoveryModel;

        protected IHassClientContext        ClientContext;

        public  string                      ObjectId { get; set; }
        public  string                      Name { get; }
        public  string                      Id { get; }
        public  string                      Domain { get; }

        public bool                         UseAttributes { get; }

        public int                          UpdateIntervalSeconds { get; }
        public DateTime                     LastUpdated { get; private set; }
        public string                       PreviousPublishedState { get; private set; }
        public string                       PreviousPublishedAttributes { get; private set; }

        protected AbstractDiscoverable( string name, string domain,
                                        int updateIntervalSeconds = 10, string id = default, bool useAttributes = false ) {
            Name = name;
            Domain = domain;
            UseAttributes = useAttributes;

            Id = string.IsNullOrEmpty( id ) ? Guid.NewGuid().ToString() : id;
            UpdateIntervalSeconds = updateIntervalSeconds;

            LastUpdated = DateTime.MinValue;
            PreviousPublishedState = string.Empty;
            PreviousPublishedAttributes = string.Empty;
        }

        public void InitializeParameters( IHassClientContext contextProvider ) {
            ClientContext = contextProvider;
        }

        public virtual string                   GetAttributes() => String.Empty;
        public virtual bool                     ProcessMessage( string topic, string payload ) => false;

        protected abstract BaseDiscoveryModel   CreateDiscoveryModel();
        public abstract string                  GetCombinedState();
        public abstract IList                   GetStatesToPublish();

        public BaseDiscoveryModel GetDiscoveryModel() =>
            mDeviceDiscoveryModel ??= CreateDiscoveryModel();

        public void ClearDiscoveryModel() =>
            mDeviceDiscoveryModel = null;

        public void UpdatePublishedState( string state, string attributes ) {
            PreviousPublishedState = state;
            PreviousPublishedAttributes = attributes;
            LastUpdated = DateTime.UtcNow;
        }

        public void ResetChecks() {
            LastUpdated = DateTime.MinValue;

            PreviousPublishedState = string.Empty;
            PreviousPublishedAttributes = string.Empty;
        }
    }
}
