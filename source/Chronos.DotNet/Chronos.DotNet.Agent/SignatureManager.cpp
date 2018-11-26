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
				namespace Emit
				{
					Signature* SignatureManager::Alloc()
					{
						Signature* signature = new Signature();
						memset(signature, 0, sizeof(Signature));
						return signature;
					}

					SignatureElement* SignatureManager::AllocElement()
					{
						SignatureElement* element = new SignatureElement();
						memset(element, 0, sizeof(SignatureElement));
						return element;
					}

					void SignatureManager::Release(Signature* signature)
					{
						if (signature == null)
						{
							return;
						}
						ReleaseElementChain(signature->Front);
						__FREEOBJ(signature);
					}

					void SignatureManager::ReleaseElement(SignatureElement* element)
					{
						if (element == null)
						{
							return;
						}
						__FREEOBJ(element);
					}

					void SignatureManager::ReleaseElementChain(SignatureElement* chain)
					{
						SignatureElement* current = chain;
						while (current != null)
						{
							SignatureElement* next = current->Next;
							__FREEOBJ(current);
							current = next;
						}
					}

					Signature* SignatureManager::Read(mdSignature signatureToken, IMetaDataImport* metadataImport)
					{
						PCCOR_SIGNATURE corSignature = null;
						ULONG corSignatureSize;
						__RETURN_NULL_IF_FAILED(metadataImport->GetSigFromToken(signatureToken, &corSignature, &corSignatureSize));
						return Read(corSignature);
					}

					Signature* SignatureManager::Read(PCCOR_SIGNATURE corSignature)
					{
						Signature* signature = Alloc();
						CorCallingConvention callConvention;
						corSignature += CorSigUncompressData(corSignature, (ULONG*)&callConvention);
						signature->CallingConvention = callConvention;

						ULONG corElementsCount;
						corSignature += CorSigUncompressData(corSignature, &corElementsCount);
						if ((callConvention & CorCallingConvention::IMAGE_CEE_CS_CALLCONV_LOCAL_SIG) != CorCallingConvention::IMAGE_CEE_CS_CALLCONV_LOCAL_SIG &&
							(callConvention & CorCallingConvention::IMAGE_CEE_CS_CALLCONV_FIELD) != CorCallingConvention::IMAGE_CEE_CS_CALLCONV_FIELD)
						{
							//add return type;
							corElementsCount++;
						}
						SignatureElement* current = null;
						for (__uint i = 0; i < corElementsCount; i++)
						{
							SignatureElement* previous = current;
							current = ReadElement(&corSignature);
							if (signature->Front == null)
							{
								signature->Front = current;
							}
							else
							{
								previous->Next = current;
							}
						}
						return signature;
					}

					SignatureElement* SignatureManager::ReadElement(PCCOR_SIGNATURE* corSignatureRef)
					{
						SignatureElement* element = AllocElement();
						PCCOR_SIGNATURE corSignature = *corSignatureRef;
						CorElementType elementType;
						corSignature += CorSigUncompressElementType(corSignature, &elementType);
						element->ElementType = elementType;
						switch (elementType)
						{
							case ELEMENT_TYPE_VOID:
							case ELEMENT_TYPE_BOOLEAN:
							case ELEMENT_TYPE_CHAR:
							case ELEMENT_TYPE_I1:
							case ELEMENT_TYPE_U1:
							case ELEMENT_TYPE_I2:
							case ELEMENT_TYPE_U2:
							case ELEMENT_TYPE_I4:
							case ELEMENT_TYPE_U4:
							case ELEMENT_TYPE_I8:
							case ELEMENT_TYPE_U8:
							case ELEMENT_TYPE_R4:
							case ELEMENT_TYPE_R8:
							case ELEMENT_TYPE_STRING:
							case ELEMENT_TYPE_VAR:
							case ELEMENT_TYPE_MVAR:
							case ELEMENT_TYPE_TYPEDBYREF:
							case ELEMENT_TYPE_I:
							case ELEMENT_TYPE_U:
							case ELEMENT_TYPE_OBJECT:
							case ELEMENT_TYPE_FNPTR:
							case ELEMENT_TYPE_INTERNAL:
							case ELEMENT_TYPE_MAX:
							case ELEMENT_TYPE_MODIFIER:
							case ELEMENT_TYPE_END:
							case ELEMENT_TYPE_SENTINEL:
							{
								//nothing to do more
								break;
							}
							case ELEMENT_TYPE_VALUETYPE:
							case ELEMENT_TYPE_CLASS:
							case ELEMENT_TYPE_CMOD_REQD:
							case ELEMENT_TYPE_CMOD_OPT:
							{
								mdToken token;
								corSignature += CorSigUncompressToken(corSignature, &token);
								element->ClassToken = token;
								break;
							}
							case ELEMENT_TYPE_SZARRAY:
							case ELEMENT_TYPE_PINNED:
							case ELEMENT_TYPE_PTR:
							case ELEMENT_TYPE_BYREF:
							{
								element->ChildElement = ReadElement(&corSignature);
								break;
							}
							case ELEMENT_TYPE_GENERICINST:
							{
								//parse generic class
								element->ChildElement = ReadElement(&corSignature);
								//parse elements
								ULONG argumentsCount = CorSigUncompressData(corSignature);
								SignatureElement* current = null;
								for (ULONG i = 0; i < argumentsCount; i++)
								{
									SignatureElement* previous = current;
									current = ReadElement(&corSignature);
									if (element->ArgumentElement == null)
									{
										element->ArgumentElement = current;
									}
									else
									{
										previous->Next = current;
									}
								}
								break;
							}
							case ELEMENT_TYPE_ARRAY:
							{
								//TODO
								/*signature = ParseSignature(signature, metaDataImport);
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
								if (sizes[i] != 0)
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
								}*/
								break;
							}
						}
						*corSignatureRef = corSignature;
						return element;
					}
					__uint SignatureManager::GetRawSize(Signature* signature)
					{
						__uint size = sizeof(CorCallingConvention);
						size += GetRawSizeRecurcive(signature->Front);
						return size;
					}

					__uint SignatureManager::GetRawSizeRecurcive(SignatureElement* element)
					{
						__uint size = 0;
						if (element == null)
						{
							return size;
						}
						size += sizeof(CorElementType);
						if (element->ClassToken != 0)
						{
							size += sizeof(mdToken);
						}
						size += GetRawSizeRecurcive(element->ArgumentElement);
						size += GetRawSizeRecurcive(element->ChildElement);
						size += GetRawSizeRecurcive(element->Next);
						return size;
					}

					__uint SignatureManager::GetChainCount(SignatureElement* chain)
					{
						__uint count = 0;
						SignatureElement* current = chain;
						while (current != null)
						{
							count++;
							current = current->Next;
						}
						return count;
					}

					mdSignature SignatureManager::Write(Signature* signature, IMetaDataEmit* metadataEmit)
					{
						__uint rawSize = GetRawSize(signature);
						__byte* corSignatureData = new __byte[rawSize];
						__byte* corSignatureDataBegin = corSignatureData;

						corSignatureData += CorSigCompressData(signature->CallingConvention, corSignatureData);

						__uint actualElementsCount = GetChainCount(signature->Front);
						__uint elementsCount = actualElementsCount;
						if (signature->CallingConvention == CorCallingConvention::IMAGE_CEE_CS_CALLCONV_DEFAULT)
						{
							elementsCount--;
						}
						corSignatureData += CorSigCompressData(elementsCount, corSignatureData);

						SignatureElement* current = signature->Front;
						for (__uint i = 0; i < actualElementsCount; i++)
						{
							corSignatureData = WriteElement(current, corSignatureData);
							current = current->Next;
						}

						__uint realSize = corSignatureData - corSignatureDataBegin;

						__byte* temp = new __byte[realSize];
						memcpy(temp, corSignatureDataBegin, realSize);
						__FREEARR(corSignatureDataBegin);
						PCCOR_SIGNATURE corSignature = (PCCOR_SIGNATURE)temp;

						mdSignature corSignatureToken;
						HRESULT result = metadataEmit->GetTokenFromSig(corSignature, realSize, &corSignatureToken);
						return corSignatureToken;
					}

					__byte* SignatureManager::WriteElement(SignatureElement* element, __byte* corSignatureData)
					{
						corSignatureData += CorSigCompressElementType(element->ElementType, corSignatureData);
						switch (element->ElementType)
						{
							case ELEMENT_TYPE_VOID:
							case ELEMENT_TYPE_BOOLEAN:
							case ELEMENT_TYPE_CHAR:
							case ELEMENT_TYPE_I1:
							case ELEMENT_TYPE_U1:
							case ELEMENT_TYPE_I2:
							case ELEMENT_TYPE_U2:
							case ELEMENT_TYPE_I4:
							case ELEMENT_TYPE_U4:
							case ELEMENT_TYPE_I8:
							case ELEMENT_TYPE_U8:
							case ELEMENT_TYPE_R4:
							case ELEMENT_TYPE_R8:
							case ELEMENT_TYPE_STRING:
							case ELEMENT_TYPE_VAR:
							case ELEMENT_TYPE_MVAR:
							case ELEMENT_TYPE_TYPEDBYREF:
							case ELEMENT_TYPE_I:
							case ELEMENT_TYPE_U:
							case ELEMENT_TYPE_OBJECT:
							case ELEMENT_TYPE_FNPTR:
							case ELEMENT_TYPE_INTERNAL:
							case ELEMENT_TYPE_MAX:
							case ELEMENT_TYPE_MODIFIER:
							case ELEMENT_TYPE_END:
							case ELEMENT_TYPE_SENTINEL:
							{
								//nothing to do more
								break;
							}
							case ELEMENT_TYPE_VALUETYPE:
							case ELEMENT_TYPE_CLASS:
							case ELEMENT_TYPE_CMOD_REQD:
							case ELEMENT_TYPE_CMOD_OPT:
							{
								corSignatureData += CorSigCompressToken(element->ClassToken, corSignatureData);
								break;
							}
							case ELEMENT_TYPE_SZARRAY:
							case ELEMENT_TYPE_PINNED:
							case ELEMENT_TYPE_PTR:
							case ELEMENT_TYPE_BYREF:
							{
								corSignatureData = WriteElement(element->ChildElement, corSignatureData);
								break;
							}
							case ELEMENT_TYPE_GENERICINST:
							{
								//parse generic class
								corSignatureData = WriteElement(element->ChildElement, corSignatureData);
								//parse elements
								__uint argumentsCount = GetChainCount(element->ArgumentElement);
								corSignatureData += CorSigCompressData(argumentsCount, corSignatureData);
								SignatureElement* current = element->ArgumentElement;
								for (ULONG i = 0; i < argumentsCount; i++)
								{
									corSignatureData = WriteElement(current, corSignatureData);
									current = current->Next;
								}
								break;
							}
							case ELEMENT_TYPE_ARRAY:
							{
								//TODO
								/*signature = ParseSignature(signature, metaDataImport);
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
								if (sizes[i] != 0)
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
								}*/
								break;
							}
						}
						return corSignatureData;
					}

					__uint SignatureManager::InsertElement(Signature* signature, SignatureElement* element)
					{
						__uint index = 0;
						if (signature->Front == null)
						{
							signature->Front = element;
							return index;
						}
						SignatureElement* final = signature->Front;
						while (final->Next != null)
						{
							index++;
							final = final->Next;
						}
						final->Next = element;
						index++;
						return index;
					}
				}
			}
		}
	}
}