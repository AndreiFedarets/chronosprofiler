#include "stdafx.h"
#include "Chronos.Agent.h"
#include "Chronos.Agent.Internal.h"

namespace Chronos
{
	namespace Agent
	{

		void TaskWorker::Push(ITask* task)
		{
			TaskWorkerInternal::GetCurrent()->Push(task);
		}
		
		void SetMaxThreadsCount(__ushort threadsCount)
		{
			//TODO: implement
		}

		void SetMinThreadsCount(__ushort threadsCount)
		{
			//TODO: implement
		}
	}
}