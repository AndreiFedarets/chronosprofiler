using System;

namespace Adenium
{
    public abstract class PropertyChangedBaseEx : Caliburn.Micro.PropertyChangedBase, IDisposable
    {
        protected PropertyChangedBaseEx()
        {
            Dispatcher = SmartDispatcher.Current;
        }

        protected IDispatcher Dispatcher { get; private set; }

        public override void NotifyOfPropertyChange(string propertyName)
        {
            Dispatcher.Invoke(() => base.NotifyOfPropertyChange(propertyName));
        }

        public virtual void Dispose()
        {
            
        }
    }
}
