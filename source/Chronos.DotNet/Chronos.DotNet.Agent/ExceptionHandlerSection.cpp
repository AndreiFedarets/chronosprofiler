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
					ExceptionHandlerSection::~ExceptionHandlerSection()
					{
						ExceptionHandlerSectionClause* current = FirstClause;
						FirstClause = null;
						while (current != null)
						{
							ExceptionHandlerSectionClause* next = current->Next;
							__FREEOBJ(current);
							current = next;
						}
					}

					__uint ExceptionHandlerSection::GetClausesCount()
					{
						__uint count = 0;
						ExceptionHandlerSectionClause* current = FirstClause;
						while (current != null)
						{
							count++;
							current = current->Next;
						}
						return count;
					}

					__uint ExceptionHandlerSection::GetFullSize()
					{
						__uint count = GetClausesCount();
						return COR_ILMETHOD_SECT_EH_FAT::Size(count);
					}

					void ExceptionHandlerSection::Copy(BYTE* sectionData)
					{
						if (FirstClause == null || sectionData == null)
						{
							return;
						}
						//NOTE: at this moment we do not support building of Small sections
						//... even if section matches Small requirements it will be converted to Fat
						//... it's fine to CLR
						COR_ILMETHOD_SECT_EH_FAT* ilSection = (COR_ILMETHOD_SECT_EH_FAT*)sectionData;
						//set Section Kind
						ilSection->Kind = CorILMethodSect::CorILMethod_Sect_EHTable | CorILMethodSect::CorILMethod_Sect_FatFormat;

						//set Clauses
						sectionData = (BYTE*)&ilSection->Clauses;
						__uint clausesCount = 0;
						ExceptionHandlerSectionClause* clause = FirstClause;
						while (clause != null)
						{
							clausesCount++;
							clause->Copy(sectionData);
							clause = clause->Next;
							sectionData = sectionData + sizeof(IMAGE_COR_ILMETHOD_SECT_EH_CLAUSE_FAT);
						}

						//set DataSize
						__uint dataSize = COR_ILMETHOD_SECT_EH_FAT::Size(clausesCount);
						ilSection->SetDataSize(dataSize);
					}

					/*void ExceptionHandlerSection::AddFinally(__uint tryOffset, __uint tryLength, __uint handlerOffset, __uint handlerLength)
					{
						ExceptionHandlerSectionClause* clause = new ExceptionHandlerSectionClause();
						clause->Flags = CorExceptionFlag::COR_ILEXCEPTION_CLAUSE_FINALLY;
						clause->TryOffset = tryOffset;
						clause->TryLength = tryLength;
						clause->HandlerOffset = handlerOffset;
						clause->HandlerLength = handlerLength;
						Clauses->push_back(clause);
					}*/

					void ExceptionHandlerSection::LoadSmallSection(COR_ILMETHOD_SECT_EH_SMALL* section)
					{
						__int count = section->DataSize / sizeof(COR_ILMETHOD_SECT_EH_CLAUSE_SMALL);
						ExceptionHandlerSectionClause* current = null;
						for (__int i = 0; i < count; i++)
						{
							IMAGE_COR_ILMETHOD_SECT_EH_CLAUSE_SMALL ilClause = section->Clauses[i];
							ExceptionHandlerSectionClause* previous = current;
							current = new ExceptionHandlerSectionClause(ilClause);
							if (FirstClause == null)
							{
								FirstClause = current;
							}
							else
							{
								previous->Next = current;
							}
						}
					}

					void ExceptionHandlerSection::LoadFatSection(COR_ILMETHOD_SECT_EH_FAT* section)
					{
						__int count = section->DataSize / sizeof(COR_ILMETHOD_SECT_EH_CLAUSE_FAT);
						ExceptionHandlerSectionClause* current = null;
						for (__int i = 0; i < count; i++)
						{
							IMAGE_COR_ILMETHOD_SECT_EH_CLAUSE_FAT ilClause = section->Clauses[i];
							ExceptionHandlerSectionClause* previous = current;
							current = new ExceptionHandlerSectionClause(ilClause);
							if (FirstClause == null)
							{
								FirstClause = current;
							}
							else
							{
								previous->Next = current;
							}
						}
					}
				}
			}
		}
	}
}