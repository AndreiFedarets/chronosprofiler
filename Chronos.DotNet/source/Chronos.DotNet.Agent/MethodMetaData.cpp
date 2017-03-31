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
				MethodMetadata::MethodMetadata(ICorProfilerInfo2* corProfilerInfo, RuntimeMetadataProvider* provider, FunctionID functionId)
				{
					_corProfilerInfo = corProfilerInfo;
					_provider = provider;
					_functionId = functionId;
					_methodToken = 0;
					_classId = 0;
					_typeToken = 0;
					_moduleId = 0;
					_assemblyId = 0;
					_name = null;
					_returnParameter = null;
					_parameters = null;
				}

				MethodMetadata::~MethodMetadata()
				{
					__FREEOBJ(_name);
					__FREEOBJ(_returnParameter);
					if (_parameters != null)
					{
						for (__vector<ParameterMetadata*>::iterator i = _parameters->begin(); i != _parameters->end(); ++i)
						{
							ParameterMetadata* parameter = *i;
							__FREEOBJ(parameter);
						}
					}
				}

				FunctionID MethodMetadata::GetId()
				{
					return _functionId;
				}

				mdMethodDef MethodMetadata::GetMethodToken()
				{
					Initialize();
					return _methodToken;
				}

				ClassID MethodMetadata::GetTypeId()
				{
					Initialize();
					return _classId;
				}

				mdTypeDef MethodMetadata::GetTypeToken()
				{
					Initialize();
					return _typeToken;
				}

				ModuleID MethodMetadata::GetModuleId()
				{
					Initialize();
					return _moduleId;
				}
				
				AssemblyID MethodMetadata::GetAssemblyId()
				{
					Initialize();
					return _assemblyId;
				}

				__string* MethodMetadata::GetName()
				{
					Initialize();
					return _name;
				}

				void MethodMetadata::Initialize()
				{
					if (_name == null)
					{
						HRESULT result;
						IMetaDataImport2* metaDataImport = null;

						result = _corProfilerInfo->GetFunctionInfo(_functionId, &_classId, &_moduleId, &_methodToken);

						result = _corProfilerInfo->GetModuleInfo(_moduleId, 0, 0, 0, null, &_assemblyId);
						result = _corProfilerInfo->GetTokenAndMetaDataFromFunction(_functionId, IID_IMetaDataImport2, (IUnknown**)&metaDataImport, &_methodToken);
			
						PCCOR_SIGNATURE corSignature;
						const __uint NameMaxLength = 1000;
						__wchar nativeName[NameMaxLength];
						result = metaDataImport->GetMethodProps(_methodToken, &_typeToken, nativeName, NameMaxLength, 0, 0, &corSignature, 0, 0, 0);

						_name = new __string(nativeName);

						//_parameters = new __vector<ParameterMetadata*>();

						//corSignature += CorSigUncompressData(corSignature, (ULONG*)&_callingConvention);

						////ArgumentsCount
						//ULONG argumentsCount = 0;
						//if (_callingConvention != IMAGE_CEE_CS_CALLCONV_FIELD)
						//{
						//	corSignature += CorSigUncompressData(corSignature, &argumentsCount);
						//}

						//ParameterMetadata* parameter;
						//_returnParameter = new ParameterMetadata(metaDataImport, &corSignature);
						//for (ULONG i = 0; i < argumentsCount; i++)
						//{
						//	parameter = new ParameterMetadata(metaDataImport, &corSignature);
						//	_parameters->push_back(parameter);
						//}

						metaDataImport->Release();
					}
				}
			}
		}
	}
}