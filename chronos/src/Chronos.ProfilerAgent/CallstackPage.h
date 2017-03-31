#pragma once
#include "Units.h"
#include "TypeSize.h"

struct CCallstackPage
{
	CCallstackPage(__uint bufferSize);
	~CCallstackPage();
    void Initialize(__uint threadId, __uint callstackId, __uint pageIndex, __uint beginLifetime);
	void ClosePage(__byte flag, __uint endLifetime);
	__bool ContainsData();
	void Write(__byte eventType, __short depth, CUnitBase* unit, __uint time);
	void Write(__byte* frame, __uint size);

	__byte* Buffer;
	__uint ThreadId;
	__uint CallstackId;
	__uint PageIndex;
	__uint FullSize;
	__uint BufferSize;
	__uint MetaSize;
	__uint BeginLifetime;
	__bool Completed;

	enum StackFlag
	{
		Break = 0,
		Close = 1,
		Continue = 2,
	};
};
