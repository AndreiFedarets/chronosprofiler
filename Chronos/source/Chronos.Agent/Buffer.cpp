#include "stdafx.h"
#include "Chronos.Agent.h"

namespace Chronos
{
	namespace Agent
	{
		Buffer::Buffer()
		{
			Size = 0;
			Data = null;
		}

		Buffer::Buffer(__byte* data, __uint size)
		{
			Size = size;
			Data = data;
		}

		Buffer::Buffer(__uint size)
		{
			Size = size;
			Data = new __byte[size];
		}

		Buffer::~Buffer()
		{
			__FREEARR(Data);
		}

		Buffer* Buffer::Clone()
		{
			Buffer* buffer = new Buffer(Size);
			memcpy(buffer->Data, Data, Size);
			return buffer;
		}
	}
}