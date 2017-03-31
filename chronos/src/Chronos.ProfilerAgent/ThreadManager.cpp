#pragma once
#include "StdAfx.h"
#include "ThreadManager.h"

CThreadManager::CThreadManager(ICorProfilerInfo2* corProfilerInfo2)
		: CUnitManagerBase<CThreadInfo>(corProfilerInfo2)
{
	_mainThreadName = L"Main Thread";
	_finalizationThreadName = L"Finalization Thread";
	_workerThreadName = L"Worker Thread";
}

void CThreadManager::Initialize(CThreadInfo* unit)
{
	__try
	{
		switch (unit->Id)
		{
			case 0:
				unit->Name = _mainThreadName;
				break;
			case 1:
				unit->Name = _finalizationThreadName;
				break;
			default:
				unit->Name = _workerThreadName;
				break;
		}
	}
	__except(UnitExceptionFilter(GetExceptionCode(), GetExceptionInformation(), L"thread"))
	{
	}
}

__uint CThreadManager::GetUnitType()
{
	return CUnitType::Thread;
}
	
void CThreadManager::Serialize(CThreadInfo* unit, CBaseStream* stream)
{
	//Base
	CUnitManagerBase::Serialize(unit, stream);
	//OsThreadId
	stream->Write(&(unit->OsThreadId), TypeSize::_INT32);
}