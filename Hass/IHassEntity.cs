using System;

namespace NanoHass.Hass {
    public interface IHassEntity {
        event EventHandler  OnEntityStateChanged;
    }
}
