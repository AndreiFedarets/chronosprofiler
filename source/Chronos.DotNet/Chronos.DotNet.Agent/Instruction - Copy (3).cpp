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
					Instruction::Instruction()
					{
						memset(this, 0, sizeof(Instruction));
					}

					__uint Instruction::GetOffset(Instruction* instruction)
					{
						__uint offset = 0;
						while (instruction->Previous != null)
						{
							__uint instructionSize = instruction->OpCode->TokenSize + instruction->;
						}
						return offset;
					}

					__uint Instruction::GetSize()
					{
						__uint size = OpCode->TokenSize + GetPayloadSize();
						return size;
					}
				}
			}
		}
	}
}