using System;

namespace Chronos.Core
{
	[Serializable]
	public class AgentSettings
	{
        public AgentSettings(Guid sessionToken, uint callPageSize, uint threadStreamsCount)
		{
			SessionToken = sessionToken;
			CallPageSize = callPageSize;
            ThreadStreamsCount = threadStreamsCount;
		}

		public Guid SessionToken { get; private set; }

		public uint CallPageSize { get; private set; }

        public uint ThreadStreamsCount { get; private set; }
	}
}
