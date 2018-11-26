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
				AssemblyReference::AssemblyReference(mdAssemblyRef assemblyRef, IMetaDataAssemblyImport* assemblyImport)
				{
					_assemblyRef = assemblyRef;
					_assemblyImport = assemblyImport;
					_assemblyName = null;
					_assemblyMetadata = { 0 };
					_publicKeyToken = null;
				}

				AssemblyReference::~AssemblyReference()
				{
					__FREEOBJ(_publicKeyToken);
					__FREEOBJ(_assemblyName);
				}

				mdAssemblyRef AssemblyReference::GetToken()
				{
					return _assemblyRef;
				}

				__string* AssemblyReference::GetName()
				{
					Initialize();
					return _assemblyName;
				}

				ASSEMBLYMETADATA AssemblyReference::GetMetadata()
				{
					Initialize();
					return _assemblyMetadata;
				}

				Buffer* AssemblyReference::GetPublicKeyToken()
				{
					Initialize();
					return _publicKeyToken;
				}

				void AssemblyReference::Initialize()
				{
					if (_assemblyName == null)
					{
						__byte* publicKeyToken = null;
						ULONG publicKeyTokenSize = 0;
						wchar_t assemblyName[255] { 0 };
						ULONG assemblyNameLength = 0;
						char* hashValue = null;
						ULONG hashLength = 0;
						DWORD flags = 0;
						HRESULT result = _assemblyImport->GetAssemblyRefProps(_assemblyRef, (const void**)&publicKeyToken, &publicKeyTokenSize, assemblyName, _countof(assemblyName), &assemblyNameLength, &_assemblyMetadata, (const void**)&hashValue, &hashLength, &flags);
						_assemblyName = new __string(assemblyName);
						_publicKeyToken = new Buffer(publicKeyToken, publicKeyTokenSize);
					}
				}
			}
		}
	}
}