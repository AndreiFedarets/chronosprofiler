#include "stdafx.h"
#include "Chronos.Agent.h"

namespace Chronos
{
	namespace Agent
	{
		HostClient::HostClient(IStreamFactory* streamFactory)
		{
			_streamFactory = streamFactory;
		}

		__bool HostClient::StartProfilingSession(__guid configurationToken, __guid applicationUid, SessionSettings* settings)
		{
			IStream* stream;
			HRESULT result = _streamFactory->ConnectHostInvokeStream(&stream);
			if (FAILED(result))
			{
				return false;
			}
			
			MemoryStream argumentsStream;
			__int currentProcessId = CurrentProcess::GetProcessId();
			Marshaler::MarshalGuid(&configurationToken, &argumentsStream);
			Marshaler::MarshalInt(currentProcessId, &argumentsStream);
			Marshaler::MarshalInt(CurrentProcess::GetProcessPlatform(), &argumentsStream);
			Marshaler::MarshalGuid(&applicationUid, &argumentsStream);
			
			__guid startProfilingSessionOperationCode = Converter::ConvertStringToGuid(L"{B68D7CDC-E999-416A-A9D0-E4A22D243E5F}");
			Marshaler::MarshalGuid(&startProfilingSessionOperationCode, stream);
			Buffer* arguments = argumentsStream.ToArray();
			Marshaler::MarshalBuffer(arguments, stream);
			__FREEOBJ(arguments);

			__int resultCode = Marshaler::DemarshalInt(stream);
			Buffer* resultBuffer = Marshaler::DemarshalBuffer(stream);
			MemoryStream resultStream(resultBuffer->Data, resultBuffer->Size);
			__FREEOBJ(resultBuffer);
			if (resultCode != 0 || resultStream.GetLength() == 0)
			{
				return false;
			}
			if (!settings->Initialize(&resultStream))
			{
				return false;
			}
			return true;
		}
	}
}
