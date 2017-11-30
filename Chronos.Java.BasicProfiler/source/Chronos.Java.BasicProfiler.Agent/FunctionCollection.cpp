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
				HRESULT FunctionCollection::InitializeUnitSpecial(FunctionInfo* unit)
				{
					unit->InitializeMetadata(_metadataProvider);
					return S_OK;
				}
	
				const __guid FunctionCollection::ServiceToken = Converter::ConvertStringToGuid(L"{96B10F0C-91E5-4D81-9EBA-AD7F8BD71B13}");
			}
		}
	}
}