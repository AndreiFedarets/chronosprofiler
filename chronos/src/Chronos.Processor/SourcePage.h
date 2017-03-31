#pragma once
#include "stdafx.h"

struct CSourcePage
{
	CSourcePage(__uint threadId,  __uint callstackId, __uint pageIndex, __uint beginLifetime, __uint endLifetime, __byte flag, __byte* data, __int dataSize)
		: ThreadId(threadId), CallstackId(callstackId), PageIndex(pageIndex), BeginLifetime(beginLifetime), EndLifetime(endLifetime), Flag(flag), Data(data), DataSize(dataSize)
	{
	}
	CSourcePage()
		: ThreadId(0), CallstackId(0), PageIndex(0), BeginLifetime(0), EndLifetime(0), Flag(0), Data(0), DataSize(0)
	{
	}
    __uint ThreadId;
    __uint CallstackId;
    __uint PageIndex;
    __uint BeginLifetime;
    __uint EndLifetime;
	__byte Flag;
    __byte* Data;
    __int DataSize;
	__bool IsEmpty()
	{
		return (Data == null || DataSize <= 0);
	}
	void ReleaseData()
	{
		__FREEARR(Data);
	}
};