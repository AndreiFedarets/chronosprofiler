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
					Instruction* InstructionManager::Alloc()
					{
						return Alloc(0);
					}

					Instruction* InstructionManager::Alloc(__uint valueSize)
					{
						__uint size = sizeof(Instruction);
						if (valueSize > sizeof(__ulong))
						{
							size += valueSize - sizeof(__ulong);
						}
						__byte* data = new __byte[size];
						memset(data, 0, size);
						Instruction* instruction =(Instruction*)data;
						instruction->ValueSize = valueSize;
						return instruction;
					}

					void InstructionManager::Release(Instruction* instruction)
					{
						if (instruction == null)
						{
							return;
						}
						__byte* data = (__byte*)instruction;
						__FREEARR(data);
					}

					void InstructionManager::ReleaseChain(Instruction* instruction)
					{
						instruction = GetChainFront(instruction);
						while (instruction != null)
						{
							Instruction* temp = instruction;
							instruction = instruction->Next;
							Release(temp);
						}
					}

					Instruction* InstructionManager::Read(__byte* ilCode)
					{
						OpCode* opCode = OpCodes::Read(ilCode);
						__ASSERT(opCode != null, L"Unknown OpCode");
						ilCode += opCode->TokenSize;
						Instruction* instruction = Create(opCode, ilCode);
						return instruction;
					}

					Instruction* InstructionManager::ReadChain(__byte* ilCode, __uint ilCodeSize)
					{
						if (ilCodeSize == 0)
						{
							return null;
						}
						Instruction* current = null;
						Instruction* previous = null;
						__byte* ilCodeEnd = ilCode + ilCodeSize;
						while (ilCode < ilCodeEnd)
						{
							previous = current;
							current = Read(ilCode);
							ilCode += GetSize(current);
							current->Previous = previous;
							if (previous != null)
							{
								previous->Next = current;
							}
						}
						current = GetChainFront(current);
						return current;
					}

					Instruction* InstructionManager::Create(OpCode* opCode, __byte* valueData)
					{
						Instruction* instruction = null;
						__uint valueSize = 0;
						if (opCode->GetValueSize != null)
						{
							valueSize = opCode->GetValueSize(valueData);
						}
						else
						{
							valueSize = opCode->ValueSize;
						}
						instruction = Alloc(valueSize);
						instruction->OpCode = opCode;
						memcpy(&instruction->ArrayValue, valueData, valueSize);
						return instruction;
					}

					Instruction* InstructionManager::Create(OpCode* opCode, __byte value)
					{
						return Create(opCode, (__byte*)&value);
					}

					Instruction* InstructionManager::Create(OpCode* opCode, __ushort value)
					{
						return Create(opCode, (__byte*)&value);
					}

					Instruction* InstructionManager::Create(OpCode* opCode, __uint value)
					{
						return Create(opCode, (__byte*)&value);
					}

					Instruction* InstructionManager::Create(OpCode* opCode, __ulong value)
					{
						return Create(opCode, (__byte*)&value);
					}

					__uint InstructionManager::GetSize(Instruction* instruction)
					{
						__uint size = instruction->OpCode->TokenSize + instruction->ValueSize;
						return size;
					}

					__uint InstructionManager::WriteTo(Instruction* instruction, __byte* data)
					{
						OpCode* opCode = instruction->OpCode;
						memcpy(data, (__byte*)&opCode->Token, opCode->TokenSize);
						data += opCode->TokenSize;
						memcpy(data, (__byte*)&instruction->ArrayValue, instruction->ValueSize);
						return GetSize(instruction);
					}

					__uint InstructionManager::WriteChainTo(Instruction* instruction, __byte* data)
					{
						__byte chainSize = 0;
						while (instruction != null)
						{
							chainSize += WriteTo(instruction, data + chainSize);
							instruction = instruction->Next;
						}
						return chainSize;
					}

					__uint InstructionManager::GetOffset(Instruction* instruction)
					{
						if (instruction == null)
						{
							return 0;
						}
						__uint offset = 0;
						while (instruction->Previous != null)
						{
							instruction = instruction->Previous;
							offset += GetSize(instruction);
						}
						return offset;
					}

					Instruction* InstructionManager::ByOffset(Instruction* chain, __uint offset)
					{
						Instruction* target = null;
						Instruction* current = GetChainFront(chain);
						while (current != null)
						{
							if (offset == 0)
							{
								target = current;
								break;
							}
							offset -= GetSize(current);
							current = current->Next;
						}
						return target;
					}

					Instruction* InstructionManager::ByOffset(Instruction* chain, __uint offset, __uint length)
					{
						offset += length;
						Instruction* target = null;
						Instruction* current = GetChainFront(chain);
						while (current != null)
						{
							offset -= GetSize(current);
							if (offset == 0)
							{
								target = current;
								break;
							}
							current = current->Next;
						}
						return target;
					}

					__uint InstructionManager::GetRangeSize(Instruction* instructionFrom, Instruction* instructionTo)
					{
						__uint size = 0;
						while (instructionFrom != instructionTo && instructionFrom != null)
						{
							size += GetSize(instructionFrom);
							instructionFrom = instructionFrom->Next;
						}
						if (instructionFrom != null)
						{
							size += GetSize(instructionFrom);
						}
						return size;
					}

					__uint InstructionManager::GetChainSize(Instruction* instruction)
					{
						instruction = GetChainFront(instruction);
						__uint size = 0;
						while (instruction != null)
						{
							size += GetSize(instruction);
							instruction = instruction->Next;
						}
						return size;
					}

					Instruction* InstructionManager::GetChainFront(Instruction* instruction)
					{
						if (instruction == null)
						{
							return null;
						}
						while (instruction->Previous != null)
						{
							instruction = instruction->Previous;
						}
						return instruction;
					}

					Instruction* InstructionManager::GetChainFinal(Instruction* instruction)
					{
						if (instruction == null)
						{
							return null;
						}
						while (instruction->Next != null)
						{
							instruction = instruction->Next;
						}
						return instruction;
					}

					Instruction* InstructionManager::InsertChainBefore(Instruction* instruction, Instruction* chain)
					{
						Instruction* chainFront = InstructionManager::GetChainFront(chain);
						Instruction* chainFinal = InstructionManager::GetChainFinal(chain);

						Instruction* instructionAfter = instruction->Previous;
						Instruction* instructionBefore = instruction;

						if (instructionAfter != null)
						{
							instructionAfter->Next = chainFront;
							chainFront->Previous = instructionAfter;
						}
						if (instructionBefore != null)
						{
							instructionBefore->Previous = chainFinal;
							chainFinal->Next = instructionBefore;
						}

						return GetChainFront(chainFront);
					}
				}
			}
		}
	}
}