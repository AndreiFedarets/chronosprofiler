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
					ExceptionHandler* ExceptionHandlerManager::Alloc()
					{
						ExceptionHandler* handler = new ExceptionHandler();
						memset(handler, 0, sizeof(ExceptionHandler));
						return handler;
					}

					void ExceptionHandlerManager::Release(ExceptionHandler* handler)
					{
						if (handler == null)
						{
							return;
						}
						__FREEOBJ(handler);
					}

					void ExceptionHandlerManager::ReleaseChain(ExceptionHandler* chain)
					{
						ExceptionHandler* current = chain;
						while (current != null)
						{
							ExceptionHandler* next = current->Next;
							__FREEOBJ(current);
							current = next;
						}
					}

					ExceptionHandler* ExceptionHandlerManager::ReadChain(const COR_ILMETHOD_SECT* ilSection, Instruction* instructionChain)
					{
						if (ilSection->IsFat())
						{
							COR_ILMETHOD_SECT_EH_FAT* fatSection = (COR_ILMETHOD_SECT_EH_FAT*)&ilSection->Fat;
							return ReadChainFat(fatSection, instructionChain);
						}
						else
						{
							COR_ILMETHOD_SECT_EH_SMALL* smallSection = (COR_ILMETHOD_SECT_EH_SMALL*)&ilSection->Small;
							return ReadChainSmall(smallSection, instructionChain);
						}
					}

					ExceptionHandler* ExceptionHandlerManager::ReadChainSmall(COR_ILMETHOD_SECT_EH_SMALL* ilSection, Instruction* chain)
					{
						__int count = ilSection->DataSize / sizeof(IMAGE_COR_ILMETHOD_SECT_EH_CLAUSE_SMALL);
						ExceptionHandler* front = null;
						ExceptionHandler* current = null;
						for (__int i = 0; i < count; i++)
						{
							IMAGE_COR_ILMETHOD_SECT_EH_CLAUSE_SMALL ilClause = ilSection->Clauses[i];
							ExceptionHandler* previous = current;
							current = ReadSmall(&ilClause, chain);
							if (front == null)
							{
								front = current;
							}
							else
							{
								previous->Next = current;
							}
						}
						return front;
					}

					ExceptionHandler* ExceptionHandlerManager::ReadChainFat(COR_ILMETHOD_SECT_EH_FAT* ilSection, Instruction* chain)
					{
						__int count = ilSection->DataSize / sizeof(IMAGE_COR_ILMETHOD_SECT_EH_CLAUSE_FAT);
						ExceptionHandler* front = null;
						ExceptionHandler* current = null;
						for (__int i = 0; i < count; i++)
						{
							IMAGE_COR_ILMETHOD_SECT_EH_CLAUSE_FAT ilClause = ilSection->Clauses[i];
							ExceptionHandler* previous = current;
							current = ReadFat(&ilClause, chain);
							if (front == null)
							{
								front = current;
							}
							else
							{
								previous->Next = current;
							}
						}
						return front;
					}

					ExceptionHandler* ExceptionHandlerManager::ReadSmall(IMAGE_COR_ILMETHOD_SECT_EH_CLAUSE_SMALL* ilClause, Instruction* instructionChain)
					{
						ExceptionHandler* handler = Alloc();
						handler->Flags = (CorExceptionFlag)ilClause->Flags;
						handler->ClassToken = ilClause->ClassToken;
						handler->TryBegin = InstructionManager::ByOffset(instructionChain, ilClause->TryOffset);
						handler->TryEnd = InstructionManager::ByOffset(instructionChain, ilClause->TryOffset, ilClause->TryLength);
						handler->HandlerBegin = InstructionManager::ByOffset(instructionChain, ilClause->HandlerOffset);
						handler->HandlerEnd = InstructionManager::ByOffset(instructionChain, ilClause->HandlerOffset, ilClause->HandlerLength);
						if (handler->Flags == CorExceptionFlag::COR_ILEXCEPTION_CLAUSE_FILTER)
						{
							handler->Filter = InstructionManager::ByOffset(instructionChain, ilClause->FilterOffset);
						}
						return handler;
					}

					ExceptionHandler* ExceptionHandlerManager::ReadFat(IMAGE_COR_ILMETHOD_SECT_EH_CLAUSE_FAT* ilClause, Instruction* instructionChain)
					{
						ExceptionHandler* handler = Alloc();
						handler->Flags = (CorExceptionFlag)ilClause->Flags;
						handler->ClassToken = ilClause->ClassToken;
						handler->TryBegin = InstructionManager::ByOffset(instructionChain, ilClause->TryOffset);
						handler->TryEnd = InstructionManager::ByOffset(instructionChain, ilClause->TryOffset, ilClause->TryLength);
						handler->HandlerBegin = InstructionManager::ByOffset(instructionChain, ilClause->HandlerOffset);
						handler->HandlerEnd = InstructionManager::ByOffset(instructionChain, ilClause->HandlerOffset, ilClause->HandlerLength);
						if (handler->Flags == CorExceptionFlag::COR_ILEXCEPTION_CLAUSE_FILTER)
						{
							handler->Filter = InstructionManager::ByOffset(instructionChain, ilClause->FilterOffset);
						}
						return handler;
					}

					__uint ExceptionHandlerManager::GetChainSize(ExceptionHandler* chain)
					{
						__uint count = GetChainCount(chain);
						return COR_ILMETHOD_SECT_EH_FAT::Size(count);
					}

					__uint ExceptionHandlerManager::GetChainCount(ExceptionHandler* chain)
					{
						__uint count = 0;
						while (chain->Next != null)
						{
							count++;
							chain = chain->Next;
						}
						return count;
					}

					__uint ExceptionHandlerManager::WriteTo(ExceptionHandler* handler, __byte* ilClauseData)
					{
						IMAGE_COR_ILMETHOD_SECT_EH_CLAUSE_FAT* ilClause = (IMAGE_COR_ILMETHOD_SECT_EH_CLAUSE_FAT*)ilClauseData;
						ilClause->Flags = handler->Flags;
						ilClause->TryOffset = InstructionManager::GetOffset(handler->TryBegin);
						ilClause->TryLength = InstructionManager::GetRangeSize(handler->TryBegin, handler->TryEnd);
						ilClause->HandlerOffset = InstructionManager::GetOffset(handler->HandlerBegin);
						ilClause->HandlerLength = InstructionManager::GetRangeSize(handler->HandlerBegin, handler->HandlerEnd);
						ilClause->ClassToken = handler->ClassToken;
						if (ilClause->Flags == CorExceptionFlag::COR_ILEXCEPTION_CLAUSE_FILTER)
						{
							ilClause->FilterOffset = InstructionManager::GetOffset(handler->Filter);
						}
						return sizeof(IMAGE_COR_ILMETHOD_SECT_EH_CLAUSE_FAT);
					}

					void ExceptionHandlerManager::WriteChainTo(ExceptionHandler* chain, __byte* ilSectionData)
					{
						if (chain == null)
						{
							return;
						}
						//NOTE: at this moment we do not support building of Small sections
						//... even if section matches Small requirements it will be converted to Fat
						//... it's fine to CLR
						COR_ILMETHOD_SECT_EH_FAT* ilSection = (COR_ILMETHOD_SECT_EH_FAT*)ilSectionData;

						//set Section Kind
						ilSection->Kind = CorILMethodSect::CorILMethod_Sect_EHTable | CorILMethodSect::CorILMethod_Sect_FatFormat;

						//set Clauses
						__uint clausesCount = 0;
						__byte* ilClausesData = (BYTE*)&ilSection->Clauses;
						ExceptionHandler* current = chain;
						while (current != null)
						{
							ilClausesData += WriteTo(current, ilClausesData);
							clausesCount++;
							current = current->Next;
						}

						//set DataSize
						__uint dataSize = COR_ILMETHOD_SECT_EH_FAT::Size(clausesCount);
						ilSection->SetDataSize(dataSize);
					}
				}
			}
		}
	}
}