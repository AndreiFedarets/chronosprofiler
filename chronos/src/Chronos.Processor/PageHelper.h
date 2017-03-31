#pragma once
#include "SourcePage.h"
#include "ConvertedPage.h"
#include "NativeHelpers.h"
#include "BlockedCallstack.h"

typedef void (__stdcall *OnMergeCompletedCallback)(CConvertedPage);

class CPageHelper
{
public:
	static CConvertedPage ConvertPage(CSourcePage sourcePage)
	{
		CConvertedPage convertedPage;
		convertedPage.BeginLifetime = sourcePage.BeginLifetime;
		convertedPage.BeginPageRange = sourcePage.PageIndex;
		convertedPage.Data = CNativeHelpers::ConvertPageRwToIm(sourcePage.Data, sourcePage.DataSize, &(convertedPage.DataSize));
		convertedPage.EndLifetime = sourcePage.EndLifetime;
		convertedPage.EndPageRange = sourcePage.PageIndex;
		convertedPage.Flag = sourcePage.Flag;
		convertedPage.ThreadId = sourcePage.ThreadId;
		convertedPage.PageIndex = sourcePage.PageIndex;
		convertedPage.CallstackId = sourcePage.CallstackId;
		return convertedPage;
	}
	static CConvertedPage MergePage(CConvertedPage convertedPage)
	{
		CConvertedPage mergedPage = convertedPage;
		CBlockedCallstack* callstack = new CBlockedCallstack();
		if (convertedPage.BeginPageRange == 0)
		{
			callstack->LoadFull(convertedPage.Data, convertedPage.DataSize);
		}
		else
		{
			callstack->LoadSafe(convertedPage.Data, convertedPage.DataSize);
		}
		mergedPage.Data = callstack->Save(&(mergedPage.DataSize));
		__FREEOBJ(callstack);
		return mergedPage;
	}
	static CConvertedPage MergePages(CConvertedPage leftConvertedPage, CConvertedPage rightConvertedPage)
	{
		_ASSERT(leftConvertedPage.CallstackId == rightConvertedPage.CallstackId);
		_ASSERT((rightConvertedPage.BeginPageRange - leftConvertedPage.EndPageRange) == 1);
		CConvertedPage tempPage;
		tempPage.ThreadId = leftConvertedPage.ThreadId;
		tempPage.PageIndex = 0;
		tempPage.CallstackId = leftConvertedPage.CallstackId;
		tempPage.BeginLifetime = leftConvertedPage.BeginLifetime;
		tempPage.EndLifetime = rightConvertedPage.EndLifetime;
		tempPage.BeginPageRange = leftConvertedPage.BeginPageRange;
		tempPage.EndPageRange = rightConvertedPage.EndPageRange;
		tempPage.Flag = rightConvertedPage.Flag;
		tempPage.DataSize = leftConvertedPage.DataSize + rightConvertedPage.DataSize;
		tempPage.Data = new __byte[tempPage.DataSize];
		memcpy(tempPage.Data, leftConvertedPage.Data, leftConvertedPage.DataSize);
		memcpy(tempPage.Data + leftConvertedPage.DataSize, rightConvertedPage.Data, rightConvertedPage.DataSize);
		CConvertedPage resultPage = MergePage(tempPage);
		__FREEARR(tempPage.Data);
		return resultPage;
	}
	static CConvertedPage SortPage(CConvertedPage convertedPage)
	{
		CConvertedPage sortedPage = convertedPage;
		sortedPage.Data = new __byte[sortedPage.DataSize];
		CNativeHelpers::SortCallstackEvents(convertedPage.Data, convertedPage.Data + convertedPage.DataSize, sortedPage.Data);
		return sortedPage;
	}
	static __uint ReindexForNextLevel(__uint pageIndex)
	{
		return pageIndex / 2;
	}
	static bool ComparePages(CConvertedPage page1, CConvertedPage page2)
	{
		if (page1.DataSize != page2.DataSize)
		{
			return false;
		}
		for (__int i = 0; i < page1.DataSize; i++)
		{
			if (page1.Data[i] != page2.Data[i])
			{
				return false;
			}
		}
		return true;
	}
};