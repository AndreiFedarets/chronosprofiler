#include "stdafx.h"	
#include "Chronos.Agent.h"

namespace Chronos
{
	namespace Agent
	{
		GatewayPackageWriter::GatewayPackageWriter(GatewayClient* gatewayClient, __byte dataMarker, __uint packageSize)
		{
			_gatewayClient = gatewayClient;
			_dataMarker = dataMarker;
			_packageSize = packageSize;
			_currentPackage = GatewayPackage::CreateDynamic(_dataMarker);
		}

		GatewayPackageWriter::~GatewayPackageWriter()
		{
			if (_currentPackage->GetDataSize() > 0)
			{
				_gatewayClient->Send(_currentPackage);
			}
			else
			{
				__FREEOBJ(_currentPackage);
			}
		}

		__uint GatewayPackageWriter::Write(void* data, __uint size)
		{
			if (_currentPackage->GetDataSize() + size >= _packageSize)
			{
				_gatewayClient->Send(_currentPackage, false);
				_currentPackage = GatewayPackage::CreateDynamic(_dataMarker);
			}
			return _currentPackage->Write(data, size);
		}

		__bool GatewayPackageWriter::Initialized()
		{
			return _currentPackage != null;
		}
	}
}