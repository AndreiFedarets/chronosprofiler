#include "stdafx.h"
#include "Chronos.DotNet.BasicProfiler.Agent.h"

namespace Chronos
{
	namespace Agent
	{
		namespace DotNet
		{
			namespace BasicProfiler
			{
				HRESULT AssemblyCollection::InitializeUnitSpecial(AssemblyInfo* unit)
				{
					unit->InitializeMetadata(_metadataProvider);
					return S_OK;
				}

				const __guid AssemblyCollection::ServiceToken = Converter::ConvertStringToGuid(L"{9AC093C1-AF4A-4999-9B4E-4DB6414D8762}");
			}
		}
	}
}