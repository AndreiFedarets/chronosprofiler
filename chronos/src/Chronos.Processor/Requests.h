#pragma once
#include "ConvertedPage.h"
#include "SourcePage.h"
#include "CallstackMerger.h"
#include "Request.h"

class CConvertPageRequest : public CRequest
{
public:
	CConvertPageRequest(CSourcePage* page, CCallstackMerger* merger);
	~CConvertPageRequest();
	void Dispose();
	CSourcePage* Page;
	CCallstackMerger* Merger;
};

class CMergePageRequest : public CRequest
{
public:
	CMergePageRequest(CConvertedPage* page, CCallstackMerger* merger);
	~CMergePageRequest();
	void Dispose();
	CConvertedPage* Page;
	CCallstackMerger* Merger;
};

struct CMergePagesRequest : public CRequest
{
public:
	CMergePagesRequest(CConvertedPage* leftPage, CConvertedPage* rightPage, CCallstackMerger* merger);
	~CMergePagesRequest();
	void Dispose();
	CConvertedPage* LeftPage;
	CConvertedPage* RightPage;
	CCallstackMerger* Merger;
};

