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
				AppDomainMetadata::AppDomainMetadata(ICorProfilerInfo2* corProfilerInfo, RuntimeMetadataProvider* provider, AppDomainID appDomainId)
				{
					_corProfilerInfo = corProfilerInfo;
					_provider = provider;
					_appDomainId = appDomainId;
					_name = null;
				}

				AppDomainMetadata::~AppDomainMetadata()
				{
					__FREEOBJ(_name);
				}

				AssemblyID AppDomainMetadata::GetId()
				{
					return _appDomainId;
				}

				__string* AppDomainMetadata::GetName()
				{
					Initialize();
					return _name;
				}
				
				void AppDomainMetadata::Initialize()
				{
					if (_name == null)
					{
						const __uint NameMaxLength = 1000;
						__wchar nativeName[NameMaxLength];
						HRESULT result = _corProfilerInfo->GetAppDomainInfo(_appDomainId, NameMaxLength, 0, (__wchar*)&nativeName, 0);
						_name = new __string(nativeName);
					}
				}
			}
		}
	}
}