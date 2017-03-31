#pragma once
struct CBaseStream
{
	virtual __uint Write(void* data, __uint size) = 0;
	virtual __uint Read(void* data, __uint size) = 0;
	virtual void Dispose() = 0;
	virtual void Skip(__uint count) = 0;
	virtual __bool End() = 0;
};