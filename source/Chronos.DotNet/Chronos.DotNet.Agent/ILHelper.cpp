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
					unsigned long ILHelper::GetMethodFullSize(COR_ILMETHOD_TINY* method)
					{
						unsigned long size = 0;
						size += sizeof(COR_ILMETHOD_TINY); // header size
						size += method->GetCodeSize();     // code size
						return size;
					}

					unsigned long ILHelper::GetMethodFullSize(COR_ILMETHOD_FAT* method)
					{
						unsigned long size = 0;

						// header size (GetSize() returns count of 4-byte blocks)
						size += method->GetSize() * 4;

						// code size
						size += method->GetCodeSize();

						// sections size
						unsigned long sectionsSize = 0;
						GetMethodSectionsData(method, &sectionsSize);
						size += sectionsSize;

						return size;
					}

					IMAGE_COR_ILMETHOD* ILHelper::ConvertTinyMethodToFat(COR_ILMETHOD_TINY* sourceMethod, IMethodMalloc* methodMalloc)
					{
						__uint targetMethodSize = sizeof(COR_ILMETHOD_FAT) + sourceMethod->GetCodeSize();
						IMAGE_COR_ILMETHOD* targetMethod = (IMAGE_COR_ILMETHOD*)methodMalloc->Alloc(targetMethodSize);
						memset(targetMethod, 0, targetMethodSize);
						CopyTinyToFat(sourceMethod, (COR_ILMETHOD_FAT*)&targetMethod->Fat);
						return targetMethod;
					}

					IMAGE_COR_ILMETHOD* ILHelper::ConvertTinyMethodToFat(COR_ILMETHOD_TINY* sourceMethod)
					{
						__uint targetMethodSize = sizeof(COR_ILMETHOD_FAT) + sourceMethod->GetCodeSize();
						IMAGE_COR_ILMETHOD* targetMethod = (IMAGE_COR_ILMETHOD*)new BYTE[targetMethodSize];
						memset(targetMethod, 0, targetMethodSize);
						CopyTinyToFat(sourceMethod, (COR_ILMETHOD_FAT*)&targetMethod->Fat);
						return targetMethod;
					}

					void ILHelper::CopyTinyToFat(COR_ILMETHOD_TINY* sourceMethod, COR_ILMETHOD_FAT* targetMethod)
					{
						unsigned int flags = 0;

						//set FatFormat flag
						flags = flags | CorILMethod_FatFormat;
						targetMethod->SetFlags(flags);

						//set MaxStack
						targetMethod->SetMaxStack(sourceMethod->GetMaxStack());

						//set Size
						unsigned int headerSize = sizeof(COR_ILMETHOD_FAT) / 4;
						targetMethod->SetSize(headerSize);

						//copy Code
						BYTE* targetCode = targetMethod->GetCode();
						BYTE* sourceCode = sourceMethod->GetCode();
						unsigned int sourceCodeSize = sourceMethod->GetCodeSize();
						memcpy(targetCode, sourceCode, sourceCodeSize);

						//set CodeSize
						targetMethod->SetCodeSize(sourceCodeSize);
					}

					IMAGE_COR_ILMETHOD* ILHelper::CloneFatMethod(COR_ILMETHOD_FAT* sourceMethod)
					{
						__uint targetMethodSize = GetMethodFullSize(sourceMethod);
						IMAGE_COR_ILMETHOD* targetMethod = (IMAGE_COR_ILMETHOD*)new BYTE[targetMethodSize];
						memset(targetMethod, 0, targetMethodSize);
						CopyFatToFat(sourceMethod, (COR_ILMETHOD_FAT*)&targetMethod->Fat);
						return targetMethod;
					}

					IMAGE_COR_ILMETHOD* ILHelper::CloneFatMethod(COR_ILMETHOD_FAT* sourceMethod, IMethodMalloc* methodMalloc)
					{
						__uint targetMethodSize = GetMethodFullSize(sourceMethod);
						IMAGE_COR_ILMETHOD* targetMethod = (IMAGE_COR_ILMETHOD*)methodMalloc->Alloc(targetMethodSize);
						memset(targetMethod, 0, targetMethodSize);
						CopyFatToFat(sourceMethod, (COR_ILMETHOD_FAT*)&targetMethod->Fat);
						return targetMethod;
					}

					void ILHelper::CopyFatToFat(COR_ILMETHOD_FAT* sourceMethod, COR_ILMETHOD_FAT* targetMethod)
					{
						//set Flags
						targetMethod->SetFlags(sourceMethod->GetFlags());

						//set MaxStack
						targetMethod->SetMaxStack(sourceMethod->GetMaxStack());

						//set Size
						targetMethod->SetSize(sourceMethod->GetSize());

						//set LocalVarSigTok
						targetMethod->SetLocalVarSigTok(sourceMethod->GetLocalVarSigTok());

						//copy Code
						BYTE* targetCode = targetMethod->GetCode();
						BYTE* sourceCode = sourceMethod->GetCode();
						unsigned long sourceCodeSize = sourceMethod->GetCodeSize();
						memcpy(targetCode, sourceCode, sourceCodeSize);

						//set CodeSize
						targetMethod->SetCodeSize(sourceCodeSize);

						// copy sections
						unsigned long sectionsDataSize = 0;
						BYTE* sourceSectionsData = GetMethodSectionsData(sourceMethod, &sectionsDataSize);
						if (sourceSectionsData != null)
						{
							// we can query sections on target method becase: 
							//1. source method has sections
							//2. we copied flags
							BYTE* targetSectionsData = (BYTE*)targetMethod->GetSect();
							memcpy(targetSectionsData, sourceSectionsData, sectionsDataSize);
						}
					}

					BYTE* ILHelper::GetMethodSectionsData(COR_ILMETHOD_FAT* method, unsigned long* dataSize)
					{
						BYTE* sectionData = null;
						*dataSize = 0;
						const COR_ILMETHOD_SECT* firstSection = method->GetSect();
						if (firstSection != null)
						{
							sectionData = (BYTE*)firstSection;
							//find last section
							const COR_ILMETHOD_SECT* lastSection = firstSection;
							while (true)
							{
								const COR_ILMETHOD_SECT* temp = lastSection->Next();
								if (temp == null)
								{
									break;
								}
								lastSection = temp;
							}
							// move to the aligned end of lastSection
							lastSection = ((COR_ILMETHOD_SECT*)(((BYTE *)lastSection) + lastSection->DataSize()))->Align();
							__uint sectionsDelta = (BYTE*)lastSection - (BYTE*)firstSection;
							*dataSize = sectionsDelta;// +lastSection->DataSize();
						}
						return sectionData;
					}

					void ILHelper::UpdateEHSectionsOffset(COR_ILMETHOD_FAT* method, int offsetDelta)
					{
						const COR_ILMETHOD_SECT* section = method->GetSect();
						while (section)
						{
							if (section->Kind() != CorILMethod_Sect_EHTable)
							{
								continue;
							}
							COR_ILMETHOD_SECT_EH* handlingSection = (COR_ILMETHOD_SECT_EH *)section;
							if (handlingSection->IsFat())
							{
								COR_ILMETHOD_SECT_EH_FAT *fatHandlingSection = (COR_ILMETHOD_SECT_EH_FAT*)handlingSection;

								for (unsigned int i = 0; i < handlingSection->EHCount(); i++)
								{
									IMAGE_COR_ILMETHOD_SECT_EH_CLAUSE_FAT *fatHandlingClause = &fatHandlingSection->Clauses[i];

									if (fatHandlingClause->Flags & COR_ILEXCEPTION_CLAUSE_FILTER)
									{
										fatHandlingClause->FilterOffset += offsetDelta;
									}
									fatHandlingClause->HandlerOffset += offsetDelta;
									fatHandlingClause->TryOffset += offsetDelta;
								}
							}
							else
							{
								COR_ILMETHOD_SECT_EH_SMALL* smallHandlingSection = (COR_ILMETHOD_SECT_EH_SMALL*)section;

								for (unsigned int i = 0; i < handlingSection->EHCount(); i++)
								{
									IMAGE_COR_ILMETHOD_SECT_EH_CLAUSE_SMALL *smallHandlingClause = &smallHandlingSection->Clauses[i];
									if (smallHandlingClause->Flags & COR_ILEXCEPTION_CLAUSE_FILTER)
									{
										smallHandlingClause->FilterOffset += offsetDelta;
									}
									smallHandlingClause->TryOffset += offsetDelta;
									smallHandlingClause->HandlerOffset += offsetDelta;
								}
							}
							section = section->Next();
						}
					}
				}
			}
		}
	}
}