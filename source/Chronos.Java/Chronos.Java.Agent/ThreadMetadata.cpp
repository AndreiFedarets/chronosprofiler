#include "stdafx.h"
#include "Chronos.Java.Agent.h"

namespace Chronos
{
	namespace Agent
	{
		namespace Java
		{
			namespace Reflection
			{
				ThreadMetadata::ThreadMetadata(jvmtiEnv* jvmtiEnv, jthread thread, __uint osThreadId)
				{
					_jvmtiEnv = jvmtiEnv;
					_thread = thread;
					_threadId = __JOBJECT_TO_UID(thread);
					_osThreadId = osThreadId;
					_name = null;
				}

				ThreadMetadata::~ThreadMetadata()
				{
					__FREEOBJ(_name);
				}

				__uptr ThreadMetadata::GetId()
				{
					return _threadId;
				}

				__uint ThreadMetadata::GetOsThreadId()
				{
					return _osThreadId;
				}

				__string* ThreadMetadata::GetName()
				{
					Initialize();
					return _name;
				}

				void ThreadMetadata::Initialize()
				{
					if (_name == null)
					{
						jvmtiThreadInfo info;
						_jvmtiEnv->GetThreadInfo(_thread, &info);
						std::string name(info.name);
						_name = new __string(name.begin(), name.end());
					}
				}
			}
		}
	}
}