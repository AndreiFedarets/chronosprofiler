#pragma once
#include "LockFreeFactory.h"
#include "Block.h"
#include "HeaderBlock.h"

class CBlockedCallstack
{
public:
	CBlockedCallstack(void);
	~CBlockedCallstack(void);
	__int GetCount();
	void LoadFull(__byte* page, __int pageSize);
	void LoadSafe(__byte* page, __int pageSize);
	__byte* Save(__int* pageSize);
private:
	void LoadInternal(__byte* pageStart, __byte* pageEnd);
	CSLFFactory<CBlock>* _blockFactory;
	std::vector<CHeaderBlock*>* _headers;
	__byte* _firstPeakBlock;
	__uint _firstPeakBlockSize;
};

