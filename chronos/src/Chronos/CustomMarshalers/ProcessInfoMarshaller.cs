using System;
using System.IO;
using Chronos.Core;
using Rhiannon.Serialization.Marshaling;

namespace Chronos.CustomMarshalers
{
	public class ProcessInfoMarshaller : GenericMarshaler<ProcessInfo>
	{
		protected override void MarshalInternal(ProcessInfo value, Stream stream)
		{
			UInt32Marshaler.Marshal(value.EndTime, stream);
			StringMarshaler.Marshal(value.ExecutableFullName, stream);
			Int32Marshaler.Marshal(value.ProcessId, stream);
			StringMarshaler.Marshal(value.ProcessName, stream);
			DateTimeMarshaler.Marshal(value.StartTime, stream);
            UInt32Marshaler.Marshal(value.SyncTime, stream);
			ByteArrayMarshaler.Marshal(value.ProcessIcon, stream);
		}

		protected override ProcessInfo DemarshalInternal(Stream stream)
		{
			uint endTime = UInt32Marshaler.Demarshal(stream);
			string executableFullName = StringMarshaler.Demarshal(stream);
			int processId = Int32Marshaler.Demarshal(stream);
			string processName = StringMarshaler.Demarshal(stream);
			DateTime startTime = DateTimeMarshaler.Demarshal(stream);
		    uint syncTime = UInt32Marshaler.Demarshal(stream);
			byte[] icon = ByteArrayMarshaler.Demarshal(stream);
            ProcessInfo processInfo = new ProcessInfo(processId, processName, executableFullName, icon, startTime, syncTime);
			processInfo.EndTime = endTime;
			return processInfo;
		}
	}
}
