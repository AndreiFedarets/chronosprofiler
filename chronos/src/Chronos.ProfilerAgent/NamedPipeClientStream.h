#pragma once
#include "NamedPipeStream.h"

class CNamedPipeClientStream : public CNamedPipeStream
{
public:
	CNamedPipeClientStream(std::wstring name, __uint desiredAccess, __uint shareMode, LPSECURITY_ATTRIBUTES securityAttributes, __uint creationDisposition, __uint flagsAndAttributes);
};

