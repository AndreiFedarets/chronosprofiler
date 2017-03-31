#include "stdafx.h"
#include "Chronos.Agent.Internal.h"

namespace Chronos
{
	namespace Agent
	{
		SyncGatewayClient::SyncGatewayClient(IStreamFactory* streamFactory)
		{
			_streamFactory = streamFactory;
			_currentStreamId = 0;
			_syncStreamsCount = 0;
		}

		SyncGatewayClient::~SyncGatewayClient()
		{
		}

		HRESULT SyncGatewayClient::Initialize(GatewaySettings* gatewaySettings)
		{
			HRESULT result = S_OK;
			if (!gatewaySettings->GetSyncStreamsCount(&_syncStreamsCount))
			{
				return E_FAIL;
			}
			for (__byte i = 0; i < _syncStreamsCount; i++)
			{
				HRESULT r = _streamFactory->ConnectDaemonDataStream(&(_streams[i]));
				if (FAILED(r))
				{
					__ASSERT(false, L"SyncGatewayClient::Initialize: _streamFactory->ConnectDaemonDataStream failed");
					result = r;
				}
			}
			return result;
		}
		
		void SyncGatewayClient::Send(GatewayPackage* package)
		{
			if (_stream == null)
			{
				Lock lock(&_criticalSection);
				_stream = _streams[_currentStreamId];
				_currentStreamId++;
				if (_currentStreamId >= _syncStreamsCount)
				{
					_currentStreamId = 0;
				}
			}
			__byte* buffer = package->GetBuffer();
			__uint dataSize = package->GetPayloadSize();
			_stream->Write(buffer, dataSize);
		}

		IStream* SyncGatewayClient::_stream = null;
	}
}