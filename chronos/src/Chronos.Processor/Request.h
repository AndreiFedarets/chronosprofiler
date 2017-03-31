#pragma once
class CRequest
{
public:
	CRequest(void);
	~CRequest(void);
	virtual void Dispose() = 0;
};

