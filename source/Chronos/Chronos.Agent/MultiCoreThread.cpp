#include "stdafx.h"
#include "Chronos.Agent.h"

namespace Chronos
{
	namespace Agent
	{
		MultiCoreThread::MultiCoreThread(ICallback* callback, __uint threadsCount)
			: _callback(callback), _threadsCount(threadsCount), _threads(null)
		{
		}

		MultiCoreThread::~MultiCoreThread(void)
		{
			Stop();
			__FREEOBJ(_callback);
		}
		
		void MultiCoreThread::Start()
		{
			Start(null);
		}

		void MultiCoreThread::Start(LPVOID parameter)
		{
			Stop();
			_threads = new SingleCoreThread*[_threadsCount];
			for (__uint i = 0; i < _threadsCount; i++)
			{
				_threads[i] = new SingleCoreThread(_callback, true);
			}
			for (__uint i = 0; i < _threadsCount; i++)
			{
				_threads[i]->Start(parameter);
			}
		}

		void MultiCoreThread::Stop()
		{
			if (_threads == null)
			{
				return;
			}
			for (__uint i = 0; i < _threadsCount; i++)
			{
				_threads[i]->Stop();
				__FREEOBJ(_threads[i]);
			}
			__FREEARR(_threads);
		}

		__bool MultiCoreThread::IsAlive()
		{
			if (_threads == null)
			{
				return false;
			}
			for (__uint i = 0; i < _threadsCount; i++)
			{
				if (_threads[i]->IsAlive())
				{
					return true;
				}
			}
			return false;
		}

		void MultiCoreThread::GetWorkingThreads(std::vector<SingleCoreThread*>* threads)
		{
			if (_threads == null)
			{
				return;
			}
			for (__uint i = 0; i < _threadsCount; i++)
			{
				SingleCoreThread* thread = _threads[i];
				if (thread->IsAlive())
				{
					threads->push_back(thread);
				}
			}
		}
		
		__bool MultiCoreThread::SetPriority(__int priority)
		{
			if (_threads == null)
			{
				return false;
			}
			__bool result = true;
			for (__uint i = 0; i < _threadsCount; i++)
			{
				SingleCoreThread* thread = _threads[i];
				if (!thread->SetPriority(priority))
				{
					result = false;
				}
			}
			return result;
		}

		__bool MultiCoreThread::SetPriorityClass(__int priorityClass)
		{
			if (_threads == null)
			{
				return false;
			}
			__bool result = true;
			for (__uint i = 0; i < _threadsCount; i++)
			{
				SingleCoreThread* thread = _threads[i];
				if (!thread->SetPriorityClass(priorityClass))
				{
					result = false;
				}
			}
			return result;
		}
	}
}