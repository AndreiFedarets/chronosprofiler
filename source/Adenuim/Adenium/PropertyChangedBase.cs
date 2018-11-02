using System;

namespace Adenium
{
    public abstract class PropertyChangedBaseEx : Caliburn.Micro.PropertyChangedBase, IDisposable
    {
        public override void NotifyOfPropertyChange(string propertyName)
        {
            SmartDispatcher.Main.Invoke(() => base.NotifyOfPropertyChange(propertyName));
        }

        public virtual void Dispose()
        {
            
        }
    }
}
