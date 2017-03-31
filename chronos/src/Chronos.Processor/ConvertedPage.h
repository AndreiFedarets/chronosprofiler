#pragma once
#include "stdafx.h"
#include "PageState.h"

struct CConvertedPage
{
	CConvertedPage()
		: ThreadId(0), CallstackId(0), PageIndex(0), BeginPageRange(0), EndPageRange(0), BeginLifetime(0), EndLifetime(0), Flag(0), Data(null), DataSize(0), RootEvent(null)
	{
	}
    __uint ThreadId;
    __uint CallstackId;
    __uint PageIndex;
    __uint BeginPageRange;
    __uint EndPageRange;
    __uint BeginLifetime;
    __uint EndLifetime;
    __byte Flag;
    __byte* Data;
    __int DataSize;
    __byte* RootEvent;
	__bool IsRoot()
	{
		return Flag != CPageState::Continue && PageIndex == 0;
	}
	__byte Hand()
	{
		return PageIndex % 2;
	}
	__bool IsEmpty()
	{
		return Data == null || DataSize == 0;
	}
	void ReleaseData()
	{
		__FREEARR(Data);
	}
};