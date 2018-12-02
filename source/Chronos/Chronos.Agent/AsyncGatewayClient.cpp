#include "stdafx.h"
#include "Chronos.Agent.Internal.h"

namespace Chronos
{
	namespace Agent
	{
		AsyncGatewayClient::AsyncGatewayClient(IStreamFactory* streamFactory)
		{
			for (__ushort i = 0; i < PackagesMaxCount; i++)
			{
				_packages[i] = new GatewayPackageContainer();
			}
			_streamFactory = streamFactory;
			_asyncStreamsCount = 0;
		}

		AsyncGatewayClient::~AsyncGatewayClient()
		{
			_sending = false;
			while (_sendingThread->IsAlive())
			{
				Sleep(1);
			}
			__FREEOBJ(_sendingThread);
			for (__ushort i = 0; i < PackagesMaxCount; i++)
			{
				__FREEOBJ(_packages[i]);
			}
		}

		HRESULT AsyncGatewayClient::Initialize(GatewaySettings* gatewaySettings)
		{
			if (!gatewaySettings->GetAsyncStreamsCount(&_asyncStreamsCount))
			{
				return E_FAIL;
			}
			ThisCallback<AsyncGatewayClient>* callback = new ThisCallback<AsyncGatewayClient>(this, &AsyncGatewayClient::StartSending);
			_sendingThread = new MultiCoreThread(callback, _asyncStreamsCount);
			_sending = true;
			_sendingThread->Start();
			//_sendingThread->SetPriority(IThread::HighestPriority);
			return S_OK;
		}

		void AsyncGatewayClient::Send(GatewayPackage* package)
		{
			//Go through the array until package will be inserted
			__bool packageSent = false;
			do
			{
				for (__ushort i = 0; i < PackagesMaxCount; i++)
				{
					GatewayPackageContainer* container = _packages[i];
					if (container->Value == null)
					{
						GatewayPackage* temp = container->SetValue(package);
						//If we took not-null value (e.g. if second thread setted value before us)
						//then we should 're-send' this package again
						if (temp != null)
						{
							Send((GatewayPackage*)temp);
						}
						packageSent = true;
						break;
					}
				}
				if (!packageSent)
				{
					Sleep(1);
				}
			}
			while (!packageSent);
		}
		
		void AsyncGatewayClient::StartSending(void* parameter)
		{
			IStream* stream = null;
			HRESULT result = _streamFactory->ConnectDaemonDataStream(&stream);
			if (FAILED(result))
			{
				__ASSERT(false, L"AsyncGatewayClient::StartSending: _streamFactory->ConnectDaemonDataStream failed");
				return;
			}
			__byte step = (__byte)parameter;
			__bool packageSent;
			while (_sending)
			{
				packageSent = false;
				for (__ushort i = 0; i < PackagesMaxCount ; ++i)
				{
					GatewayPackageContainer* container = _packages[i];
					if (container->Value != null)
					{
						GatewayPackage* package = container->SetValue(null);
						//If we took not-null value (e.g. if second thread setted value before us)
						//then we should 're-send' this package again
						if (package != null)
						{
							__byte* buffer = package->GetBuffer();
							__uint payloadSize = package->GetPayloadSize();
							stream->Write(buffer, payloadSize);
							__FREEOBJ(package);
							packageSent = true;
						}
					}
				}
				if (!packageSent)
				{
					Sleep(1);
				}
			}
		}

		void AsyncGatewayClient::GetWorkingThreads(std::vector<SingleCoreThread*>* threads)
		{
			_sendingThread->GetWorkingThreads(threads);
		}

		void AsyncGatewayClient::WaitWhileSending()
		{
			__byte attempts = 0;
			while (attempts < 4)
			{
				if (PackagesCount() == 0)
				{
					attempts++;
				}
				else
				{
					attempts = 0;
				}
			}
		}
		
		__size AsyncGatewayClient::PackagesCount()
		{
			__int count = 0;
			for (__ushort i = 0; i < PackagesMaxCount ; ++i)
			{
				GatewayPackageContainer* container = _packages[i];
				if (container->Value != null)
				{
					count++;
				}
			}
			return count;
		}

	}
}