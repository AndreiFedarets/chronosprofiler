using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using Rhiannon.Extensions;

namespace Rhiannon.Windows.Presentation
{
	public class PropertyChangedNotifier : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		//protected void NotifyPropertyChanged()
		//{
		//    StackTrace stackTrace = new StackTrace(false);
		//    StackFrame propertyFrame = stackTrace.GetFrame(1);
		//    NotifyPropertyChanged(propertyFrame.GetMethod());
		//}

		protected void SetPropertyAndNotifyChanged<T>(Expression<Func<T>> property, ref T field, T newValue)
		{
			field = newValue;
			string name = property.GetPropertyName();
			NotifyPropertyChanged(name);
		}


		protected void NotifyPropertyChanged<T>(Expression<Func<T>> property)
		{
			string name = property.GetPropertyName();
			NotifyPropertyChanged(name);
		}

		protected void NotifyPropertyChanged(MethodBase property)
		{
			string name = property.GetPropertyName();
			NotifyPropertyChanged(name);
		}

		protected void NotifyPropertyChanged(string propertyName)
		{
			NotifyPropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

		protected void NotifyPropertyChanged(object sender, string info)
		{
			NotifyPropertyChanged(sender, new PropertyChangedEventArgs(info));
		}

		protected void NotifyPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(sender, e);
			}
		}

		public void ForceNotify()
		{
			foreach (PropertyInfo propertyInfo in GetType().GetProperties())
			{
				NotifyPropertyChanged(propertyInfo.Name);
			}
		}

	}
}
