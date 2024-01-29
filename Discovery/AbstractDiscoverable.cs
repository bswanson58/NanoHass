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

        public  string                      Name { get; }
        public  string                      EntityIdentifier { get; }
        public  string                      Domain { get; }

        public bool                         UseAttributes { get; }

        public int                          UpdateIntervalSeconds { get; }
        public DateTime                     LastUpdated { get; private set; }
        public string                       PreviousPublishedState { get; private set; }
        public string                       PreviousPublishedAttributes { get; private set; }

        protected AbstractDiscoverable( string name, string domain, string entityIdentifier,
                                        int updateIntervalSeconds = 10, bool useAttributes = false ) {
            Name = name;
            Domain = domain;
            EntityIdentifier = string.IsNullOrEmpty( entityIdentifier ) ? Guid.NewGuid().ToString() : entityIdentifier;

            UseAttributes = useAttributes;
            UpdateIntervalSeconds = updateIntervalSeconds;

            LastUpdated = DateTime.MinValue;
            PreviousPublishedState = string.Empty;
            PreviousPublishedAttributes = string.Empty;
        }

        public virtual string                   GetAttributes() => String.Empty;
        public virtual bool                     ProcessMessage( string topic, string payload ) => false;
        public virtual string                   GetSubscriptionTopic() => String.Empty;

        protected abstract BaseDiscoveryModel   CreateDiscoveryModel();
        public abstract string                  GetDiscoveryPayload();
        public abstract string                  GetStatePayload();
        public abstract IList                   GetStatesToPublish();

        public void InitializeParameters( IHassClientContext contextProvider ) {
            ClientContext = contextProvider;
        }

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
