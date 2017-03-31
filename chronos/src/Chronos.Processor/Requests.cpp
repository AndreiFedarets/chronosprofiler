#include "StdAfx.h"
#include "Requests.h"

CConvertPageRequest::CConvertPageRequest(CSourcePage* page, CCallstackMerger* merger)
		: Page(page), Merger(merger)
{ 
}

CConvertPageRequest::~CConvertPageRequest()
{
	Dispose();
}

void CConvertPageRequest::Dispose()
{
	Page->ReleaseData();
	__FREEOBJ(Page);
}
//--------------------------------------------------------------------------------------------------------------------------
CMergePageRequest::CMergePageRequest(CConvertedPage* page, CCallstackMerger* merger)
		: Page(page), Merger(merger)
{
}
	
CMergePageRequest::~CMergePageRequest()
{
	Dispose();
}

void CMergePageRequest::Dispose()
{
	Page->ReleaseData();
	__FREEOBJ(Page);
}
//--------------------------------------------------------------------------------------------------------------------------
CMergePagesRequest::CMergePagesRequest(CConvertedPage* leftPage, CConvertedPage* rightPage, CCallstackMerger* merger)
		: LeftPage(leftPage), RightPage(rightPage), Merger(merger)
{

}

CMergePagesRequest::~CMergePagesRequest()
{
	Dispose();
}

void CMergePagesRequest::Dispose()
{
	LeftPage->ReleaseData();
	__FREEOBJ(LeftPage);
	RightPage->ReleaseData();
	__FREEOBJ(RightPage);
}