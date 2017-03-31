#pragma once
#include "NamedPipeClientStream.h"

class CClientBase
{
public:
	CClientBase(std::wstring pipeName);
	~CClientBase(void);
	void Dispose();

protected:
	bool Connect();
	CBaseStream* _stream;
	std::wstring _pipeName;
};
