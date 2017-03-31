#pragma once
#include "Request.h"

class CTask
{
public:
	CTask(void (*action)(void*), CRequest* parameter);
	~CTask();
	void Execute();
private:
	void (*Action)(void*);
	CRequest* Parameter;
};

