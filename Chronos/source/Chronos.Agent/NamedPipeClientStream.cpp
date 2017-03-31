#include "stdafx.h"
#include "Chronos.Agent.h"

namespace Chronos
{
	namespace Agent
	{
		NamedPipeClientStream::NamedPipeClientStream(__string port, __uint accessMode)
		{
			std::wstring pipeName(L"\\\\.\\pipe\\");
			pipeName.append(port);
			for (__int i = 0; i < 2000; i++)
			{
				_pipeHandle = CreateFileW(pipeName.c_str(), ConvertAccessMode(accessMode), ConvertShareMode(accessMode), null, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, null);
				if (_pipeHandle != INVALID_HANDLE_VALUE)
				{
					break;
				}
				Sleep(1);
			}
		}
	}
}