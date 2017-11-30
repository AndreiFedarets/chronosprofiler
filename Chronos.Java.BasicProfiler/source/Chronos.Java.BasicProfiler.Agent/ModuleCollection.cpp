#include "stdafx.h"
#include "Chronos.Java.BasicProfiler.Agent.h"

namespace Chronos
{
	namespace Agent
	{
		namespace Java
		{
			namespace BasicProfiler
			{
				HRESULT ModuleCollection::InitializeUnitSpecial(ModuleInfo* unit)
				{
					unit->InitializeMetadata(_metadataProvider);
					return S_OK;
				}

				const __guid ModuleCollection::ServiceToken = Converter::ConvertStringToGuid(L"{C91A24B3-0E3D-4F37-BC01-DC89E870A9E2}");
			}
		}
	}
}