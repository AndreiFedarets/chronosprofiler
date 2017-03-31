#pragma once
#include "BaseStream.h"

class CMemoryStream : public CBaseStream
{
public:
	CMemoryStream(__byte* data, __uint dataSize);
	CMemoryStream();
	~CMemoryStream();
	__uint Write(void* data, __uint size);
	__uint Read(void* data, __uint size);
	void Skip(__uint count);
	__bool End();
	__byte* ToArray();
	__uint GetLength();
	void Dispose();
private:
	void Init(__byte* buffer, __uint dataSize, __uint bufferSize, __uint bufferPageSize);
	void Resize();
	__byte* _buffer;
	__uint _bufferSize;
	__uint _dataSize;
	__uint _position;
	__uint _bufferPageSize;
};

