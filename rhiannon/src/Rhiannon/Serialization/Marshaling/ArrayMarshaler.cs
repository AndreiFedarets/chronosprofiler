using System;
using System.Diagnostics;
using System.IO;

namespace Rhiannon.Serialization.Marshaling
{
	public class ArrayMarshaler : Marshaler
	{
		private readonly Type _elementType;

		public ArrayMarshaler(Type type)
			: base(type)
		{
			_elementType = type.GetElementType();
		}

		public override void MarshalObject(object value, Stream stream)
		{
			Debug.Assert(value is Array, "ArrayMarshaler: value is not array");
			Array array = (Array) value;
			Int32Marshaler.Marshal(array.Length, stream);
			foreach (object element in array)
			{
				MarshalingManager.Marshal(element, stream);
			}
		}

		public override object DemarshalObject(Stream stream)
		{
			int count = Int32Marshaler.Demarshal(stream);
			Array array = Array.CreateInstance(_elementType, count);
			for (int i = 0; i < count; i++)
			{
				object element = MarshalingManager.Demarshal(_elementType, stream);
				array.SetValue(element, i);
			}
			return array;
		}
	}
}
