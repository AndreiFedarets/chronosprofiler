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
				ModuleMetadata::ModuleMetadata(ICorProfilerInfo2* corProfilerInfo, RuntimeMetadataProvider* provider, ModuleID moduleId)
				{
					_corProfilerInfo = corProfilerInfo;
					_provider = provider;
					_metaDataImport = null;
					_moduleId = moduleId;
					_assemblyId = 0;
					_baseLoadAddress = 0;
					_name = null;
					_references = null;
				}

				ModuleMetadata::~ModuleMetadata()
				{
					__FREEOBJ(_name);
					if (_metaDataImport != null)
					{
						_metaDataImport->Release();
					}
				}

				ModuleID ModuleMetadata::GetId()
				{
					return _moduleId;
				}

				AssemblyID ModuleMetadata::GetAssemblyId()
				{
					Initialize();
					return _assemblyId;
				}

				LPCBYTE ModuleMetadata::GetBaseLoadAddress()
				{
					Initialize();
					return _baseLoadAddress;
				}

				__string* ModuleMetadata::GetName()
				{
					Initialize();
					return _name;
				}

				IMetaDataImport2* ModuleMetadata::GetMetadataImport()
				{
					if (_metaDataImport == null)
					{
						HRESULT result = _corProfilerInfo->GetModuleMetaData(_moduleId, ofRead, IID_IMetaDataImport2, (IUnknown**) &_metaDataImport);
					}
					return _metaDataImport;
				}
				
				void ModuleMetadata::Initialize()
				{
					if (_name == null)
					{
						const __uint NameMaxLength = 1000;
						__wchar nativeName[NameMaxLength];
						HRESULT result = _corProfilerInfo->GetModuleInfo(_moduleId, &_baseLoadAddress, NameMaxLength, 0, nativeName, &_assemblyId);
						_name = new __string(nativeName);
					}
				}

				__vector<AssemblyReference*>* ModuleMetadata::GetReferences()
				{
					if (_references == null)
					{
						_references = new __vector<AssemblyReference*>();

						IMetaDataAssemblyImport* assemblyImport = null;
						__RETURN_NULL_IF_FAILED(_corProfilerInfo->GetModuleMetaData(_moduleId, ofRead, IID_IMetaDataAssemblyImport, (IUnknown**)&assemblyImport));

						HCORENUM assemblyEnumerator = null;
						mdAssemblyRef assemblyRefs[32]{ 0 };
						ULONG assemblyRefsCount = 0;
						HRESULT result = assemblyImport->EnumAssemblyRefs(&assemblyEnumerator, assemblyRefs, _countof(assemblyRefs), &assemblyRefsCount);

						for (__uint i = 0; i < assemblyRefsCount; i++)
						{
							/*AssemblyReference* reference = new AssemblyReference(assemblyRefs[i], new __string(assemblyName), assemblyMetadata, publicKeyToken, publicKeyTokenSize);
							_references->push_back(reference);*/
							AssemblyReference* reference = new AssemblyReference(assemblyRefs[i], assemblyImport);
							_references->push_back(reference);

						}
						assemblyImport->Release();
					}
					return _references;
				}

				AssemblyReference* ModuleMetadata::FindReference(__string* assemblyName)
				{
					__vector<AssemblyReference*>* references = GetReferences();
					for (__vector<Reflection::AssemblyReference*>::iterator i = references->begin(); i != references->end(); ++i)
					{
						AssemblyReference* reference = *i;
						if (StringComparer::Equals(assemblyName, reference->GetName(), true))
						{
							return reference;
						}
					}
					return null;
				}
			}
		}
	}
}