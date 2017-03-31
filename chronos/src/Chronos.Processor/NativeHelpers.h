#pragma once
#include "Macro.h"
//=====================================================================================================================
//================================================ CONST ==============================================================
//=====================================================================================================================
/*	LOWLEVEL (LL) EVENT LAYOUT						HIGHLEVEL (HL) EVENT LAYOUT

	0 |__1 EVENT TYPE								0 |__1 EVENT TYPE
	1 |												1 |
	2 |												2 |
	3 |												3 |
	4 |__4 UNIT										4 |__4 UNIT
	5 |												5 |
	6 |												6 |
	7 |												7 |
	8 |__4 TIME										8 |__4 TIME
	9 |												9 |
	10|__2 DEPTH									10|__2 DEPTH
	--												11|
	11												12|
													13|
													14|__4 HITS
													--
													15
*/

#define LL_EVENT_SIZE 11
#define HL_EVENT_SIZE 15

#define EVENT_TYPE_OFFSET 0
#define EVENT_UNIT_OFFSET 1
#define EVENT_TIME_OFFSET 5
#define EVENT_DEPTH_OFFSET 9
#define EVENT_HITS_OFFSET 11

#define MAX_DEPTH 30000
class CNativeHelpers
{
public:
//---------------------------------------------------------------------------------------------------------------------
	static __long GetToken(__byte* data)
	{
		__long token = 0;
		__byte* tokenPointer = (__byte*)(&token);
		__byte* eventTypePart = tokenPointer + 3;
		__int* unitPart = (__int*)(tokenPointer + 4);
		*eventTypePart = *(data + EVENT_TYPE_OFFSET);
		*unitPart = *(__int*)(data + EVENT_UNIT_OFFSET);
		return token;
	}
//---------------------------------------------------------------------------------------------------------------------
	static void MergeTimeAndHits(__byte* target, __byte* source)
	{
		*(__uint*)(target + EVENT_TIME_OFFSET) += *(__uint*)(source + EVENT_TIME_OFFSET);
		*(__int*)(target + EVENT_HITS_OFFSET) += *(__int*)(source + EVENT_HITS_OFFSET);
	}
//---------------------------------------------------------------------------------------------------------------------
	static __byte* FindFirstPeak(__byte* pageStart, __byte* pageEnd)
	{
		if(pageStart >= pageEnd)
		{
			return null;
		}
		__short peakDepth = 32767;
		__byte* pageOffset = pageStart;
		__short* currentDepth = null;
		__byte* firstPeak = null;
		while (pageOffset != pageEnd)
		{
			currentDepth = (__short*)(pageOffset + EVENT_DEPTH_OFFSET);
			if (*currentDepth < peakDepth)
			{
				peakDepth = *currentDepth;
				firstPeak = pageOffset;
			}
			pageOffset += HL_EVENT_SIZE;
		}
		return firstPeak;
	}
//---------------------------------------------------------------------------------------------------------------------
	static __byte* FindLastPeak(__byte* pageStart, __byte* pageEnd)
	{
		if(pageStart >= pageEnd)
		{
			return null;
		}
		__short peakDepth = 32767;
		__byte* pageOffset = pageStart;
		__short* currentDepth = null;
		__byte* lastPeak = null;
		while (pageOffset != pageEnd)
		{
			currentDepth = (__short*)(pageOffset + EVENT_DEPTH_OFFSET);
			if (*currentDepth < peakDepth)
			{
				peakDepth = *currentDepth;
				lastPeak = pageOffset;
			}
			else if (*currentDepth == peakDepth)
			{
				lastPeak = pageOffset;
			}
			pageOffset += HL_EVENT_SIZE;
		}
		return lastPeak;
	}
//---------------------------------------------------------------------------------------------------------------------
	static __byte* ConvertPageRwToIm(__byte* rwPage, __int rwPageSize, __int* imPageSize)
	{
		__int eventsCount = (__int)(rwPageSize / LL_EVENT_SIZE);
		*imPageSize = eventsCount * HL_EVENT_SIZE;
		__byte* imPage = new __byte[*imPageSize];
		__byte* imPageOffset = imPage;
		__byte* rwPageOffset = rwPage;
		__byte* rwPageEnd = rwPage + rwPageSize;
		while (rwPageOffset != rwPageEnd)
		{
			//use MAX_DEPTH here to filter events
			memcpy(imPageOffset, rwPageOffset, LL_EVENT_SIZE);
			__int* hits = (__int*)(imPageOffset + EVENT_HITS_OFFSET);
			*hits = (__int)1;
			imPageOffset += HL_EVENT_SIZE;
			rwPageOffset += LL_EVENT_SIZE;
		}
		return imPage;
	}
//---------------------------------------------------------------------------------------------------------------------
	static __byte* SortCallstackEvents(__byte* pageStart, __byte* pageEnd, __byte* result)
	{
		__byte* currentPeak = null;
		while ((currentPeak = CNativeHelpers::FindFirstPeak(pageStart, pageEnd)) != null)
		{
			memcpy(result, currentPeak, HL_EVENT_SIZE);
			result = CNativeHelpers::SortCallstackEvents(pageStart, currentPeak, result + HL_EVENT_SIZE);
			pageStart = currentPeak + HL_EVENT_SIZE;
		}
		return result;
	}
//---------------------------------------------------------------------------------------------------------------------
	static __byte* GetRootEvent(__byte* page, __int pageSize)
	{
		__byte* rootEvent = new __byte[HL_EVENT_SIZE];
		__int offset = pageSize - HL_EVENT_SIZE;
		memcpy(rootEvent, page + offset, HL_EVENT_SIZE);
		return rootEvent;
	}
};