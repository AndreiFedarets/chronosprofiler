#pragma once
#include <stack>
#include <queue>
#include <windows.h>
#include "BaseStream.h"
#include "Timer.h"
#include "Units.h"
#include "CallstackPage.h"
#include "CallstackProcessor.h"
#include "ProfilerController.h"

#define STACK_MAX_DEPTH 32767
#define STACK_FRAME_SIZE 11

class CCallstack
{
public:
	CCallstack(CProfilerController* controller, __uint pageSizeInEvents, __uint threadId);
	~CCallstack(void);
	void Call(__byte eventType, CUnitBase* unit);
	void Ret(__byte eventType, CUnitBase* unit);
	void CallRet(__byte eventType, CUnitBase* unit);
	__uint CurrentUnitId();
private:
	void FlushPage(__byte flag);
	void CloseStack(__byte flag);
	void CreatePage();
	__bool EndOfStack();
#ifdef CALLSTACK_RAW_EVENTS
	void Write(__byte* frame);
#else
	void Write(__byte eventType, __uint totalChildren, CUnitBase* unit, __uint time);
#endif

    CBaseStream* _directThreadStream;
	CCallstackStorage* _storage;

	__short _stackDepth;
	__uint _threadId;
	CCallstackPage* _page;
	__bool _pageIsReady;
	__uint _pageSizeInEvents;
	CProfilerController* _controller;
	__uint _currentPageIndex;
	__uint _currentCallstackId;

#ifdef CALLSTACK_RAW_EVENTS
	__byte** _frames;
#else
	__byte _eventType[STACK_MAX_DEPTH];
	CUnitBase* _unit[STACK_MAX_DEPTH];
	__uint _startTime[STACK_MAX_DEPTH];
#endif
};

