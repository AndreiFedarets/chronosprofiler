#include "stdafx.h"
#include "Chronos.Agent.h"

namespace Chronos
{
	namespace Agent
	{
		DynamicSettingBlock::DynamicSettingBlock(__int blockSize, __byte* block)
			: _buffer(new Buffer(block, blockSize))
		{
		}

		DynamicSettingBlock::~DynamicSettingBlock(void)
		{
			__FREEOBJ(_buffer);
		}
		
		__bool DynamicSettingBlock::AsBool()
		{
			if (sizeof(__bool) == 1)
			{
				__byte value =_buffer->Data[0];
				if (value == 0)
				{
					return false;
				}
				return true;
			}
			MemoryStream stream(_buffer->Data, _buffer->Size);
			return Marshaler::DemarshalBool(&stream);
		}

		__byte DynamicSettingBlock::AsByte()
		{
			return _buffer->Data[0];
		}

		__short DynamicSettingBlock::AsShort()
		{
			MemoryStream stream(_buffer->Data, _buffer->Size);
			return Marshaler::DemarshalShort(&stream);
		}

		__ushort DynamicSettingBlock::AsUShort()
		{
			MemoryStream stream(_buffer->Data, _buffer->Size);
			return Marshaler::DemarshalUShort(&stream);
		}

		__int DynamicSettingBlock::AsInt()
		{
			MemoryStream stream(_buffer->Data, _buffer->Size);
			return Marshaler::DemarshalInt(&stream);
		}

		__uint DynamicSettingBlock::AsUInt()
		{
			MemoryStream stream(_buffer->Data, _buffer->Size);
			return Marshaler::DemarshalUInt(&stream);
		}

		__long DynamicSettingBlock::AsLong()
		{
			MemoryStream stream(_buffer->Data, _buffer->Size);
			return Marshaler::DemarshalLong(&stream);
		}

		__ulong DynamicSettingBlock::AsULong()
		{
			MemoryStream stream(_buffer->Data, _buffer->Size);
			return Marshaler::DemarshalULong(&stream);
		}
		
		__size DynamicSettingBlock::AsSize()
		{
			MemoryStream stream(_buffer->Data, _buffer->Size);
			return Marshaler::DemarshalSize(&stream);
		}

		__string DynamicSettingBlock::AsString()
		{
			MemoryStream stream(_buffer->Data, _buffer->Size);
			return Marshaler::DemarshalString(&stream);
		}

		__guid DynamicSettingBlock::AsGuid()
		{
			MemoryStream stream(_buffer->Data, _buffer->Size);
			return Marshaler::DemarshalGuid(&stream);
		}

		IStreamReader* DynamicSettingBlock::OpenRead()
		{
			MemoryStream* stream = new MemoryStream(_buffer->Data, _buffer->Size);
			return stream;
		}
	}
}