#include "stdafx.h"
#include "Chronos.Agent.Internal.h"

namespace Chronos
{
	namespace Agent
	{
		void GatewayClientSettings::DisableSyncClient()
		{
			IsSyncClientEnabled = false;
			//Sleep to allow sync gateway client write flush data
			Sleep(1000);
		}

		void GatewayClientSettings::EnableSyncClient()
		{
			IsSyncClientEnabled = true;
		}

		volatile __bool GatewayClientSettings::IsSyncClientEnabled = true;
	}
}