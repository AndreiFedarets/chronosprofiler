using System;
using System.IO;
using Chronos.Core;
using Rhiannon.Serialization.Marshaling;

namespace Chronos.CustomMarshalers
{
	public class AgentSettingsMarshaller : GenericMarshaler<AgentSettings>
	{
		protected override void MarshalInternal(AgentSettings value, Stream stream)
		{
			GuidMarshaler.Marshal(value.SessionToken, stream);
            UInt32Marshaler.Marshal(value.CallPageSize, stream);
            UInt32Marshaler.Marshal(value.ThreadStreamsCount, stream);
		}

		protected override AgentSettings DemarshalInternal(Stream stream)
		{
			Guid sessionToken = GuidMarshaler.Demarshal(stream);
            uint callPageSize = UInt32Marshaler.Demarshal(stream);
            uint threadStreamsCount = UInt32Marshaler.Demarshal(stream);
            return new AgentSettings(sessionToken, callPageSize, threadStreamsCount);
		}
	}
}
