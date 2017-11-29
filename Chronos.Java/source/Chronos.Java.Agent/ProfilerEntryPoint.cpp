#include "stdafx.h"
#include "ProfilerEntryPoint.h"

extern "C" JNIEXPORT jint JNICALL Agent_OnLoad(JavaVM* vm, char* options, void* reserved) 
{
	/*jvmtiEventCallbacks c;
	c.MethodEntry*/
    return 0;
}

extern "C" JNIEXPORT jint JNICALL Agent_OnAttach(JavaVM* vm, char* options, void* reserved)
{
    return 0;
}

extern "C" JNIEXPORT jint JNICALL JNI_OnLoad(JavaVM* vm, void* reserved) 
{
    return JNI_VERSION_1_6;
}

ProfilerEntryPoint::ProfilerEntryPoint()
	: _application(null)
{
}

ProfilerEntryPoint::~ProfilerEntryPoint()
{
}