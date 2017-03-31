#pragma once
class CBlock
{
public:
	CBlock(void);
	~CBlock(void);
	void Init(__byte* e);
	CBlock* AppendChild(CBlock* block);
	void GetCount(__int* count);
	__byte* Save(__byte* buffer);
public:
	__byte* Data;
	__long Token;
	__short Depth;
	CBlock* Prev;
	CBlock* Next;
	CBlock* FirstChild;
	CBlock* LastChild;
	CBlock* Parent;
};

