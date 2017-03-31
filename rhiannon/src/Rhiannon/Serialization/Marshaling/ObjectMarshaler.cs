using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Diagnostics;

namespace Rhiannon.Serialization.Marshaling
{
	public class ObjectMarshaler : Marshaler
	{
		private readonly PropertyInfo[] _properties;

		public ObjectMarshaler(Type type)
			: base(type)
		{
			_properties = GetMarshalingProperties(type);
		}

		public override void MarshalObject(object value, Stream stream)
		{
			Debug.Assert(value != null, "ObjectMarshaler: value cannot be null");
			foreach (PropertyInfo propertyInfo in _properties)
			{
				object propertyValue = propertyInfo.GetValue(value, null);
				MarshalingManager.Marshal(propertyValue, stream);
			}
		}

		public override object DemarshalObject(Stream stream)
		{
			object obj = Activator.CreateInstance(ManagedType);
			foreach (PropertyInfo propertyInfo in _properties)
			{
				object propertyValue = MarshalingManager.Demarshal(propertyInfo.PropertyType, stream);
				propertyInfo.SetValue(obj, propertyValue, null);
			}
			return obj;
		}

		private static PropertyInfo[] GetMarshalingProperties(Type type)
		{
			IList<PropertyInfo> properties = new List<PropertyInfo>();
			foreach (PropertyInfo propertyInfo in type.GetProperties())
			{
				object[] ignored = propertyInfo.GetCustomAttributes(typeof (MarshalingIgnoreAttribute), true);
				if (ignored.Length == 0)
				{
					properties.Add(propertyInfo);
				}
			}
			return properties.ToArray();
		}
	}
}
