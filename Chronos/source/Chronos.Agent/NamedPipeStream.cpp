#include "stdafx.h"
#include "Chronos.Agent.h"

namespace Chronos
{
	namespace Agent
	{
		NamedPipeStream::NamedPipeStream(void)
		{
			_pipeHandle = null;
		}

		NamedPipeStream::~NamedPipeStream()
		{
			if (_pipeHandle != null && _pipeHandle != INVALID_HANDLE_VALUE)
			{
				FlushFileBuffers(_pipeHandle);
				CloseHandle(_pipeHandle);
				_pipeHandle = null;
			}
		}

		__uint NamedPipeStream::Write(void* data, __uint size)
		{
			DWORD bytesWritten;
			BOOL result = WriteFile(_pipeHandle, data, size, &bytesWritten, null);
			return bytesWritten;
		}

		__uint NamedPipeStream::Read(void* data, __uint size)
		{
			DWORD bytesRead;
			BOOL result = ReadFile(_pipeHandle, data, size, &bytesRead, null);
			if (!result)
			{
				_lastError = GetLastError();
			}
			return bytesRead;
		}

		void NamedPipeStream::Skip(__uint count)
		{
			BOOL result = ReadFile(_pipeHandle, 0, count, 0, null);
		}

		__bool NamedPipeStream::End()
		{
			return _pipeHandle != null;
		}

		__bool NamedPipeStream::Initialized()
		{
			return _pipeHandle != null && _pipeHandle != INVALID_HANDLE_VALUE;
		}

		__uint NamedPipeStream::ConvertAccessMode(__uint accessMode)
		{
			switch (accessMode)
			{
				case IStream::Input:
					return PIPE_ACCESS_INBOUND;
				case IStream::Output:
					return PIPE_ACCESS_OUTBOUND;
				default:
					return PIPE_ACCESS_DUPLEX;
			}
		}

		__uint NamedPipeStream::ConvertShareMode(__uint accessMode)
		{
			switch (accessMode)
			{
				case IStream::Input:
					return FILE_SHARE_WRITE;
				case IStream::Output:
					return FILE_SHARE_READ;
				default:
					return FILE_SHARE_READ | FILE_SHARE_WRITE;
			}
		}
	}
}