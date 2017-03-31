using System.IO;
using Chronos.Core;
using Rhiannon.Serialization.Marshaling;

namespace Chronos.CustomMarshalers
{
	public abstract class UnitBaseMarshaler<T> : GenericMarshaler<T> where T : UnitBase, new()
	{
		protected override void MarshalInternal(T value, Stream stream)
		{
			MarshalCommonPart(value, stream);
			MarshalSpecificPart(value, stream);
		}

		protected override T DemarshalInternal(Stream stream)
		{
			T value = DemarshalCommonPart(stream);
			DemarshalSpecificPart(value, stream);
			return value;
		}

		protected void MarshalCommonPart(T value, Stream stream)
		{
			UInt32Marshaler.Marshal(value.Id, stream);
			UInt64Marshaler.Marshal(value.ManagedId, stream);
			UInt32Marshaler.Marshal(value.BeginLifetime, stream);
			UInt32Marshaler.Marshal(value.EndLifetime, stream);
			StringMarshaler.Marshal(value.Name, stream);
			Int32Marshaler.Marshal(value.Revision, stream);
		}

		protected T DemarshalCommonPart(Stream stream)
		{
			T value = new T();
			value.Id = UInt32Marshaler.Demarshal(stream);
			value.ManagedId = UInt64Marshaler.Demarshal(stream);
			value.BeginLifetime = UInt32Marshaler.Demarshal(stream);
			value.EndLifetime = UInt32Marshaler.Demarshal(stream);
			value.Name = StringMarshaler.Demarshal(stream);
			value.Revision = Int32Marshaler.Demarshal(stream);
			return value;
		}

		protected abstract void MarshalSpecificPart(T value, Stream stream);

		protected abstract void DemarshalSpecificPart(T value, Stream stream);
	}
}
