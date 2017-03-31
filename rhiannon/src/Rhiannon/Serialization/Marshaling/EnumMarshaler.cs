using System;
using System.Diagnostics;
using System.IO;

namespace Rhiannon.Serialization.Marshaling
{
	public class EnumMarshaler : Marshaler
	{
		private readonly Marshaler _underlyingMarshaler;

		public EnumMarshaler(Type type)
			: base(type)
		{
			_underlyingMarshaler = MarshalingManager.GetMarshaler(Enum.GetUnderlyingType(type));
		}

		public override void MarshalObject(object value, Stream stream)
		{
			Debug.Assert(value.GetType() == ManagedType);
			object underlyingValue = Convert.ChangeType(value, _underlyingMarshaler.ManagedType);
			_underlyingMarshaler.MarshalObject(underlyingValue, stream);
		}

		public override object DemarshalObject(Stream stream)
		{
			object value = _underlyingMarshaler.DemarshalObject(stream);
			return value;
		}
	}
}