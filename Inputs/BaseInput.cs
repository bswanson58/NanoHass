using NanoHass.Discovery;
using System;
using NanoHass.Hass;

namespace NanoHass.Inputs {
    public abstract class BaseInput : AbstractDiscoverable, IHassEntity {
        public event EventHandler   OnEntityStateChanged;

        protected BaseInput( string name, string domain, string entityIdentifier,
            int updateIntervalSeconds = 10, bool useAttributes = false ) :
            base( name, domain, entityIdentifier, updateIntervalSeconds, useAttributes ) { }

        public abstract object GetValue();
        public abstract void SetValue( object value );

        protected void TriggerStateChange() =>
            OnEntityStateChanged?.Invoke( this, EventArgs.Empty );
    }
}
