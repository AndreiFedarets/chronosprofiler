#include "stdafx.h"
#include "Chronos.Agent.h"

namespace Chronos
{
	namespace Agent
	{
		MemoryStream::MemoryStream(__byte* data, __uint dataSize)
		{
			__byte* copy = new __byte[dataSize];
			memcpy(copy, data, dataSize);
			Init(copy, dataSize, dataSize);
		}
		
		MemoryStream::MemoryStream()
		{
			Init(new __byte[PageDefaultSize], 0, PageDefaultSize);
		}

		MemoryStream::~MemoryStream()
		{
			if (_buffer != null)
			{
				_dataSize = 0;
				_bufferSize = 0;
				__FREEARR(_buffer);
			}
		}

		__uint MemoryStream::Write(void* data, __uint size)
		{
			if (_position + size > _bufferSize)
			{
				Resize(size);
			}
			memcpy(_buffer + _position, data, size);
			__int overlapped = _position + size - _dataSize;
			if (overlapped > 0)
			{
				_dataSize += overlapped;
			}
			_position += size;
			return size;
		}

		__uint MemoryStream::Read(void* data, __uint size)
		{
			__uint actualReadSize;
			if (_position + size > _dataSize)
			{
				actualReadSize = _dataSize - _position;
			}
			else
			{
				actualReadSize = size;
			}
			memcpy(data, _buffer + _position, actualReadSize);
			_position += actualReadSize;
			return actualReadSize;
		}

		void MemoryStream::Skip(__uint count)
		{
			_position += count;
		}
		
		void MemoryStream::Seek(__uint position)
		{
			if (position > _dataSize)
			{
				position = _dataSize;
			}
			_position = position;
		}

		__bool MemoryStream::End()
		{
			return _position >= _dataSize;
		}

		Buffer* MemoryStream::ToArray()
		{
			Buffer* value = new Buffer(_dataSize);
			memcpy(value->Data, _buffer, value->Size);
			return value;
		}

		void MemoryStream::CopyTo(IStreamWriter* writer)
		{
			writer->Write(_buffer, _dataSize);
		}

		__uint MemoryStream::GetLength()
		{
			return _dataSize;
		}

		void MemoryStream::Init(__byte* buffer, __uint dataSize, __uint bufferSize)
		{
			_buffer = buffer;
			_bufferSize = bufferSize;
			_dataSize = dataSize;
			_position = 0;
			_bufferPageSize = PageDefaultSize;
		}

		void MemoryStream::Resize(__uint needSize)
		{
			//each resize we double page size
			_bufferPageSize += _bufferPageSize; 
			if (needSize > _bufferPageSize)
			{
				_bufferSize += needSize;
			}
			else
			{
				_bufferSize += _bufferPageSize;
			}
			__byte* buffer = new __byte[_bufferSize];
			memcpy(buffer, _buffer, _dataSize);
			__FREEARR(_buffer);
			_buffer = buffer;
		}

		__bool MemoryStream::Initialized()
		{
			return _buffer != null;
		}

		const __uint MemoryStream::PageDefaultSize = 1024; //1kb
	}
}