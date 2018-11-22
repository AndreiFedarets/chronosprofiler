#include <memory>
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
					Method::Method(IMAGE_COR_ILMETHOD* ilMethod)
					{
						ExceptionSection = new ExceptionSection();
						COR_ILMETHOD_TINY* tinyMethod = (COR_ILMETHOD_TINY*)&ilMethod->Tiny;
						COR_ILMETHOD_FAT* fatMethod = (COR_ILMETHOD_FAT*)&ilMethod->Fat;
						if (tinyMethod->IsTiny())
						{
							LoadTinyMethod(tinyMethod);
						}
						else
						{
							LoadFatMethod(fatMethod);
						}
					}

					Method::~Method()
					{
						InstructionManager::ReleaseChain(FrontInstruction);
						FrontInstruction = null;
						__FREEOBJ(ExceptionSection);
					}

					__uint Method::GetFullSize()
					{
					}

					__uint Method::AlignSize(__uint size)
					{
					}

					void Method::LoadFatMethod(COR_ILMETHOD_FAT* method)
					{
					}

					void Method::Copy(BYTE* methodData)
					{
						//NOTE: at this moment we do not support building of Tiny method
						//... even if method matches Tiny requirements it will be converted to Fat
						//... it's fine to CLR
						IMAGE_COR_ILMETHOD* ilMethod = (IMAGE_COR_ILMETHOD*)methodData;
						COR_ILMETHOD_FAT* targetMethod = (COR_ILMETHOD_FAT*)&ilMethod->Fat;

						unsigned int flags = 0;

						//set flags
						flags = flags | CorILMethodFlags::CorILMethod_FatFormat;
						if (LocalVarSigTok != 0)
						{
							flags = flags | CorILMethodFlags::CorILMethod_InitLocals;
						}
						if (ExceptionSection->FirstClause != null)
						{
							flags = flags | CorILMethodFlags::CorILMethod_MoreSects;
						}
						targetMethod->SetFlags(flags);

						//set Size
						unsigned int headerSize = sizeof(COR_ILMETHOD_FAT) / 4;
						targetMethod->SetSize(headerSize);

						//set MaxStack
						targetMethod->SetMaxStack(MaxStack);

						__uint codeSize = InstructionManager::GetChainSize(FrontInstruction);
						//set CodeSize
						targetMethod->SetCodeSize(codeSize);

						//set LocalVarSigTok
						targetMethod->SetLocalVarSigTok(LocalVarSigTok);

						//copy Code
						BYTE* targetCode = targetMethod->GetCode();
						InstructionManager::WriteChainTo(FrontInstruction, targetCode);

						//set ExceptionSection
						methodData = (BYTE*)targetMethod->GetSect();
						ExceptionSection->Copy(methodData);
						methodData += ExceptionSection->GetFullSize();
					}

					void Method::InsertBefore(Instruction* instruction, Instruction* chain)
					{
						Instruction* chainFront = InstructionManager::MoveToFront(chain);
						Instruction* chainFinal = InstructionManager::MoveToFinal(chain);
						FrontInstruction = InstructionManager::InsertBefore(instruction, chain);
						//__uint chainFrontOffset = InstructionManager::GetOffset(chainFront);
						//__uint chainFinalOffset = InstructionManager::GetOffset(chainFinal);
						////update exception sections
						//MethodExceptionSectionClause* clause = ExceptionSection->FirstClause;
						//while (clause != null)
						//{
						//	if (clause->TryOffset)
						//}
					}

					//void Method::ResizeCode(__uint progologSize, __uint epilogSize)
					//{
					//	__uint newSize = CodeSize + progologSize + epilogSize;
					//	__byte* newCode = new __byte[newSize];
					//	memcpy((newCode + progologSize), Code, CodeSize);
					//	__FREEARR(Code);
					//	Code = newCode;
					//	CodeSize = newSize;
					//}

					//void Method::InsertBegin(__byte* code, __uint codeSize)
					//{
					//	//resize Code
					//	ResizeCode(codeSize, 0);

					//	//insert code
					//	memcpy(Code, code, codeSize);

					//	//update offsets
					//	for (__vector<MethodExceptionSection*>::iterator i = Sections->begin(); i != Sections->end(); ++i)
					//	{
					//		MethodExceptionSection* section = *i;
					//		__vector<MethodExceptionSectionClause*>* clauses = section->Clauses;
					//		for (__vector<MethodExceptionSectionClause*>::iterator j = clauses->begin(); j != clauses->end(); ++j)
					//		{
					//			MethodExceptionSectionClause* clause = *j;
					//			clause->HandlerOffset += codeSize;
					//			clause->TryOffset += codeSize;
					//			if ((clause->Flags & CorExceptionFlag::COR_ILEXCEPTION_CLAUSE_FILTER) == CorExceptionFlag::COR_ILEXCEPTION_CLAUSE_FILTER)
					//			{
					//				clause->FilterOffset += codeSize;
					//			}
					//		}
					//	}
					//}

					//void Method::InsertEnd(__byte* code, __uint codeSize)
					//{
					//	__uint oldSize = CodeSize;
					//	__byte lastOpCode = GetLastOpCode();
					//	ResizeCode(0, codeSize);
					//	if (lastOpCode == OpCodes::Ret->Token || lastOpCode == OpCodes::Throw->Token)
					//	{
					//		__byte* offset = Code + oldSize - 1;
					//		memcpy(offset, code, codeSize);
					//		*(Code + CodeSize - 1) = lastOpCode;
					//	}
					//	else
					//	{
					//		__byte* offset = Code + oldSize;
					//		memcpy(offset, code, codeSize);
					//	}
					//}
				}
			}
		}
	}
}