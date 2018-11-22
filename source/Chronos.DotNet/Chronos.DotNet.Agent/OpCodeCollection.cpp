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
					OpCodeCollection::OpCodeCollection()
					{
						_items = new __map<__ushort, OpCode*>();
						//Nop
						_items->insert(std::pair<__ushort, OpCode*>(0x00, new OpCode((__byte)0x00))); 
						//
					}

					OpCodeCollection::OpCodeCollection()
					{
						for (__map<__ushort, OpCode*>::iterator i = _items->begin(); i != _items->end(); ++i)
						{
							OpCode* opCode = i->second;
							__FREEOBJ(opCode);
						}
						__FREEOBJ(_items);
					}
				}
			}
		}
	}
}