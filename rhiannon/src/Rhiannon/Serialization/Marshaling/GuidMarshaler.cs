using System;
using System.IO;

namespace Rhiannon.Serialization.Marshaling
{
	public class GuidMarshaler : GenericMarshaler<Guid>
	{
		protected override void MarshalInternal(Guid value, Stream stream)
		{
			Marshal(value, stream);
		}

		protected override Guid DemarshalInternal(Stream stream)
		{
			return Demarshal(stream);
		}

		public static void Marshal(Guid value, Stream stream)
		{
			StringMarshaler.Marshal(value.ToString(), stream);
		}

		public static Guid Demarshal(Stream stream)
		{
			return new Guid(StringMarshaler.Demarshal(stream));
		}
	}
}
