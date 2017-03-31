#include "stdafx.h"
#include "Chronos.Agent.h"

namespace Chronos
{
	namespace Agent
	{
		DWORD WINAPI __SingleCoreThreadCallbackInternal(LPVOID paramter)
		{
			LPVOID* params = (LPVOID*)paramter;
			ICallback* callback = (ICallback*)params[0];
			void* param = params[1];
			callback->Call(param);
			return 0;
		}

		SingleCoreThread::SingleCoreThread(ICallback* callback, __bool keepCallbackAlive)
		{
			_callback = callback;
			_threadHandle = null;
			_win32ThreadID = 0;
			_keepCallbackAlive = keepCallbackAlive;
		}
		
		SingleCoreThread::SingleCoreThread(ICallback* callback)
		{
			_callback = callback;
			_threadHandle = null;
			_win32ThreadID = 0;
			_keepCallbackAlive = false;
		}

		SingleCoreThread::~SingleCoreThread(void)
		{
			if (_threadHandle != null)
			{
				Stop();
			}
			if (!_keepCallbackAlive)
			{
				__FREEOBJ(_callback);
			}
		}
		
		void SingleCoreThread::Start()
		{
			Start(null);
		}

		void SingleCoreThread::Start(LPVOID parameter)
		{
			if (_threadHandle != null)
			{
				return;
			}
			LPVOID* params = new LPVOID[2];
			params[0] = _callback;
			params[1] = parameter;
			_threadHandle = CreateThread(null, 0, __SingleCoreThreadCallbackInternal, params, 0, &_win32ThreadID);
			//SetThreadName();
			//SetThreadPriority(_threadHandle, THREAD_PRIORITY_HIGHEST);
		}

		void SingleCoreThread::Stop()
		{
			if (_threadHandle == null)
			{
				return;
			}
			TerminateThread(_threadHandle, 0);
			_threadHandle = null;
		}

		__bool SingleCoreThread::IsAlive()
		{
			// Read thread's exit code.
			DWORD exitCode = 0;
			if(GetExitCodeThread(_threadHandle, &exitCode))
			{
				// if return code is STILL_ACTIVE,
				// then thread is live.
				return (exitCode == STILL_ACTIVE);
			}
			else
			{
				return false;
			}
		}

		HANDLE SingleCoreThread::GetThreadHandle()
		{
			return _threadHandle;
		}

		__uint SingleCoreThread::GetThreadId()
		{
			return _win32ThreadID;
		}

		__bool SingleCoreThread::SetPriority(__int priority)
		{
			if (_threadHandle == null)
			{
				return false;
			}
			if (::SetThreadPriority(_threadHandle, priority))
			{
				return true;
			}
			return false;
		}

		__bool SingleCoreThread::SetPriorityClass(__int priorityClass)
		{
			if (_threadHandle == null)
			{
				return false;
			}
			if (::SetPriorityClass(_threadHandle, priorityClass))
			{
				return true;
			}
			return false;
		}

		////source: http://msdn.microsoft.com/en-us/library/xcb2z8hs.aspx
		//void SingleCoreThread::SetThreadName()
		//{
		//	if (_threadName == null || _threadName->empty())
		//	{
		//		return;
		//	}

		//	const DWORD MS_VC_EXCEPTION=0x406D1388;

		//	#pragma pack(push,8)
		//	typedef struct tagTHREADNAME_INFO
		//	{
		//	   DWORD dwType; // Must be 0x1000.
		//	   LPCSTR szName; // Pointer to name (in user addr space).
		//	   DWORD dwThreadID; // Thread ID (-1=caller thread).
		//	   DWORD dwFlags; // Reserved for future use, must be zero.
		//	} THREADNAME_INFO;
		//	#pragma pack(pop)

		//	THREADNAME_INFO info;
		//	info.dwType = 0x1000;
		//	info.szName = _threadName->c_str();
		//	info.dwThreadID = _win32ThreadID;
		//	info.dwFlags = 0;

		//	__try
		//	{
		//		RaiseException( MS_VC_EXCEPTION, 0, sizeof(info)/sizeof(ULONG_PTR), (ULONG_PTR*)&info );
		//	}
		//	__except(EXCEPTION_EXECUTE_HANDLER)
		//	{
		//	}
		//}
	}
}