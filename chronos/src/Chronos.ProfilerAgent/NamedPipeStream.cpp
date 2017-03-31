#include "StdAfx.h"
#include "NamedPipeStream.h"


CNamedPipeStream::CNamedPipeStream(void)
{
	_pipeHandle = null;
}

CNamedPipeStream::~CNamedPipeStream()
{
	Dispose();
}

__uint CNamedPipeStream::Write(void* data, __uint size)
{
	DWORD bytesWritten;
	BOOL result = WriteFile(_pipeHandle, data, size, &bytesWritten, null);
	return bytesWritten;
}

__uint CNamedPipeStream::Read(void* data, __uint size)
{
	DWORD bytesRead;
	BOOL result = ReadFile(_pipeHandle, data, size, &bytesRead, null);
	return bytesRead;
}

void CNamedPipeStream::Skip(__uint count)
{
	BOOL result = ReadFile(_pipeHandle, 0, count, 0, null);
}

__bool CNamedPipeStream::End()
{
	return _pipeHandle != null;
}

__bool CNamedPipeStream::Initialized()
{
	return _pipeHandle != null && _pipeHandle != INVALID_HANDLE_VALUE;
}

void CNamedPipeStream::Dispose()
{
	if (_pipeHandle != null && _pipeHandle != INVALID_HANDLE_VALUE)
	{
		FlushFileBuffers(_pipeHandle);
		CloseHandle(_pipeHandle);
		_pipeHandle = null;
	}
}
