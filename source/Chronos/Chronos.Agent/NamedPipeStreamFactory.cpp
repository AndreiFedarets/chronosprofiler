#include "stdafx.h"
#include "Chronos.Agent.h"

namespace Chronos
{
	namespace Agent
	{
		NamedPipeStreamFactory::NamedPipeStreamFactory(void)
			: _lastDaemonDataStreamIndex(0)
		{
		}

		NamedPipeStreamFactory::~NamedPipeStreamFactory(void)
		{
		}

		HRESULT NamedPipeStreamFactory::InitializeDaemonStreams(__guid sessionUid)
		{
			_sessionToken = sessionUid;
			return S_OK;
		}

		HRESULT NamedPipeStreamFactory::ConnectDaemonDataStream(IStream** stream)
		{
			__byte index = (__byte)InterlockedIncrement(&_lastDaemonDataStreamIndex) - 1;
			//format: chronosprofiler_[SESSION_UID]_data_[INDEX]
			//example: chronosprofiler_1B6E1937B1A84E54B2E0D4DC1CD7642A_data_0
			std::wstring port(L"chronosprofiler_");
			port.append(Converter::ConvertGuidToString(_sessionToken, 'N'));
			port.append(L"_data_");
			port.append(Converter::ConvertIntToString(index));

			NamedPipeClientStream* clientStream = new NamedPipeClientStream(port.c_str(), IStream::Output);

			if (!clientStream->Initialized())
			{
				__FREEOBJ(clientStream);
				return E_FAIL;
			}
			*stream = clientStream;
			return S_OK;
		}

		HRESULT NamedPipeStreamFactory::ConnectHostInvokeStream(IStream** stream)
		{
			//format: chronosprofiler_1B6E1937B1A84E54B2E0D4DC1CD7642A_invoke
			std::wstring port(L"chronosprofiler_1B6E1937B1A84E54B2E0D4DC1CD7642A_invoke");

			*stream = new NamedPipeClientStream(port.c_str(), IStream::Dumplex);
			if (!(*stream)->Initialized())
			{
				__FREEOBJ(stream);
				return E_FAIL;
			}
			return S_OK;
		}

		HRESULT NamedPipeStreamFactory::CreateAgentInvokeStream(IServerStream** stream, GUID agentApplicationUid)
		{
			__uint processId = CurrentProcess::GetProcessId();
			
			//format: chronosprofiler_[AGENT_APPLICATION_UID]_invoke
			//example: chronosprofiler_1B6E1937B1A84E54B2E0D4DC1CD7642A_invoke
			std::wstring port(L"chronosprofiler_");
			port.append(Converter::ConvertGuidToString(agentApplicationUid, 'N'));
			port.append(L"_invoke");

			*stream = new NamedPipeServerStream(port.c_str(), IStream::Dumplex);

			if (!(*stream)->Initialized())
			{
				__FREEOBJ(stream);
				return E_FAIL;
			}
			return S_OK;
		}

		HRESULT NamedPipeStreamFactory::CreateDaemonDataStream(IServerStream** stream)
		{
			__byte index = (__byte)InterlockedIncrement(&_lastDaemonDataStreamIndex) - 1;
			//format: chronosprofiler_[SESSION_UID]_data_[INDEX]
			//example: chronosprofiler_1B6E1937B1A84E54B2E0D4DC1CD7642A_data_0
			std::wstring port(L"chronosprofiler_");
			port.append(Converter::ConvertGuidToString(_sessionToken, 'N'));
			port.append(L"_data_");
			port.append(Converter::ConvertIntToString(index));

			NamedPipeServerStream* serverStream = new NamedPipeServerStream(port.c_str(), IStream::Input);

			if (!serverStream->Initialized())
			{
				__FREEOBJ(serverStream);
				return E_FAIL;
			}
			*stream = serverStream;
			return S_OK;
		}
	}
}