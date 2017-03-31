#include "StdAfx.h"
#include "NamedPipeServerStream.h"


CNamedPipeServerStream::CNamedPipeServerStream(std::wstring name)
{
	for (__int i = 0; i < 2000; i++)
	{
		_pipeHandle = CreateNamedPipe(name.c_str(), PIPE_ACCESS_DUPLEX, PIPE_TYPE_BYTE | PIPE_WAIT, 1, 0, 0, 0, NULL);
		if (_pipeHandle != INVALID_HANDLE_VALUE)
		{
			break;
		}
		Sleep(1);
	}
	if (_pipeHandle == INVALID_HANDLE_VALUE)
	{
		_ASSERT(false);
		_pipeHandle = null;
	}
}

__bool CNamedPipeServerStream::WaitForConnection()
{
	BOOL result = ConnectNamedPipe(_pipeHandle, null);
	if (result == FALSE)
	{
		return false;
	}
	return true;
}

__bool CNamedPipeServerStream::Disconnect()
{
	BOOL result = DisconnectNamedPipe(_pipeHandle);
	if (result == FALSE)
	{
		return false;
	}
	return true;
}

