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
				ParameterMetadata::ParameterMetadata(IMetaDataImport2* metaDataImport, PCCOR_SIGNATURE* corSignature)
				{
					_name = null;
					_elementType = (CorElementType)0;
					_typeName = null;
					_isArray = false;
					_byRef = false;
					_typeToken = 0;
					Initialize(metaDataImport, corSignature);
				}

				ParameterMetadata::~ParameterMetadata()
				{
					__FREEOBJ(_name);
					__FREEOBJ(_typeName);
				}

				void ParameterMetadata::Initialize(IMetaDataImport2* metaDataImport, PCCOR_SIGNATURE* corSignature)
				{
					PCCOR_SIGNATURE paramSignature = *corSignature;
					ParseSignature(paramSignature, metaDataImport);
					ParseType(*corSignature, metaDataImport);
				}
				
				PCCOR_SIGNATURE ParameterMetadata::ParseSignature(PCCOR_SIGNATURE signature, IMetaDataImport* metaDataImport)
				{
					_typeName = new __string();
					COR_SIGNATURE corSignature = *signature++;
					_elementType = static_cast<CorElementType>(corSignature);

					switch (corSignature) 
					{	
						case ELEMENT_TYPE_VOID:
							_typeName->append(L"void");
							break;
						case ELEMENT_TYPE_BOOLEAN:
							_typeName->append(L"bool");
							break;
						case ELEMENT_TYPE_CHAR:
							_typeName->append(L"wchar");
							break;
						case ELEMENT_TYPE_I1:
							_typeName->append(L"byte");
							break;
						case ELEMENT_TYPE_U1:
							_typeName->append(L"ubyte");
							break;
						case ELEMENT_TYPE_I2:
							_typeName->append(L"short");
							break;
						case ELEMENT_TYPE_U2:
							_typeName->append(L"ushort");
							break;
						case ELEMENT_TYPE_I4:
							_typeName->append(L"int");
							break;
						case ELEMENT_TYPE_U4:
							_typeName->append(L"uint");
							break;
						case ELEMENT_TYPE_I8:
							_typeName->append(L"long");
							break;
						case ELEMENT_TYPE_U8:
							_typeName->append(L"ulong");
							break;
						case ELEMENT_TYPE_R4:
							_typeName->append(L"float");
							break;
						case ELEMENT_TYPE_R8:
							_typeName->append(L"double");
							break;
						case ELEMENT_TYPE_STRING:
							_typeName->append(L"string");
							break;
						case ELEMENT_TYPE_VAR:
							_typeName->append(L"class variable(unsigned int8)");
							break;
						case ELEMENT_TYPE_MVAR:
							_typeName->append(L"method variable(unsigned int8)");
							break;
						case ELEMENT_TYPE_TYPEDBYREF:
							_typeName->append(L"refany");
							break;
						case ELEMENT_TYPE_I:
							_typeName->append(L"int");
							break;
						case ELEMENT_TYPE_U:
							_typeName->append(L"uint");
							break;
						case ELEMENT_TYPE_OBJECT:
							_typeName->append(L"object");
							break;
						case ELEMENT_TYPE_SZARRAY:
							signature = ParseSignature(signature, metaDataImport);
							_typeName->append(L"[]");
							break;
						case ELEMENT_TYPE_PINNED:
							signature = ParseSignature(signature, metaDataImport);
							_typeName->append(L"pinned");
							break;
						case ELEMENT_TYPE_PTR:
							signature = ParseSignature(signature, metaDataImport);
							_typeName->append(L"*");
							break;
						case ELEMENT_TYPE_BYREF:
							signature = ParseSignature(signature, metaDataImport);
							_typeName->append(L"&");
							break;
						case ELEMENT_TYPE_VALUETYPE:
						case ELEMENT_TYPE_CLASS:
						case ELEMENT_TYPE_CMOD_REQD:
						case ELEMENT_TYPE_CMOD_OPT:
							{
								mdToken token;
								signature += CorSigUncompressToken(signature, &token); 
								if (TypeFromToken(token) == mdtTypeRef)
								{
									ULONG classNameSize = 0;
									metaDataImport->GetTypeRefProps(token, NULL, 0, 0, &classNameSize);
									wchar_t* className = new wchar_t[classNameSize];
									metaDataImport->GetTypeRefProps(token, NULL, className, classNameSize, 0);
									_typeName->append(className);
								}
								else
								{
									ULONG classNameSize = 0;
									metaDataImport->GetTypeDefProps(token, NULL, NULL, &classNameSize, NULL, NULL);
									wchar_t* className = new wchar_t[classNameSize];
									metaDataImport->GetTypeDefProps(token, className, classNameSize, NULL, NULL, NULL);
									_typeName->append(className);
								}

							}
							break;
						case ELEMENT_TYPE_GENERICINST:
							{
								signature = ParseSignature(signature, metaDataImport);
								_typeName->append(L"<");
								ULONG arguments = CorSigUncompressData(signature);
								for (ULONG i = 0; i < arguments; ++i)
								{
									if(i != 0)
									{
										_typeName->append(L", ");
									}
									signature = ParseSignature(signature, metaDataImport);
								}
								_typeName->append(L"<");
							}
							break;
						case ELEMENT_TYPE_ARRAY:
							{
								signature = ParseSignature(signature, metaDataImport);
								ULONG rank = CorSigUncompressData(signature);
								if (rank == 0) 
								{
									_typeName->append(L"[?]");
								}
								else 
								{		
									ULONG arraysize = (sizeof(ULONG) * 2 * rank);
									ULONG *lower = (ULONG *)_alloca(arraysize);
									memset(lower, 0, arraysize); 
									ULONG *sizes = &lower[rank];

									ULONG numsizes = CorSigUncompressData(signature);
									for (ULONG i = 0; i < numsizes && i < rank; i++)
									{
										sizes[i] = CorSigUncompressData(signature);
									}

									ULONG numlower = CorSigUncompressData(signature);
									for (ULONG i = 0; i < numlower && i < rank; i++)
									{
										lower[i] = CorSigUncompressData(signature);
									}

									_typeName->append(L"[");
									for (ULONG i = 0; i < rank; ++i)
									{					
										if (i > 0) 
										{
											_typeName->append(L",");
										}

										if (lower[i] == 0)
										{
											if(sizes[i] != 0)
											{
												_typeName->append(Converter::ConvertIntToString(sizes[i]));
											}
										}	
										else	
										{
											_typeName->append(Converter::ConvertIntToString(lower[i]));
											_typeName->append(L"...");

											if (sizes[i] != 0)
											{
												_typeName->append(Converter::ConvertIntToString(lower[i] + sizes[i] + 1));
											}
										}
									}
									_typeName->append(L"]");
								}
							} 
							break;
						//----------------------
						case ELEMENT_TYPE_FNPTR:
							_typeName->append(L"function");
							break;
						case ELEMENT_TYPE_INTERNAL:
							_typeName->append(L"<INTERNAL>");
							break;
						case ELEMENT_TYPE_MAX:
							_typeName->append(L"<INVALID>");
							break;
						case ELEMENT_TYPE_MODIFIER:
							_typeName->append(L"<MODIFIER>");
							break;
						/*case ELEMENT_TYPE_R4_HFA:
							_typeName->append(L"<R4_HFA>");
							break;
						case ELEMENT_TYPE_R8_HFA:
							_typeName->append(L"<R8_HFA>");
							break;*/
						//----------------------
						case ELEMENT_TYPE_END:
						case ELEMENT_TYPE_SENTINEL:
							_typeName->append(L"<UNKNOWN>");
							break;
					}
					return signature;
				}

				void ParameterMetadata::ParseType(PCCOR_SIGNATURE& signature, IMetaDataImport* metaDataImport)
				{
					_elementType = (CorElementType) *signature++;
					_byRef = (ELEMENT_TYPE_BYREF == _elementType);
					if(_byRef)
					{
						_elementType = (CorElementType) *signature++;
					}

					_isArray = (ELEMENT_TYPE_SZARRAY == _elementType);

					if(_isArray)
					{
						_elementType = (CorElementType) *signature++;
					}

					_typeToken = mdTypeDefNil;

					if(ELEMENT_TYPE_VALUETYPE == _elementType || ELEMENT_TYPE_CLASS == _elementType)
					{
						signature += CorSigUncompressToken(signature, &_typeToken);
					}
				}
			}
		}
	}
}