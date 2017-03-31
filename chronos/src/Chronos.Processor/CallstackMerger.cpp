#include "StdAfx.h"
#include "CallstackMerger.h"
#include "Requests.h"

void ConvertPage(void* parameter)
{
    CConvertPageRequest* request = (CConvertPageRequest*)parameter;
    CConvertedPage* convertedPage = new CConvertedPage();
    *convertedPage = CPageHelper::ConvertPage(*(request->Page));
    request->Merger->PushPage(convertedPage);
}

void MergePage(void* parameter)
{
    CMergePageRequest* request = (CMergePageRequest*)parameter;
    CConvertedPage* page = request->Page;
    if (page->IsEmpty())
    {
        return;
    }
    CConvertedPage* mergedPage = new CConvertedPage();
    *mergedPage = CPageHelper::MergePage(*page);
    mergedPage->PageIndex = CPageHelper::ReindexForNextLevel(page->PageIndex);
    request->Merger->GetNextLevel()->PushPage(mergedPage);
}

void MergePages(void* parameter)
{
    CMergePagesRequest* request = (CMergePagesRequest*)parameter;
    CConvertedPage* leftHandPage = request->LeftPage;
    CConvertedPage* rightHandPage = request->RightPage;
    CConvertedPage* mergedPage = new CConvertedPage();
    *mergedPage = CPageHelper::MergePages(*leftHandPage, *rightHandPage);
    mergedPage->PageIndex = CPageHelper::ReindexForNextLevel(leftHandPage->PageIndex);
    request->Merger->GetNextLevel()->PushPage(mergedPage);
}

CCallstackMerger::CCallstackMerger(__uint level, CTaskWorker* taskWorker, __uint capacity, void(*onMergedCallback)(CConvertedPage*))
    : _level(level), _taskWorker(taskWorker), _capacity(capacity), _originalCapacity(capacity), _nextLevelMerger(null), _onMergedCallback(onMergedCallback)
{
    _pages = CreatePages(capacity);
}

CCallstackMerger::~CCallstackMerger(void)
{
    __FREEARR(_pages);
    __FREEOBJ(_nextLevelMerger);
}

void CCallstackMerger::PushPage(CSourcePage* sourcePage)
{
    CConvertPageRequest* request = new CConvertPageRequest(sourcePage, this);
    CTask* task = new CTask(ConvertPage, request);
    _taskWorker->Push(task);
}

void CCallstackMerger::PushPage(CConvertedPage* page)
{
    if (page->IsRoot())
    {
        _onMergedCallback(page);
        return;
    }
    CheckCapacity(page->PageIndex + 1);
	__byte nodeHand = page->Hand();
    //Node is left-hand node
	CLock lock(&_pagesMonitor);
    if (nodeHand == 0)
    {
        if (page->Flag == CPageState::Close)
        {
            CMergePageRequest* request = new CMergePageRequest(page, this);
            CTask* task = new CTask(MergePage, request);
            _taskWorker->Push(task);
        }
        else
        {
            __uint rightHandPageIndex = page->PageIndex + 1;
            CConvertedPage* rightHandPage = _pages[rightHandPageIndex];
            //right-hand node is not exist, we cannot start merging - save node to _nodes
            if (rightHandPage == null)
            {
                _pages[page->PageIndex] = page;
            }
            //right-hand node exists, we can start merging
            else
            {
                _pages[rightHandPageIndex] = null;
                CMergePagesRequest* request = new CMergePagesRequest(page, rightHandPage, this);
                CTask* task = new CTask(MergePages, request);
                _taskWorker->Push(task);
            }
        }
    }
    else
    {
        __uint leftHandPageIndex = page->PageIndex - 1;
        CConvertedPage* leftHandPage = _pages[leftHandPageIndex];
        //left-hand node is not exist, we cannot start merging - save node to _nodes
        if (leftHandPage == null)
        {
            _pages[page->PageIndex] = page;
        }
        //left-hand node exists, we can start merging
        else
        {
            _pages[leftHandPageIndex] = null;
            CMergePagesRequest* request = new CMergePagesRequest(leftHandPage, page, this);
            CTask* task = new CTask(MergePages, request);
            _taskWorker->Push(task);
        }
    }
}

void CCallstackMerger::CheckCapacity(__uint requiredCapacity)
{
    CLock lock(&_pagesMonitor);
    if (_capacity <= requiredCapacity)
    {
        __uint newCapacity = _capacity + _originalCapacity;
        if (requiredCapacity <= newCapacity)
        {
            newCapacity = requiredCapacity + 1;
        }
        CConvertedPage** pages = CreatePages(newCapacity);
        for (__uint i = 0; i < _capacity; i++)
        {
            pages[i] = _pages[i];
        }
        __FREEARR(_pages);
        _pages = pages;
        _capacity = newCapacity;
    }
}

CCallstackMerger* CCallstackMerger::GetNextLevel()
{
    CLock lock(&_nextLevelMonitor);
    if (_nextLevelMerger == null)
    {
        _nextLevelMerger = new CCallstackMerger(_level + 1, _taskWorker, _capacity / 2, _onMergedCallback);
    }
    return _nextLevelMerger;
}

CConvertedPage** CCallstackMerger::CreatePages(__uint capacity)
{
    CConvertedPage** pages = new CConvertedPage*[capacity];
    for (__uint i = 0; i < capacity; i++)
    {
        pages[i] = null;
    }
    return pages;
}