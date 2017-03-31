using System;
using System.IO;

namespace Rhiannon.Serialization.Marshaling
{
	public abstract class Marshaler
	{
		protected Marshaler(Type managedType)
		{
			ManagedType = managedType;
		}

		public Type ManagedType { get; private set; }

		public abstract void MarshalObject(object value, Stream stream);

		public abstract object DemarshalObject(Stream stream);
	}
}
