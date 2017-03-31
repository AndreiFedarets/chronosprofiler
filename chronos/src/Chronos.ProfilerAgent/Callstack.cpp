#include "StdAfx.h"
#include "Callstack.h"

CCallstack::CCallstack(CProfilerController* controller, __uint pageSizeInEvents, __uint threadId)
	: _currentPageIndex(0), _pageSizeInEvents(pageSizeInEvents), _controller(controller), _page(null), _pageIsReady(false), _stackDepth(-1), _threadId(threadId)
{
#ifdef CALLSTACK_RAW_EVENTS
	_frames = new __byte*[STACK_MAX_DEPTH];
	for (__int i = 0; i < STACK_MAX_DEPTH; i++)
	{
		_frames[i] = new __byte[STACK_FRAME_SIZE];
	}
#endif
	_currentCallstackId = controller->GenerateCallstackId();
    _directThreadStream = controller->CreateThreadStream(threadId);
	_storage = controller->GetCallstackStorage();
}

CCallstack::~CCallstack(void)
{
	if (_pageIsReady)
	{
		if (_page->ContainsData())
		{
			FlushPage(CCallstackPage::Break);
		}
		else
		{
			__FREEOBJ(_page);
		}
	}
}

void CCallstack::Call(__byte eventType, CUnitBase* unit)
{
	if (!_controller->IsEnabled && _stackDepth < 0)
	{
		return;
	}
	if (!_pageIsReady)
	{
		CreatePage();
	}
#ifdef CALLSTACK_RAW_EVENTS
	__byte* var_byte_0;
	__uint* var_ulong_0;
	__short* var_short_0;
	__byte* frame;
	_stackDepth++;
	frame = _frames[_stackDepth];
	//EventType
	var_byte_0 = frame;
	*var_byte_0 = eventType;
	//UnitId
	var_ulong_0 = (__uint*)(frame + 1);
	*var_ulong_0 = unit->Id;
	//Time
	var_ulong_0 = (__uint*)(frame + 5);
	*var_ulong_0 = CTimer::CurrentTime;
	//Depth
	var_short_0 = (__short*)(frame + 9);
	*var_short_0 = _stackDepth;
#else
	_stackDepth++;
	_eventType[_stackDepth] = eventType;
	_unit[_stackDepth] = unit;
	_startTime[_stackDepth] = CTimer::CurrentTime;
#endif
}

void CCallstack::Ret(__byte eventType, CUnitBase* unit)
{
	if (_stackDepth < 0)
	{
		return;
	}
	if (!_pageIsReady)
	{
		CreatePage();
	}
#ifdef CALLSTACK_RAW_EVENTS
	__uint* var_ulong_0;
	__byte* frame = _frames[_stackDepth];
	//Time
	var_ulong_0 = (__uint*)(frame + 5);
	*var_ulong_0 = CTimer::CurrentTime - *var_ulong_0;
	_page->Write(frame, STACK_FRAME_SIZE);
#else
	__byte currentEventType = _eventType[_stackDepth];
	CUnitBase* currentUnit = _unit[_stackDepth];
	__uint startTime = _startTime[_stackDepth];
	__uint endTime = CTimer::CurrentTime;
	_page->Write(eventType, _stackDepth, unit, endTime - startTime);
#endif
	_stackDepth--;
	if (EndOfStack())
	{
		FlushPage(CCallstackPage::Close);
	}
	else if (_page->Completed)
	{
		FlushPage(CCallstackPage::Continue);
	}
}

void CCallstack::CallRet(__byte eventType, CUnitBase* unit)
{
	Call(eventType, unit);
	Ret(eventType, unit);
}

__uint CCallstack::CurrentUnitId()
{
	__byte* frame = _frames[_stackDepth];
	return *(__uint*)(frame + 1);
}

void CCallstack::FlushPage(__byte flag)
{
	_page->ClosePage(flag, CTimer::CurrentTime);
	__bool directWrite = _storage->Full();
	__bool createNewPage = !directWrite;
	if (directWrite)
	{
		_directThreadStream->Write(_page->Buffer, _page->FullSize);
	}
	else
	{
		_storage->Push(_page);
		_page = null;
	}
	if (flag != CCallstackPage::Continue)
	{
		_currentPageIndex = 0;
		_currentCallstackId = _controller->GenerateCallstackId();
	}
	_pageIsReady = false;
}

__bool CCallstack::EndOfStack()
{
	return _stackDepth == -1;
}

void CCallstack::CreatePage()
{
	if (_page == null)
	{
		_page = new CCallstackPage(_pageSizeInEvents * STACK_FRAME_SIZE);
	}
    _page->Initialize(_threadId, _currentCallstackId, _currentPageIndex, CTimer::CurrentTime);
	_currentPageIndex++;
	_pageIsReady = true;
}