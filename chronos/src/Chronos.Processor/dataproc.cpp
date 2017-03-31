#pragma once
#include "stdafx.h"
#include "NativeHelpers.h"
#include "ConvertedPage.h"
#include "SourcePage.h"
#include "BlockedCallstack.h"
#include "PageProcessor.h"
#include "PageHelper.h"


 //static _CrtMemState MemState;
 
#pragma warning(push) 
#pragma warning(disable:4190)
//=====================================================================================================================
EXTERN_C __declspec(dllexport) __byte* Alloc(__int size)
{
    return new __byte[size];
}
//=====================================================================================================================
EXTERN_C __declspec(dllexport) void Free(__byte* pointer)
{
    __FREEARR(pointer);
}
//=====================================================================================================================
EXTERN_C __declspec(dllexport) CConvertedPage SortPage(CConvertedPage convertedPage)
{
    CConvertedPage sortedPage = CPageHelper::SortPage(convertedPage);
    return sortedPage;
}
//=====================================================================================================================
EXTERN_C __declspec(dllexport) CConvertedPage ConvertPage(CSourcePage sourcePage)
{
    CConvertedPage convertedPage = CPageHelper::ConvertPage(sourcePage);
    return convertedPage;
}
//=====================================================================================================================
EXTERN_C __declspec(dllexport) CConvertedPage MergePage(CConvertedPage convertedPage)
{
    CConvertedPage mergedPage = CPageHelper::MergePage(convertedPage);
    return mergedPage;
}
//=====================================================================================================================
extern "C" __declspec(dllexport) CConvertedPage MergePages(CConvertedPage leftConvertedPage, CConvertedPage rightConvertedPage)
{
    CConvertedPage resultPage = CPageHelper::MergePages(leftConvertedPage, rightConvertedPage);
    return resultPage;
}
//=====================================================================================================================
EXTERN_C __declspec(dllexport) void LoadProcessor(__uint threadsCount, OnMergeCompletedCallback onMergedCallback)
{
    if (CPageProcessor::CurrentProcessor == null)
    {
        CPageProcessor::CurrentProcessor = new CPageProcessor(threadsCount, onMergedCallback);
        CPageProcessor::CurrentProcessor->Start();
//#ifdef _DEBUG
//        _CrtMemState memState;
//        _CrtMemCheckpoint(&memState);
//        MemState = memState;
//#endif
    }
}
//=====================================================================================================================
EXTERN_C __declspec(dllexport) void UnloadProcessor()
{
    if (CPageProcessor::CurrentProcessor != null)
    {
        CPageProcessor::CurrentProcessor->Stop();
        __FREEOBJ(CPageProcessor::CurrentProcessor);
//#ifdef _DEBUG
//        _CrtMemDumpAllObjectsSince(&MemState);
//#endif
    }
}
//=====================================================================================================================
EXTERN_C __declspec(dllexport) void PushPage(CSourcePage sourcePage)
{
    CSourcePage* page = new CSourcePage();
    *page = sourcePage;
    CPageProcessor::CurrentProcessor->PushPage(page);
}
//=====================================================================================================================
extern "C" __declspec(dllexport) bool ComparePages(CConvertedPage page1, CConvertedPage page2)
{
    return CPageHelper::ComparePages(page1, page2);
}
//=====================================================================================================================
#pragma warning(pop) 