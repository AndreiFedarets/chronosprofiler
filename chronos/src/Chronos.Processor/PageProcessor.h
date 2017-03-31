#pragma once
#include "CoreThread.h"
#include "SourcePage.h"
#include "Lock.h"
#include "CallstackMerger.h"
#include "TaskWorker.h"

class CPageProcessor
{
public:
	CPageProcessor(__uint threadsCount, OnMergeCompletedCallback onMergedCallback);
	~CPageProcessor(void);
	void Start();
	void Stop();
	__declspec(noinline) void PushPage(CSourcePage* sourcePage);
	__declspec(noinline) CSourcePage* TakePage();
	void ProcessPagesInternal();
	__declspec(noinline) CCallstackMerger* TakeMerger(__uint callstackId);
	__declspec(noinline) void CloseMerger(__uint callstackId);
	void CompletedMerge(CConvertedPage* mergedPage);
	__bool HasPages();
	static CPageProcessor* CurrentProcessor;
private:
	void WaitWhileMerging();
	CTaskWorker* _taskWorker;
	CMonitor _pageMonitor;
	CMonitor _mergerMonitor;
	std::queue<CSourcePage*>* _pages;
	std::map<__uint, CCallstackMerger*>* _mergers;
	volatile __bool _merging;
	volatile __bool _started;
	ICoreThread* _preprocessThread;
	OnMergeCompletedCallback _onMergedCallback;
};

