#pragma once
#include "StdAfx.h"
#include "CallstackPage.h"

CCallstackPage::CCallstackPage(__uint bufferSize)
		: ThreadId(0), CallstackId(0), PageIndex(0), BeginLifetime(0)
{
	//Thread id + callstack id + page index + actual page size + page flag + begin lifetime + end lifetime
	MetaSize = TypeSize::_INT32 + TypeSize::_INT32 + TypeSize::_INT32 + TypeSize::_INT32 + TypeSize::_BYTE + TypeSize::_INT32 + TypeSize::_INT32;
	BufferSize = bufferSize + MetaSize;
	FullSize = MetaSize;
	Buffer = new __byte[BufferSize];
}

CCallstackPage::~CCallstackPage()
{
	if (Buffer != null)
	{
		__FREEARR(Buffer);
		Buffer = null;
	}
}

void CCallstackPage::Initialize(__uint threadId, __uint callstackId, __uint pageIndex, __uint beginLifetime)
{
    FullSize = MetaSize;
    ThreadId = threadId;
    CallstackId = callstackId;
    PageIndex = pageIndex;
    BeginLifetime = beginLifetime;
}

void CCallstackPage::ClosePage(__byte flag, __uint endLifetime)
{
	__int offset = 0;

	memcpy(Buffer + offset, &ThreadId, TypeSize::_INT32);
	offset += TypeSize::_INT32;
	
	memcpy(Buffer + offset, &CallstackId, TypeSize::_INT32);
	offset += TypeSize::_INT32;
	
	memcpy(Buffer + offset, &PageIndex, TypeSize::_INT32);
	offset += TypeSize::_INT32;

	__uint dataSize = FullSize - MetaSize;
	memcpy(Buffer + offset, &dataSize, TypeSize::_INT32);
	offset += TypeSize::_INT32;

	memcpy(Buffer + offset, &flag, TypeSize::_BYTE);
	offset += TypeSize::_BYTE;

	memcpy(Buffer + offset, &BeginLifetime, TypeSize::_INT32);
	offset += TypeSize::_INT32;

	memcpy(Buffer + offset, &endLifetime, TypeSize::_INT32);
	offset += TypeSize::_INT32;
}

__bool CCallstackPage::ContainsData()
{
	return FullSize > MetaSize;
}

void CCallstackPage::Write(__byte eventType, __short depth, CUnitBase* unit, __uint time)
{
	memcpy(Buffer + FullSize, &eventType, 1);
	FullSize += 1;
	memcpy(Buffer + FullSize, &(unit->Id), 4);
	FullSize += 4;
	memcpy(Buffer + FullSize, &time, 4);
	FullSize += 4;
	memcpy(Buffer + FullSize, &depth, 2);
	FullSize += 2;
	Completed = FullSize == BufferSize;
}

void CCallstackPage::Write(__byte* frame, __uint size)
{
	memcpy(Buffer + FullSize, frame, size);
	FullSize += size;
	Completed = FullSize == BufferSize;
}