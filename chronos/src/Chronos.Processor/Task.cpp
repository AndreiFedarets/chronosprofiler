#include "StdAfx.h"
#include "Task.h"


CTask::CTask(void (*action)(void*), CRequest* parameter)
		: Action(action), Parameter(parameter)
{
}

CTask::~CTask()
{
	Parameter->Dispose();
	__FREEOBJ(Parameter);
}

void CTask::Execute()
{
	Action(Parameter);
}
