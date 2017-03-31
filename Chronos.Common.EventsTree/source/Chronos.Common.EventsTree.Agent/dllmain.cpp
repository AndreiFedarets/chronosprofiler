// dllmain.cpp : Defines the entry point for the DLL application.
#include "stdafx.h"
#include "Chronos.Common.EventsTree.Agent.Internal.h"

BOOL APIENTRY DllMain( HMODULE hModule,
                       DWORD  ul_reason_for_call,
                       LPVOID lpReserved
					 )
{
	switch (ul_reason_for_call)
	{
	case DLL_PROCESS_ATTACH:
		break;
	case DLL_THREAD_ATTACH:
		break;
	case DLL_THREAD_DETACH:
		//TODO: this code is thread-unsafe!!!!
		if (Chronos::Agent::Common::EventsTree::EventsTreeLoggerCollection::Instance != null)
		{
			Chronos::Agent::Common::EventsTree::EventsTreeLoggerCollection::Instance->DestroyLogger();
		}
		break;
	case DLL_PROCESS_DETACH:
		break;
	}
	return TRUE;
}


//=====================================================================================================================
EXTERN_C __declspec(dllexport) void CreateChronosProfilingType(Chronos::Agent::IProfilingTypeAdapter** adapter)
{
	*adapter = new Chronos::Agent::Common::EventsTree::ProfilingTypeAdapter();
}

//=====================================================================================================================
EXTERN_C __declspec(dllexport) UINT_PTR DataHandler_Create(Chronos::Agent::Common::EventsTree::EventsTreeMergeCompletedCallback mergedCallback)
{
	Chronos::Agent::Common::EventsTree::DataHandler* adapter = new Chronos::Agent::Common::EventsTree::DataHandler(mergedCallback);
	return reinterpret_cast<UINT_PTR>(adapter);
}

//=====================================================================================================================
EXTERN_C __declspec(dllexport) void DataHandler_Destroy(UINT_PTR token)
{
	Chronos::Agent::Common::EventsTree::DataHandler* adapter = reinterpret_cast<Chronos::Agent::Common::EventsTree::DataHandler*>(token);
	__FREEOBJ(adapter);
}

//=====================================================================================================================
EXTERN_C __declspec(dllexport) void EventTreeMerger_Merge(__byte* source, __int sourceSize, __byte* rootEvent, __int rootEventSize, __byte** result, __int* resultSize)
{
	Chronos::Agent::Common::EventsTree::BlockedCallstack callstack;
	__byte* combinedSource;
	__int combinedSourceSize;
	__bool appendCustomRootEvent = rootEvent != null && rootEventSize > 0;
	if (appendCustomRootEvent)
	{
		combinedSourceSize = sourceSize + rootEventSize;
		combinedSource = new __byte[combinedSourceSize];
		memcpy(combinedSource, rootEvent, rootEventSize);
		memcpy(combinedSource + rootEventSize, source, sourceSize);
		Chronos::Agent::Common::EventsTree::MergedEvent* events = (Chronos::Agent::Common::EventsTree::MergedEvent*)combinedSource;
		__int eventsCount = combinedSourceSize / sizeof(Chronos::Agent::Common::EventsTree::MergedEvent);
		for (int i = 1; i < eventsCount; i++)
		{
			events[i].Depth += 1;
		}
	}
	else
	{
		combinedSourceSize = sourceSize;
		combinedSource = source;
	}
	callstack.Load(combinedSource, combinedSourceSize);
	*result = callstack.Save(resultSize);
	if (appendCustomRootEvent)
	{
		delete[] combinedSource;
	}
}

