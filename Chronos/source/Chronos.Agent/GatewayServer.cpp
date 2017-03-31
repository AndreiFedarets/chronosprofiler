#include "stdafx.h"
#include "Chronos.Agent.h"

namespace Chronos
{
	namespace Agent
	{
		GatewayServer::GatewayServer(IStreamFactory* streamFactory)
		{
			_streamFactory = streamFactory;
			_streams = new __vector<GatewayServerStream*>();
			_handlers = new IDataHandler*[256];
			_isLocked = false;
			for (__uint i = 0; i < 256; ++i)
			{
				_handlers[i] = null;
			}
		}

		GatewayServer::~GatewayServer(void)
		{
			for (__vector<GatewayServerStream*>::iterator i = _streams->begin(); i != _streams->end(); ++i)
			{
				GatewayServerStream* stream = *i;
				__FREEOBJ(stream);
			}
			__FREEOBJ(_streams);
		}

		HRESULT GatewayServer::Start(__byte streamsCount)
		{
			for (__byte i = 0; i < streamsCount; ++i)
			{
				IServerStream* serverStream;
				__RETURN_IF_FAILED( _streamFactory->CreateDaemonDataStream(&serverStream) );
				GatewayServerStream* gatewayStream = new GatewayServerStream(serverStream, _handlers);
				_streams->push_back(gatewayStream);
			}
			return S_OK;
		}

		void GatewayServer::StartReading()
		{
			for (__vector<GatewayServerStream*>::iterator i = _streams->begin(); i != _streams->end(); ++i)
			{
				GatewayServerStream* stream = *i;
				stream->StartReading();
			}
		}

		void GatewayServer::StopReading()
		{
			for (__vector<GatewayServerStream*>::iterator i = _streams->begin(); i != _streams->end(); ++i)
			{
				GatewayServerStream* stream = *i;
				stream->StopReading();
			}
		}

		HRESULT GatewayServer::RegisterHandler(__byte dataMarker, IDataHandler* handler)
		{
			if (_isLocked)
			{
				return E_FAIL;
			}
			if (_handlers[dataMarker] == null)
			{
				_handlers[dataMarker] = handler;
				return S_OK;
			}
			return E_FAIL;
		}

		__bool GatewayServer::IsLocked()
		{
			return _isLocked;
		}

		void GatewayServer::Lock()
		{
			_isLocked = true;
		}
	}
}