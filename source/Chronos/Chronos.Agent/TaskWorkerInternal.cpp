#include "stdafx.h"
#include "Chronos.Agent.Internal.h"

namespace Chronos
{
	namespace Agent
	{
		TaskWorkerInternal::TaskWorkerInternal()
		{
			_executing = false;
			_threadsCount = 2;
			for (__uint i = 0; i < TasksMaxCount; i++)
			{
				_tasks[i] = new InterlockedContainer<ITask>();
			}
		}

		TaskWorkerInternal::~TaskWorkerInternal()
		{
			Stop();
			__FREEOBJ(_workingThread);
			for (__ushort i = 0; i < TasksMaxCount; i++)
			{
				__FREEOBJ(_tasks[i]);
			}
		}

		TaskWorkerInternal* TaskWorkerInternal::GetCurrent()
		{
			if (_current == null)
			{
				Lock lock(&_currentCriticalSection);
				if (_current == null)
				{
					_current = new TaskWorkerInternal();
				}
			}
			return _current;
		}
		
		void TaskWorkerInternal::Start()
		{
			if (_executing)
			{
				return;
			}
			_executing = true;
			ThisCallback<TaskWorkerInternal>* callback = new ThisCallback<TaskWorkerInternal>(this, &TaskWorkerInternal::ExecuteTasksLoop);
			_workingThread = new MultiCoreThread(callback, _threadsCount);
			_workingThread->Start();
		}
		
		void TaskWorkerInternal::Stop()
		{
			if (_executing)
			{
				_executing = false;
				while (_workingThread->IsAlive())
				{
					Sleep(10);
				}
			}
		}

		void TaskWorkerInternal::Push(ITask* task)
		{
			if (!_executing)
			{
				Start();
			}
			//Go through the array until task will be inserted
			__bool taskPushed = false;
			do
			{
				for (__uint i = 0; i < TasksMaxCount; i++)
				{
					InterlockedContainer<ITask>* container = _tasks[i];
					if (container->Value == null)
					{
						ITask* temp = container->SetValue(task);
						//If we took not-null value (e.g. if second thread setted value before us)
						//then we should 're-push' this task again
						if (temp != null)
						{
							Push(temp);
						}
						taskPushed = true;
						break;
					}
				}
				if (!taskPushed)
				{
					Sleep(1);
				}
			}
			while (!taskPushed);
		}

		void TaskWorkerInternal::ExecuteTasksLoop(void* paramter)
		{
			__bool taskExecuted;
			while (_executing)
			{
				taskExecuted = false;
				for (__ushort i = 0; i < TasksMaxCount ; ++i)
				{
					InterlockedContainer<ITask>* container = _tasks[i];
					if (container->Value != null)
					{
						ITask* task = container->SetValue(null);
						//If we took not-null value (e.g. if second thread setted value before us)
						//then we should 're-send' this package again
						if (task != null)
						{
							task->Execute();
							__FREEOBJ(task);
							taskExecuted = true;
						}
					}
				}
				if (!taskExecuted)
				{
					Sleep(1);
				}
			}
		}

		TaskWorkerInternal* TaskWorkerInternal::_current = null;
		CriticalSection TaskWorkerInternal::_currentCriticalSection;
	}
}