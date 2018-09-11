#include "stdafx.h"
#include "Chronos.Agent.h"
#include <tlhelp32.h>

namespace Chronos
{
	namespace Agent
	{
		RuntimeController::RuntimeController(Chronos::Agent::GatewayClient* gatewayClient)
				: _suspended(false), _gatewayClient(gatewayClient)
		{
			_suspendedThreads = new std::map<__uint, HANDLE>();
		}

		RuntimeController::~RuntimeController(void)
		{
			__FREEOBJ(_suspendedThreads)
		}

		HRESULT RuntimeController::SuspendRuntime()
		{
			Lock lock(&_criticalSection);
			if (_suspended)
			{
				return S_OK;
			}

			_suspended = true;
			
			std::map<__uint, HANDLE> threads;
			do
			{
				threads.clear();
				__RETURN_IF_FAILED(GetNonSuspendedThreads(&threads));
				for (std::map<__uint, HANDLE>::iterator i = threads.begin(); i != threads.end(); ++i)
				{
					__uint threadId = (*i).first;
					HANDLE threadHandle = (*i).second;
					SuspendThread(threadHandle);
					_suspendedThreads->insert(std::pair<__uint, HANDLE>(threadId, threadHandle));
				}
			}
			while (threads.size() > 0);
			return S_OK;
		}

		HRESULT RuntimeController::ResumeRuntime()
		{
			Lock lock(&_criticalSection);
			if (!_suspended)
			{
				return S_OK;
			}
			for (std::map<__uint, HANDLE>::iterator i = _suspendedThreads->begin(); i != _suspendedThreads->end(); ++i)
			{
				HANDLE threadHandle = (*i).second;
				ResumeThread(threadHandle);
				CloseHandle(threadHandle);
			}
			_suspendedThreads->clear();
			_suspended = false;
			return S_OK;
		}

		HRESULT RuntimeController::GetNonSuspendedThreads(std::map<__uint, HANDLE>* threads)
		{
			HANDLE threadSnapshot = INVALID_HANDLE_VALUE;
			THREADENTRY32 threadEntry;

			threadSnapshot = CreateToolhelp32Snapshot(TH32CS_SNAPTHREAD, 0);
			if (threadSnapshot == INVALID_HANDLE_VALUE)
			{
				return E_FAIL;
			}
			std::vector<__uint> ignoredThreads = GetIgnoredThreads();

			threadEntry.dwSize = sizeof(THREADENTRY32);
			if(!Thread32First(threadSnapshot, &threadEntry))
			{
				CloseHandle(threadSnapshot);
				return E_FAIL;
			}
			do
			{
				__uint processId = CurrentProcess::GetProcessId();
				if (threadEntry.th32OwnerProcessID == processId)
				{
					__uint osThreadId = threadEntry.th32ThreadID;
					if (_suspendedThreads->find(osThreadId) == _suspendedThreads->end())
					{
						if (!IsThreadIgnored(&ignoredThreads, osThreadId))
						{
							HANDLE threadHandle = OpenThread(THREAD_ALL_ACCESS, FALSE, threadEntry.th32ThreadID);
							threads->insert(std::pair<__uint, HANDLE>(osThreadId, threadHandle));
						}
					}
				}
			}
			while (Thread32Next(threadSnapshot, &threadEntry));
			CloseHandle(threadSnapshot);
			return S_OK;
		}

		std::vector<__uint> RuntimeController::GetIgnoredThreads()
		{
			std::vector<SingleCoreThread*> ignoredThreads;
			_gatewayClient->GetWorkingThreads(&ignoredThreads);
			__uint currentThreadId = GetCurrentThreadId();
			std::vector<__uint> ids;
			ids.push_back(currentThreadId);
			for (std::vector<SingleCoreThread*>::iterator i = ignoredThreads.begin(); i !=ignoredThreads.end(); ++i)
			{
				SingleCoreThread* thread = (*i);
				ids.push_back(thread->GetThreadId());
			}
			return ids;
		}

		__bool RuntimeController::IsThreadIgnored(std::vector<__uint>* ignoredThreads, __uint thread)
		{
			__bool isIgnored = false;
			for (std::vector<__uint>::iterator i = ignoredThreads->begin(); i != ignoredThreads->end(); ++i)
			{
				if ((*i) == thread)
				{
					return true;
				}
			}
			return false;
		}

		const __guid RuntimeController::ServiceToken = Converter::ConvertStringToGuid(L"{65142FBB-7D37-4962-9E30-B4D5CACE3429}");
	}
}