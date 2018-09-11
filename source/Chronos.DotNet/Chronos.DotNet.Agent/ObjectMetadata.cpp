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
				ObjectMetadata::ObjectMetadata(ICorProfilerInfo2* corProfilerInfo, RuntimeMetadataProvider* provider, ObjectID objectId)
				{
					_corProfilerInfo = corProfilerInfo;
					_provider = provider;
					_objectId = objectId;
					_typeMetadata = null;
				}

				ObjectMetadata::~ObjectMetadata()
				{
				}
			}
		}
	}
}