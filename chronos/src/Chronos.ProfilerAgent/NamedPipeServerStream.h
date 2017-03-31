#pragma once
#include "NamedPipeStream.h"

class CNamedPipeServerStream: public CNamedPipeStream
{
public:
	CNamedPipeServerStream(std::wstring name);
	__bool WaitForConnection();
	__bool Disconnect();
};

