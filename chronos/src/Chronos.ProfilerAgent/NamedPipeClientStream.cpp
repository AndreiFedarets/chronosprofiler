#include "StdAfx.h"
#include "NamedPipeClientStream.h"


CNamedPipeClientStream::CNamedPipeClientStream(std::wstring name, __uint desiredAccess, __uint shareMode, LPSECURITY_ATTRIBUTES securityAttributes, __uint creationDisposition, __uint flagsAndAttributes)
{
	for (__int i = 0; i < 2000; i++)
	{
		_pipeHandle = CreateFile(name.c_str(), desiredAccess, shareMode, securityAttributes, creationDisposition, flagsAndAttributes, null);
		if (_pipeHandle != INVALID_HANDLE_VALUE)
		{
			break;
		}
		Sleep(1);
	}
}