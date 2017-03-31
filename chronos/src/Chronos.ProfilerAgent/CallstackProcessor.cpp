#include "StdAfx.h"
#include "CallstackProcessor.h"

DWORD WINAPI FlushCallstackData(LPVOID parameter)
{
	CCallstackProcessor* processor = (CCallstackProcessor*)parameter;
	while (true)
	{
		Sleep(1);
		processor->Flush();
	}
}

CCallstackProcessor::CCallstackProcessor(CBaseStream* stream, CCallstackStorage* storage)
	: _stream(stream), _storage(storage)
{
	_thread = new CSingleCoreThread(FlushCallstackData);
	_thread->Start(this);
}

CCallstackProcessor::~CCallstackProcessor(void)
{
	WaitForFlush();
	__FREEOBJ(_thread);
	__FREEOBJ(_stream);
}

void CCallstackProcessor::Flush()
{
	CCallstackPage* page = null;
	while ((page = _storage->Take()) != null)
	{
		_stream->Write(page->Buffer, page->FullSize);
		__FREEOBJ(page);
	}
}

void CCallstackProcessor::WaitForFlush()
{
	while (!_storage->Empty())
	{
		Sleep(50);
	}
}
