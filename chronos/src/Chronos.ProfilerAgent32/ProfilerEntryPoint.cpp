#include "stdafx.h"
#include "ProfilerEntryPoint.h"


EXTERN_C void __stdcall FunctionEnterGlobalFast(FunctionID functionID)
{
#ifdef FUNCTION_EVENT_DIRECT_CALL
		CFunctionInfo* functionInfo = (CFunctionInfo*)functionID;
	#ifdef THREAD_LOGGER_TLS
		CThreadLoggerPool::CurrentLogger->Callstack->Call(EventTypes::FunctionCall, functionInfo);
	#else
		CThreadLogger* logger = ThreadLoggerPool->GetCurrentThreadLogger();
		logger->Callstack->Ret(EventTypes::FunctionCall, functionInfo);
	#endif
#else
	CProfilerEntryPointBase::ProfilerCallbacksGlobal->FunctionEnter(functionID, functionID, NULL, NULL);
#endif
}

EXTERN_C void __stdcall FunctionLeaveGlobalFast(FunctionID functionID)
{
#ifdef FUNCTION_EVENT_DIRECT_CALL
		CFunctionInfo* functionInfo = (CFunctionInfo*)functionID;
	#ifdef THREAD_LOGGER_TLS
		CThreadLoggerPool::CurrentLogger->Callstack->Ret(EventTypes::FunctionCall, functionInfo);
	#else
		CThreadLogger* logger = ThreadLoggerPool->GetCurrentThreadLogger();
		logger->Callstack->Ret(EventTypes::FunctionCall, functionInfo);
	#endif
#else
	CProfilerEntryPointBase::ProfilerCallbacksGlobal->FunctionLeave(functionID, functionID, NULL, NULL);
#endif
}

EXTERN_C void __stdcall FunctionTailcallGlobalFast(FunctionID functionID)
{
#ifdef FUNCTION_EVENT_DIRECT_CALL
		CFunctionInfo* functionInfo = (CFunctionInfo*)functionID;
	#ifdef THREAD_LOGGER_TLS
		CThreadLoggerPool::CurrentLogger->Callstack->Ret(EventTypes::FunctionCall, functionInfo);
	#else
		CThreadLogger* logger = ThreadLoggerPool->GetCurrentThreadLogger();
		logger->Callstack->Ret(EventTypes::FunctionCall, functionInfo);
	#endif
#else
	CProfilerEntryPointBase::ProfilerCallbacksGlobal->FunctionTailcall(functionID, functionID, NULL);
#endif
}
	
void _declspec(naked) FunctionEnterNakedFast(FunctionID functionID)
{
	__asm
	{
		push eax
		push ecx
		push edx
		push [esp + 16]
		call FunctionEnterGlobalFast
		pop edx
		pop ecx
		pop eax
		ret 4
	}
}

void _declspec(naked) FunctionLeaveNakedFast(FunctionID functionID)
{
	__asm
	{
		push eax
		push ecx
		push edx
		push [esp + 16]
		call FunctionLeaveGlobalFast
		pop edx
		pop ecx
		pop eax
		ret 4
	}
}

void _declspec(naked) FunctionTailcallNakedFast(FunctionID functionID)
{
	__asm
	{
		push eax
		push ecx
		push edx
		push [esp + 16]
		call FunctionTailcallGlobalFast
		pop edx
		pop ecx
		pop eax
		ret 4
	}
}

EXTERN_C void __stdcall FunctionEnterGlobalSlow(FunctionID functionID, UINT_PTR clientData, COR_PRF_FRAME_INFO frameInfo, COR_PRF_FUNCTION_ARGUMENT_INFO *argInfo)
{
	CProfilerEntryPoint::ProfilerCallbacksGlobal->FunctionEnter(functionID, clientData, frameInfo, argInfo);
}

EXTERN_C void __stdcall FunctionLeaveGlobalSlow(FunctionID functionID, UINT_PTR clientData, COR_PRF_FRAME_INFO frameInfo, COR_PRF_FUNCTION_ARGUMENT_RANGE *retvalRange)
{
	CProfilerEntryPoint::ProfilerCallbacksGlobal->FunctionLeave(functionID, clientData, frameInfo, retvalRange);
}

EXTERN_C void __stdcall FunctionTailcallGlobalSlow(FunctionID functionID, UINT_PTR clientData, COR_PRF_FRAME_INFO frameInfo)
{
	CProfilerEntryPoint::ProfilerCallbacksGlobal->FunctionTailcall(functionID, clientData, frameInfo);
}
	
void _declspec(naked) FunctionEnterNakedSlow(FunctionID functionID, UINT_PTR clientData, COR_PRF_FRAME_INFO func, COR_PRF_FUNCTION_ARGUMENT_INFO *argumentInfo)
{
	__asm
	{
		push	ebp				 // Create a standard frame
		mov	 ebp,esp
		pushad					  // Preserve all registers

		mov	 eax,[ebp+0x14]	  // argumentInfo
		push	eax
		mov	 ecx,[ebp+0x10]	  // func
		push	ecx
		mov	 edx,[ebp+0x0C]	  // clientData
		push	edx
		mov	 eax,[ebp+0x08]	  // functionID
		push	eax
		call	FunctionEnterGlobalSlow

		popad					   // Restore all registers
		pop	 ebp				 // Restore EBP
		ret	 16
	}
}

void _declspec(naked) FunctionLeaveNakedSlow(FunctionID functionID, UINT_PTR clientData, COR_PRF_FRAME_INFO func, COR_PRF_FUNCTION_ARGUMENT_RANGE *retvalRange)
{
	__asm
	{
		push	ebp				 // Create a standard frame
		mov	 ebp,esp
		pushad					  // Preserve all registers

		mov	 eax,[ebp+0x14]	  // argumentInfo
		push	eax
		mov	 ecx,[ebp+0x10]	  // func
		push	ecx
		mov	 edx,[ebp+0x0C]	  // clientData
		push	edx
		mov	 eax,[ebp+0x08]	  // functionID
		push	eax
		call	FunctionLeaveGlobalSlow

		popad					   // Restore all registers
		pop	 ebp				 // Restore EBP
		ret	 16
	}
}

void _declspec(naked) FunctionTailcallNakedSlow(FunctionID functionID, UINT_PTR clientData, COR_PRF_FRAME_INFO func)
{
	__asm
	{
		push	ebp				 // Create a standard frame
		mov	 ebp,esp
		pushad					  // Preserve all registers

		mov	 eax,[ebp+0x14]	  // argumentInfo
		push	eax
		mov	 ecx,[ebp+0x10]	  // func
		push	ecx
		mov	 edx,[ebp+0x0C]	  // clientData
		push	edx
		mov	 eax,[ebp+0x08]	  // functionID
		push	eax
		call	FunctionTailcallGlobalSlow

		popad					   // Restore all registers
		pop	 ebp				 // Restore EBP
		ret	 16
	}
}

HRESULT CProfilerEntryPoint::InitializeFunctionCallbacks(__bool useFastCallbacks, ICorProfilerInfo2* corProfilerInfo2)
{
	HRESULT result;
	if (useFastCallbacks)
	{
		result = _corProfilerInfo2->SetEnterLeaveFunctionHooks(FunctionEnterNakedFast, FunctionLeaveNakedFast, FunctionTailcallNakedFast);
	}
	else
	{
		result = _corProfilerInfo2->SetEnterLeaveFunctionHooks2(FunctionEnterNakedSlow, FunctionLeaveNakedSlow, FunctionTailcallNakedSlow);
	}
	return result;
}