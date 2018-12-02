#include "stdafx.h"
#include "Chronos.Agent.h"
#include "Chronos.Agent.Internal.h"

namespace Chronos
{
	namespace Agent
	{
		GatewayClient::GatewayClient(IStreamFactory* streamFactory)
		{
			_asyncGateway = new AsyncGatewayClient(streamFactory);
			_syncGateway = new SyncGatewayClient(streamFactory);
		}

		GatewayClient::~GatewayClient(void)
		{
			__FREEOBJ(_asyncGateway);
			__FREEOBJ(_syncGateway);
		}

		HRESULT GatewayClient::Initialize(GatewaySettings* gatewaySettings)
		{
			__RETURN_IF_FAILED(_asyncGateway->Initialize(gatewaySettings));
			__RETURN_IF_FAILED(_syncGateway->Initialize(gatewaySettings));
			return S_OK;
		}

		void GatewayClient::Send(GatewayPackage* package)
		{
			if (GatewayClientSettings::IsSyncClientEnabled)
			{
				_syncGateway->Send(package);
			}
			else
			{
				package = GatewayPackage::CreateClone(package);
				_asyncGateway->Send(package);
			}
		}

		void GatewayClient::Send(GatewayPackage* package, __bool leavePackageAlive)
		{
			if (GatewayClientSettings::IsSyncClientEnabled)
			{
				_syncGateway->Send(package);
				if (!leavePackageAlive)
				{
					__FREEOBJ(package);
				}
			}
			else
			{
				if (leavePackageAlive)
				{
					package = GatewayPackage::CreateClone(package);
				}
				_asyncGateway->Send(package);
			}
		}

		void GatewayClient::SendAsync(GatewayPackage* package)
		{
			package = GatewayPackage::CreateClone(package);
			_asyncGateway->Send(package);
		}

		void GatewayClient::SendAsync(GatewayPackage* package, __bool leavePackageAlive)
		{
			if (leavePackageAlive)
			{
				package = GatewayPackage::CreateClone(package);
			}
			_asyncGateway->Send(package);
		}

		void GatewayClient::GetWorkingThreads(std::vector<SingleCoreThread*>* threads)
		{
			_asyncGateway->GetWorkingThreads(threads);
		}

		void GatewayClient::WaitWhileSending()
		{
			_asyncGateway->WaitWhileSending();
		}


		const __guid GatewayClient::ServiceToken = Converter::ConvertStringToGuid(L"{DFE325F9-4761-4741-8C48-D09FD409C1B8}");
	}
}