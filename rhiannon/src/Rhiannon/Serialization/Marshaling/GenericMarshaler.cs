using System.Diagnostics;
using System.IO;

namespace Rhiannon.Serialization.Marshaling
{
	public abstract class GenericMarshaler<T> : Marshaler
	{
		protected GenericMarshaler()
			: base(typeof(T))
		{
		}

		public sealed override void MarshalObject(object value, Stream stream)
		{
			Debug.Assert(value is T, string.Format("{0}: value is not {1}", GetType(), ManagedType));
			MarshalInternal((T)value, stream);
		}

		public sealed override object DemarshalObject(Stream stream)
		{
			return DemarshalInternal(stream);
		}

		protected abstract void MarshalInternal(T value, Stream stream);

		protected abstract T DemarshalInternal(Stream stream);
	}
}
