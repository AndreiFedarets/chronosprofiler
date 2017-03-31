#pragma once
#include "Block.h"
#include "LockFreeFactory.h"
#include "NativeHelpers.h"

class CHeaderBlock
{
public:
	CHeaderBlock(CSLFFactory<CBlock>* factory);
	~CHeaderBlock(void);
	void Append(__byte* event);
	void GetCount(__int* count);
	void Load(__byte* pageStart, __byte* pageEnd);
	__byte* Save(__byte* page);
private:
	CSLFFactory<CBlock>* _blockFactory;
	CBlock* _header;
	CBlock* _current;
};

