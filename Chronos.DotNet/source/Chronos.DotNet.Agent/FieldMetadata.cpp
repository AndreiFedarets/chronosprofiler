#include "stdafx.h"
#include "Chronos.DotNet.Agent.h"

namespace Chronos
{
	namespace Agent
	{
		namespace DotNet
		{
			namespace Reflection
			{
				FieldMetadata::FieldMetadata(ICorProfilerInfo2* corProfilerInfo, TypeMetadata* type, mdFieldDef fieldToken)
				{
					_corProfilerInfo = corProfilerInfo;
					_type = type;
					_fieldToken = fieldToken;
					_metaDataImport = null;
				}

				FieldMetadata::~FieldMetadata()
				{
					if (_metaDataImport != null)
					{
						_metaDataImport->Release();
					}
				}

				mdFieldDef FieldMetadata::GetFieldToken()
				{
					return _fieldToken;
				}
			}
		}
	}
}