#include "StdAfx.h"
#include "MemoryStream.h"


CMemoryStream::CMemoryStream(__byte* data, __uint dataSize)
{
	Init(data, dataSize, dataSize, 4096);
}

CMemoryStream::CMemoryStream()
{
	Init(new __byte[4096], 0, 4096, 4096);
}

CMemoryStream::~CMemoryStream()
{
	Dispose();
}

__uint CMemoryStream::Write(void* data, __uint size)
{
	while (_position + size > _bufferSize)
	{
		Resize();
	}
	memcpy(_buffer + _position, data, size);
	_position += size;
	_dataSize += size;
	return 0;
}

__uint CMemoryStream::Read(void* data, __uint size)
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

void CMemoryStream::Skip(__uint count)
{
	_position += count;
}

__bool CMemoryStream::End()
{
	return _position >= _dataSize;
}

__byte* CMemoryStream::ToArray()
{
	return _buffer;
}

__uint CMemoryStream::GetLength()
{
	return _dataSize;
}

void CMemoryStream::Dispose()
{
	if (_buffer != null)
	{
		_dataSize = 0;
		_bufferSize = 0;
		__FREEARR(_buffer);
	}
}

void CMemoryStream::Init(__byte* buffer, __uint dataSize, __uint bufferSize, __uint bufferPageSize)
{
	_buffer = buffer;
	_bufferSize = bufferSize;
	_dataSize = dataSize;
	_bufferPageSize = bufferPageSize;
	_position = 0;
}

void CMemoryStream::Resize()
{
	_bufferSize += _bufferPageSize;
	__byte* buffer = new __byte[_bufferSize];
	memcpy(buffer, _buffer, _dataSize);
	__FREEARR(_buffer);
	_buffer = buffer;
}
