#include "stdafx.h"
#include "Chronos.DotNet.Agent.h"
#include <iostream>
#include <sstream>

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
					Method* MethodManager::Alloc()
					{
						Method* method = new Method();
						memset(method, 0, sizeof(Method));
						return method;
					}

					void MethodManager::Release(Method* method)
					{
						if (method == null)
						{
							return;
						}
						InstructionManager::ReleaseChain(method->FrontInstruction);
						ExceptionHandlerManager::ReleaseChain(method->FrontHandler);
						__FREEOBJ(method);
					}

					Method* MethodManager::Read(IMAGE_COR_ILMETHOD* ilMethod)
					{
						Method* method = null;
						COR_ILMETHOD_TINY* tinyMethod = (COR_ILMETHOD_TINY*)&ilMethod->Tiny;
						COR_ILMETHOD_FAT* fatMethod = (COR_ILMETHOD_FAT*)&ilMethod->Fat;
						if (tinyMethod->IsTiny())
						{
							method = ReadTiny(tinyMethod);
						}
						else
						{
							method = ReadFat(fatMethod);
						}
						return method;
					}

					Method* MethodManager::ReadTiny(COR_ILMETHOD_TINY* ilMethod)
					{
						Method* method = Alloc();
						method->FrontInstruction = InstructionManager::ReadChain(ilMethod->GetCode(), ilMethod->GetCodeSize());
						method->MaxStack = ilMethod->GetMaxStack();
						method->LocalVarSigTok = 0;
						return method;
					}

					Method* MethodManager::ReadFat(COR_ILMETHOD_FAT* ilMethod)
					{
						Method* method = Alloc();
						method->FrontInstruction = InstructionManager::ReadChain(ilMethod->GetCode(), ilMethod->GetCodeSize());
						method->MaxStack = ilMethod->GetMaxStack();
						method->LocalVarSigTok = ilMethod->GetLocalVarSigTok();
						const COR_ILMETHOD_SECT* ilSection = ilMethod->GetSect();
						while (ilSection != null)
						{
							switch (ilSection->Kind())
							{
								case CorILMethodSect::CorILMethod_Sect_EHTable:
									method->FrontHandler = ExceptionHandlerManager::ReadChain(ilSection, method->FrontInstruction);
									break;
								default:
									// we do not handle other types of section as at this moment CLR do not support anything except exceptions
									break;
							}
							ilSection = ilSection->Next();
						}
						return method;
					}

					__uint MethodManager::GetSize(Method* method)
					{
						//NOTE: at this moment we do not support building of Tiny method
						//... even if method matches Tiny requirements it will be converted to Fat
						//... it's fine to CLR
						__uint size = sizeof(COR_ILMETHOD_FAT);
						__uint codeSize = InstructionManager::GetChainSize(method->FrontInstruction);
						size += AlignCodeSize(codeSize);
						size += ExceptionHandlerManager::GetChainSize(method->FrontHandler);
						return size;
					}

					__uint MethodManager::AlignCodeSize(__uint size)
					{
						//DWORD alignment
						const __uint alignment = 4;
						__uint rest = size % alignment;
						if (rest != 0)
						{
							size += alignment - rest;
						}
						return size;
					}

					void MethodManager::WriteTo(Method* method, BYTE* methodData)
					{
						//NOTE: at this moment we do not support building of Tiny method
						//... even if method matches Tiny requirements it will be converted to Fat
						//... it's fine to CLR
						IMAGE_COR_ILMETHOD* ilMethod = (IMAGE_COR_ILMETHOD*)methodData;
						COR_ILMETHOD_FAT* ilFatMethod = (COR_ILMETHOD_FAT*)&ilMethod->Fat;

						unsigned int flags = 0;

						//set flags
						flags = flags | CorILMethodFlags::CorILMethod_FatFormat;
						if (method->LocalVarSigTok != 0)
						{
							flags = flags | CorILMethodFlags::CorILMethod_InitLocals;
						}
						if (method->FrontHandler != null)
						{
							flags = flags | CorILMethodFlags::CorILMethod_MoreSects;
						}
						ilFatMethod->SetFlags(flags);

						//set Size
						unsigned int headerSize = sizeof(COR_ILMETHOD_FAT) / 4;
						ilFatMethod->SetSize(headerSize);

						//set MaxStack
						ilFatMethod->SetMaxStack(method->MaxStack);

						__uint codeSize = InstructionManager::GetChainSize(method->FrontInstruction);
						//set CodeSize
						ilFatMethod->SetCodeSize(codeSize);

						//set LocalVarSigTok
						ilFatMethod->SetLocalVarSigTok(method->LocalVarSigTok);

						//copy Code
						__byte* targetCode = ilFatMethod->GetCode();
						InstructionManager::WriteChainTo(method->FrontInstruction, targetCode);

						//set ExceptionSection
						__byte* sectionData = (__byte*)ilFatMethod->GetSect();
						ExceptionHandlerManager::WriteChainTo(method->FrontHandler, sectionData);
					}

					Instruction* MethodManager::InsertChainBefore(Method* method, Instruction* instruction, Instruction* chain)
					{
						Instruction* chainFront = InstructionManager::InsertChainBefore(instruction, chain);
						method->FrontInstruction = InstructionManager::MoveToFront(chainFront);
						return chainFront;
					}

					Instruction* MethodManager::InsertChainAfter(Method* method, Instruction* instruction, Instruction* chain)
					{
						Instruction* chainFinal = InstructionManager::InsertChainAfter(instruction, chain);
						return chainFinal;
					}

					void MethodManager::WriteDebug(Method* method)
					{
						__byte level = 0;
						Instruction* instruction = method->FrontInstruction;
						while (instruction != null)
						{
							std::ostringstream stream; 
							ExceptionHandler* handler = method->FrontHandler;
							while (handler != null)
							{
								if (handler->TryBegin == instruction)
								{
									for (__byte i = 0; i < level; i++)
									{
										stream << "    ";
									}
									stream << "try {\r\n";
									level++;
								}
								if (handler->HandlerBegin == instruction)
								{
									if (handler->Flags == CorExceptionFlag::COR_ILEXCEPTION_CLAUSE_FINALLY)
									{
										for (__byte i = 0; i < level; i++)
										{
											stream << "    ";
										}
										stream << "finally {\r\n";
									}
									else if (handler->Flags == CorExceptionFlag::COR_ILEXCEPTION_CLAUSE_NONE)
									{
										for (__byte i = 0; i < level; i++)
										{
											stream << "    ";
										}
										stream << "catch( ";
										stream << Converter::ConvertStringToStringA(Converter::ConvertIntToString(handler->ClassToken)).c_str();
										stream << ") {\r\n";
									}
									level++;
								}
								handler = handler->Next;
							}
							for (__byte i = 0; i < level; i++)
							{
								stream << "    ";
							}
							stream << instruction->OpCode->Name;
							stream << " ";
							switch (instruction->ValueSize)
							{
								case 0:
									break;
								case sizeof(__byte) :
									stream << Converter::ConvertStringToStringA(Converter::ConvertIntToString(instruction->ByteValue)).c_str();
									break;
								case sizeof(__ushort) :
									stream << Converter::ConvertStringToStringA(Converter::ConvertIntToString(instruction->ShortValue)).c_str();
									break;
								case sizeof(__uint) :
									stream << Converter::ConvertStringToStringA(Converter::ConvertIntToString(instruction->IntValue)).c_str();
									break;
								case sizeof(__long) :
									stream << Converter::ConvertStringToStringA(Converter::ConvertLongToString(instruction->LongValue)).c_str();
									break;
								default:
									stream << "[Array]";
									break;
							}
							stream << "\r\n";
							handler = method->FrontHandler;
							while (handler != null)
							{
								if (handler->TryEnd == instruction || handler->HandlerEnd == instruction)
								{
									level--;
									for (__byte i = 0; i < level; i++)
									{
										stream << "    ";
									}
									stream << "}\r\n";
								}
								handler = handler->Next;
							}
							OutputDebugStringA(stream.str().c_str());
							instruction = instruction->Next;
						}

						std::ostringstream stream;
						stream << "-----------------------------------------------------------------------\r\n";
						OutputDebugStringA(stream.str().c_str());
					}
				}
			}
		}
	}
}