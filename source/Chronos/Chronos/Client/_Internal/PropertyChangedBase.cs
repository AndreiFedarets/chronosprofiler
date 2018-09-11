using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Chronos.Client
{
    internal class PropertyChangedBase : RemoteBaseObject, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyOfPropertyChange<TProperty>(Expression<Func<TProperty>> property)
        {
            NotifyOfPropertyChange(property.GetMemberInfo().Name);
        }

        public void NotifyOfPropertyChange(string propertyName)
        {
            Action action = () => NotifyOfPropertyChange(new PropertyChangedEventArgs(propertyName));
            DispatcherHolder.Invoke(action);
        }

        private void NotifyOfPropertyChange(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}
