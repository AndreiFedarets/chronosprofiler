#include "StdAfx.h"
#include "PageProcessor.h"


DWORD WINAPI PreProcessPages(LPVOID parameter) 
{
    CPageProcessor* processor = (CPageProcessor*)parameter;
    processor->ProcessPagesInternal();
    return 0;
}

void OnMergeCompleted(CConvertedPage* mergedPage) 
{
    CPageProcessor::CurrentProcessor->CompletedMerge(mergedPage);
}

CPageProcessor::CPageProcessor(__uint threadsCount, OnMergeCompletedCallback onMergedCallback)
    : _pages(null), _merging(false), _mergers(null), _taskWorker(null), _onMergedCallback(onMergedCallback), _started(false)
{
    _taskWorker = new CTaskWorker(threadsCount);
    _preprocessThread = new CSingleCoreThread(PreProcessPages);
}

CPageProcessor::~CPageProcessor(void)
{
    __FREEOBJ(_taskWorker);
    __FREEOBJ(_pages);
    __FREEOBJ(_mergers);
}

void CPageProcessor::Start()
{
    _merging = true;
    _started = true;
    _pages = new std::queue<CSourcePage*>();
    _mergers = new std::map<__uint, CCallstackMerger*>();
    _taskWorker->Start();
    _preprocessThread->Start(this);
}

void CPageProcessor::Stop()
{
	WaitWhileMerging();
    CLock lock(&_mergerMonitor);
    std::map<__uint, CCallstackMerger*>::iterator i = _mergers->begin();
    for (i; i != _mergers->end(); ++i)
    {
        CCallstackMerger* merger = i->second;
        __FREEOBJ(merger);
    }
    _mergers->clear();
}

void CPageProcessor::WaitWhileMerging()
{
    while (HasPages())
    {
        Sleep(100);
    }
    _started = false;
    _merging = false;
    while (_preprocessThread->IsAlive())
    {
        Sleep(100);
    }
    _taskWorker->Stop();
}

__bool CPageProcessor::HasPages()
{
    CLock lock(&_pageMonitor);
    return !_pages->empty();
}

void CPageProcessor::PushPage(CSourcePage* sourcePage)
{
    if (!_started)
    {
        _ASSERT(false);
    }
    CLock lock(&_pageMonitor);
    _pages->push(sourcePage);
}

CSourcePage* CPageProcessor::TakePage()
{
    CSourcePage* page = null;
    CLock lock(&_pageMonitor);
    if (!_pages->empty())
    {
        page = _pages->front();
        _pages->pop();
    }
    return page;
}

void CPageProcessor::ProcessPagesInternal()
{
    while (true)
    {
        CSourcePage* sourcePage = TakePage();
        if (sourcePage == null)
        {
            if (_merging)
            {
                Sleep(1);
            }
            else
            {
                return;
            }
        }
        else
        {
            if (sourcePage->IsEmpty())
            {
                __FREEOBJ(sourcePage);
            }
            else
            {
                CCallstackMerger* merger = TakeMerger(sourcePage->CallstackId);
                merger->PushPage(sourcePage);
            }
        }
    }
}

CCallstackMerger* CPageProcessor::TakeMerger(__uint callstackId)
{
    CCallstackMerger* merger = null;
    CLock lock(&_mergerMonitor);
    std::map<__uint, CCallstackMerger*>::iterator pair = _mergers->find(callstackId);
    if (pair == _mergers->end())
    {
        const __uint defaultCapacity = 2048;
        merger = new CCallstackMerger(0, _taskWorker, defaultCapacity, OnMergeCompleted);
        _mergers->insert(std::pair<__uint, CCallstackMerger*>(callstackId, merger));
    }
    else
    {
        merger = pair->second;
    }
    return merger;
}

void CPageProcessor::CloseMerger(__uint callstackId)
{
    CLock lock(&_mergerMonitor);
    std::map<__uint, CCallstackMerger*>::iterator pair = _mergers->find(callstackId);
    if (pair != _mergers->end())
    {
        CCallstackMerger* merger = pair->second;
        _mergers->erase(callstackId);
        __FREEOBJ(merger);
    }
}

void CPageProcessor::CompletedMerge(CConvertedPage* mergedPage)
{
    CConvertedPage result = *mergedPage;
    if (mergedPage->Flag == CPageState::Close)
    {
        result.RootEvent = CNativeHelpers::GetRootEvent(mergedPage->Data, mergedPage->DataSize);
        _onMergedCallback(result);
    }
    else
    {
        mergedPage->ReleaseData();
    }
    CloseMerger(mergedPage->CallstackId);
    __FREEOBJ(mergedPage);
}

CPageProcessor* CPageProcessor::CurrentProcessor = null;