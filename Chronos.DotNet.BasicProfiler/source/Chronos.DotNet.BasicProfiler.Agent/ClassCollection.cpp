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
				HRESULT ClassCollection::InitializeUnitSpecial(ClassInfo* unit)
				{
					unit->InitializeMetadata(_metadataProvider);
					return S_OK;
				}

				const __guid ClassCollection::ServiceToken = Converter::ConvertStringToGuid(L"{FFC95003-A1E3-4222-A855-346373680918}");
			}
		}
	}
}