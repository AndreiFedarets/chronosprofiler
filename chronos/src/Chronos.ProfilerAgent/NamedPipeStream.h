#pragma once
#include "BaseStream.h"

class CNamedPipeStream : public CBaseStream
{
public:
	CNamedPipeStream(void);
	~CNamedPipeStream(void);
	__uint Write(void* data, __uint size);
	__uint Read(void* data, __uint size);
	void Skip(__uint count);
	__bool End();
	void Dispose();
	__bool Initialized();
protected:
	HANDLE _pipeHandle;
};

