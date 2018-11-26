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
				FieldMetadata::FieldMetadata(IMetaDataImport2* metadataImport, mdFieldDef fieldToken)
				{
					_metadataImport = metadataImport;
					_fieldToken = fieldToken;
					_name = null;
				}

				FieldMetadata::~FieldMetadata()
				{
				}

				mdFieldDef FieldMetadata::GetToken()
				{
					return _fieldToken;
				}

				__string* FieldMetadata::GetName()
				{
					Initialize();
					return _name;
				}

				void FieldMetadata::Initialize()
				{
					if (_name == null)
					{
						const __uint NameMaxLength = 1000;
						__wchar nativeName[NameMaxLength];
						_metadataImport->GetFieldProps(_fieldToken, 0, nativeName, NameMaxLength, 0, 0, 0, 0, 0, 0, 0);
						_name = new __string(nativeName);
					}
				}

			}
		}
	}
}