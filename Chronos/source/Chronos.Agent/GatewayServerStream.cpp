#include "stdafx.h"
#include "Chronos.Agent.h"

namespace Chronos
{
	namespace Agent
	{
		GatewayServerStream::GatewayServerStream(IServerStream* stream, IDataHandler** handlers)
		{
			_handlers = handlers;
			_stream = stream;
			ThisCallback<GatewayServerStream>* callback = new ThisCallback<GatewayServerStream>(this, &GatewayServerStream::StartPackagesReading);
			_thread = new SingleCoreThread(callback);
			_reading = false;
			_started = false;
		}

		GatewayServerStream::~GatewayServerStream()
		{
			__FREEOBJ(_thread);
		}

		void GatewayServerStream::StartReading()
		{
			if (_started)
			{
				return;
			}
			_reading = true;
			_thread->Start();
			//Wait while starting
			while (!_started)
			{
				Sleep(1);
			}
		}

		void GatewayServerStream::StopReading()
		{
			_reading = false;
			//Wait while stopping
			while (_started)
			{
				Sleep(1);
			}
		}

		void GatewayServerStream::StartPackagesReading(void* parameter)
		{
			_started = true;
			_stream->WaitForConnection();
			while (_reading)
			{
				__byte dataMarker = 0;
				__uint dataSize = 0;
				__byte* data = null;
				GatewayPackage::ReadPackage(_stream, &dataMarker, &data, &dataSize);
				if (dataSize == 0)
				{
					if (_stream->Disconnected())
					{
						break;
					}
					else
					{
						continue;
					}
				}
				IDataHandler* handler = _handlers[dataMarker];
				if (handler == null || handler->HandlePackage(data, dataSize))
				{
					__FREEARR(data);
					continue;
				}
			}
			_started = false;
		}
	}
}