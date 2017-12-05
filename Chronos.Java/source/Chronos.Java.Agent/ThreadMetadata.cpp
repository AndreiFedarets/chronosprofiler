#include "stdafx.h"
#include "Chronos.Java.Agent.h"
#include <string>

namespace Chronos
{
	namespace Agent
	{
		namespace Java
		{
			namespace Reflection
			{
				ThreadMetadata::ThreadMetadata(jvmtiEnv* jvmtiEnv, jthread thread, __uint threadId)
				{
					_jvmtiEnv = jvmtiEnv;
					_thread = thread;
					_threadId = threadId;
					_name = null;
				}

				ThreadMetadata::~ThreadMetadata()
				{
					__FREEOBJ(_name);
				}

				__uint ThreadMetadata::GetId()
				{
					return _threadId;
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
						//const __uint NameMaxLength = 1000;
						//__wchar nativeName[NameMaxLength];

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