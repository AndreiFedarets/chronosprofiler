using System.IO;
using System;

namespace Rhiannon.Serialization.Marshaling
{
	public class DateTimeMarshaler : GenericMarshaler<DateTime>
	{
		protected override void MarshalInternal(DateTime value, Stream stream)
		{
			Marshal(value, stream);
		}

		protected override DateTime DemarshalInternal(Stream stream)
		{
			return Demarshal(stream);
		}

		public static void Marshal(DateTime value, Stream stream)
		{
			long time = value.ToFileTime();
			Int64Marshaler.Marshal(time, stream);
		}

		public static DateTime Demarshal(Stream stream)
		{
			long time = Int64Marshaler.Demarshal(stream);
			return DateTime.FromFileTime(time);
		}
	}
}
