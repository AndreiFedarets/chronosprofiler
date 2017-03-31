#pragma once
#include "SourcePage.h"
#include "ConvertedPage.h"
#include "TaskWorker.h"
#include "Lock.h"
#include "NativeHelpers.h"
#include "Lock.h"
#include "PageState.h"
#include "PageHelper.h"

class CCallstackMerger
{
public:
    CCallstackMerger(__uint level, CTaskWorker* taskWorker, __uint capacity, void(*onMergedCallback)(CConvertedPage*));
    ~CCallstackMerger(void);
    void CheckCapacity(__uint requiredCapacity);
    void PushPage(CSourcePage* sourcePage);
    void PushPage(CConvertedPage* convertedPage);
    CCallstackMerger* GetNextLevel();
private:
    CConvertedPage** CreatePages(__uint capacity);
    CTaskWorker* _taskWorker;
    CCallstackMerger* _nextLevelMerger;
    CConvertedPage** _pages;
    __uint _level;
    __uint _capacity;
    __uint _originalCapacity;
    CMonitor _pagesMonitor;
    CMonitor _nextLevelMonitor;
    void(*_onMergedCallback)(CConvertedPage*);
};