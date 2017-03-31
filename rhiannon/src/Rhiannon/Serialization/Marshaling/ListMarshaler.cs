using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using Rhiannon.Extensions;

namespace Rhiannon.Serialization.Marshaling
{
	public class ListMarshaler : Marshaler
	{
		private readonly Type _elementType;

		public ListMarshaler(Type type)
			: base(type)
		{
			Debug.Assert(type.ImplementsInterface<IList>());
			_elementType = type.GetProperty("Item").PropertyType;
		}

		public override void MarshalObject(object value, Stream stream)
		{
			Debug.Assert(value.GetType().ImplementsInterface<IList>());
			IList list = (IList)value;
			Int32Marshaler.Marshal(list.Count, stream);
			foreach (object element in list)
			{
				MarshalingManager.Marshal(element, stream);
			}
		}

		public override object DemarshalObject(Stream stream)
		{
			int count = Int32Marshaler.Demarshal(stream);
			IList list = (IList)Activator.CreateInstance(ManagedType);
			for (int i = 0; i < count; i++)
			{
				object element = MarshalingManager.Demarshal(_elementType, stream);
				list.Add(element);
			}
			return list;
		}
	}
}
