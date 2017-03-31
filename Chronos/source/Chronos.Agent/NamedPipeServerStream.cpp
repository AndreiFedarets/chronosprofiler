#include "stdafx.h"
#include "Chronos.Agent.h"

namespace Chronos
{
	namespace Agent
	{
		NamedPipeServerStream::NamedPipeServerStream(__string port, __uint accessMode)
		{
			__string pipeName(L"\\\\.\\pipe\\");
			pipeName.append(port);
			_lastError = 0;
			for (__int i = 0; i < 2000; i++)
			{
				_pipeHandle = CreateNamedPipeW(pipeName.c_str(), ConvertAccessMode(accessMode), PIPE_TYPE_BYTE | PIPE_WAIT, 1, 0, 3*1024*1024, 0, NULL);
				if (_pipeHandle != INVALID_HANDLE_VALUE)
				{
					break;
				}
				Sleep(1);
			}
		}

		__bool NamedPipeServerStream::WaitForConnection()
		{
			BOOL result = ConnectNamedPipe(_pipeHandle, null);
			if (result == FALSE)
			{
				return false;
			}
			return true;
		}

		__bool NamedPipeServerStream::Disconnect()
		{
			BOOL result = DisconnectNamedPipe(_pipeHandle);
			if (result == FALSE)
			{
				return false;
			}
			return true;
		}

		__bool NamedPipeServerStream::Disconnected()
		{
			return _lastError == (__uint)ERROR_BROKEN_PIPE;
		}
	}
}

