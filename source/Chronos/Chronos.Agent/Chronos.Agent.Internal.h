#pragma once
#include "Chronos.Agent.h"

namespace Chronos
{
	namespace Agent
	{
// ==================================================================================================================================================
		class CrashDumpLogger
		{
			public:
				static void Setup(__string dumpsDirectoryPath);
				static __string GetDumpsDirectoryPath();
			private:
				static __string _dumpsDirectoryPath;
				static __bool _initialized;
		};

// ==================================================================================================================================================
		class GatewayClientSettings
		{
			public:
				static void DisableSyncClient();
				static void EnableSyncClient();
				static volatile __bool IsSyncClientEnabled;
		};
		
// ==================================================================================================================================================
		class TaskWorkerInternal
		{
			public:
				~TaskWorkerInternal();
				void Push(ITask* task);
				static TaskWorkerInternal* GetCurrent();
			private:
				TaskWorkerInternal();
				void Start();
				void Stop();
				void ExecuteTasksLoop(void* paramter);
			private:
				__uint _threadsCount;
				volatile __bool _executing;
				MultiCoreThread* _workingThread;
				const static __uint TasksMaxCount = 32;
				static TaskWorkerInternal* _current;
				static CriticalSection _currentCriticalSection;
				InterlockedContainer<ITask>* _tasks[TasksMaxCount];
		};

// ==================================================================================================================================================
	}
}