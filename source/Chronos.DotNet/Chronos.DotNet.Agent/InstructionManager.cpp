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
						instruction = MoveToFront(instruction);
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
							ilCode += current->GetSize();
							current->Previous = previous;
							if (previous != null)
							{
								previous->Next = current;
							}
						}
						current = MoveToFront(current);
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

					Instruction* InstructionManager::Create(OpCode* opCode)
					{
						__ulong dummy = 0;
						return Create(opCode, (__byte*)&dummy);
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

					/*__uint InstructionManager::GetSize(Instruction* instruction)
					{
						__uint size = instruction->OpCode->TokenSize + instruction->ValueSize;
						return instruction->;
					}*/

					__uint InstructionManager::WriteTo(Instruction* instruction, __byte* data)
					{
						OpCode* opCode = instruction->OpCode;
						if (opCode->TokenSize == 1)
						{
							memcpy(data, (__byte*)&opCode->TokenData[0], sizeof(__byte));
							data += sizeof(__byte);
						}
						else
						{
							memcpy(data, (__byte*)&opCode->TokenData[1], sizeof(__byte));
							data += sizeof(__byte);
							memcpy(data, (__byte*)&opCode->TokenData[0], sizeof(__byte));
							data += sizeof(__byte);
						}
						memcpy(data, (__byte*)&instruction->ArrayValue, instruction->ValueSize);
						return instruction->GetSize();
					}

					__uint InstructionManager::WriteChainTo(Instruction* instruction, __byte* data)
					{
						__uint chainSize = 0;
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
							offset += instruction->GetSize();
						}
						return offset;
					}

					Instruction* InstructionManager::ByOffset(Instruction* chain, __uint offset)
					{
						Instruction* target = null;
						Instruction* current = MoveToFront(chain);
						while (current != null)
						{
							if (offset == 0)
							{
								target = current;
								break;
							}
							offset -= current->GetSize();
							current = current->Next;
						}
						return target;
					}

					Instruction* InstructionManager::ByOffset(Instruction* chain, __uint offset, __uint length)
					{
						offset += length;
						Instruction* target = null;
						Instruction* current = MoveToFront(chain);
						while (current != null)
						{
							offset -= current->GetSize();
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
							size += instructionFrom->GetSize();
							instructionFrom = instructionFrom->Next;
						}
						if (instructionFrom != null)
						{
							size += instructionFrom->GetSize();
						}
						return size;
					}

					__uint InstructionManager::GetChainSize(Instruction* instruction)
					{
						instruction = MoveToFront(instruction);
						__uint size = 0;
						while (instruction != null)
						{
							size += instruction->GetSize();
							instruction = instruction->Next;
						}
						return size;
					}

					Instruction* InstructionManager::MoveToFront(Instruction* instruction)
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

					Instruction* InstructionManager::MoveToFinal(Instruction* instruction)
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
						Instruction* chainFront = InstructionManager::MoveToFront(chain);
						Instruction* chainFinal = InstructionManager::MoveToFinal(chain);

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

						return chainFront;
					}

					Instruction* InstructionManager::InsertChainAfter(Instruction* instruction, Instruction* chain)
					{
						Instruction* chainFront = InstructionManager::MoveToFront(chain);
						Instruction* chainFinal = InstructionManager::MoveToFinal(chain);

						Instruction* instructionAfter = instruction;
						Instruction* instructionBefore = instruction->Next;

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

						return chainFinal;
					}

					Instruction* InstructionManager::LookForward(Instruction* startFrom, OpCode* targetOpCode)
					{
						Instruction* current = startFrom;
						while (current != null && current->OpCode != targetOpCode)
						{
							current = current->Next;
						}
						return current;
					}

					Instruction* InstructionManager::LookBackward(Instruction* startFrom, OpCode* targetOpCode)
					{
						Instruction* current = startFrom;
						while (current != null && current->OpCode != targetOpCode)
						{
							current = current->Previous;
						}
						return current;
					}
				}
			}
		}
	}
}