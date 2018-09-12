#include "stdafx.h"
#include "FunctionCallbacks32Naked.h"
#include "FunctionCallbacks.h"

//=========================================================================
void _declspec(naked) FunctionEnterNaked(FunctionID functionId)
{
	__asm
	{
		push eax
		push ecx
		push edx
		//pushad
		push [esp + 0x10]
		call FunctionEnterGlobal
		//popad
		pop edx
		pop ecx
		pop eax
		ret 4 //SIZE functionId
	}
}

void _declspec(naked) FunctionLeaveNaked(FunctionID functionId)
{
	__asm
	{
		push eax
		push ecx
		push edx
		//pushad
		push [esp + 0x10]
		call FunctionLeaveGlobal
		//popad
		pop edx
		pop ecx
		pop eax
		ret 4 //SIZE functionId
	}
}

void _declspec(naked) FunctionTailcallNaked(FunctionID functionId)
{
	__asm
	{
		push eax
		push ecx
		push edx
		//pushad
		push [esp + 0x10]
		call FunctionTailcallGlobal
		//popad
		pop edx
		pop ecx
		pop eax
		ret 4 //SIZE functionId
	}
}
//=========================================================================
void _declspec(naked) FunctionEnter2Naked(FunctionID functionID, UINT_PTR clientData, COR_PRF_FRAME_INFO func, COR_PRF_FUNCTION_ARGUMENT_INFO* argumentInfo)
{
	__asm
	{
#ifdef DEBUG
		push ebp // Create a standard frame
		mov ebp,esp
#endif
		pushad // Preserve all registers

		mov eax,[ebp+0x14] // argumentInfo
		push eax
		mov ecx,[ebp+0x10] // func
		push ecx
		mov edx,[ebp+0x0C] // clientData
		push edx
		mov eax,[ebp+0x08] // functionID
		push eax
		call FunctionEnter2Global

		popad // Restore all registers
#ifdef DEBUG
		pop ebp // Restore EBP
#endif
		ret 16 //SIZE functionID + SIZE clientData + SIZE func + SIZE argumentInfo
	}
}

void _declspec(naked) FunctionLeave2Naked(FunctionID functionID, UINT_PTR clientData, COR_PRF_FRAME_INFO func, COR_PRF_FUNCTION_ARGUMENT_RANGE* retvalRange)
{
	__asm
	{
#ifdef DEBUG
		push ebp // Create a standard frame
		mov ebp,esp
#endif
		pushad // Preserve all registers

		mov eax,[ebp+0x14] // argumentInfo
		push eax
		mov ecx,[ebp+0x10] // func
		push ecx
		mov edx,[ebp+0x0C] // clientData
		push edx
		mov eax,[ebp+0x08] // functionID
		push eax
		call FunctionLeave2Global

		popad // Restore all registers
#ifdef DEBUG
		pop ebp // Restore EBP
#endif
		ret 16 //SIZE functionID + SIZE clientData + SIZE func + SIZE retvalRange
	}
}

void _declspec(naked) FunctionTailcall2Naked(FunctionID functionID, UINT_PTR clientData, COR_PRF_FRAME_INFO func)
{
	__asm
	{
#ifdef DEBUG
		push ebp // Create a standard frame
		mov ebp,esp
#endif
		pushad // Preserve all registers

		mov ecx,[ebp+0x10] // func
		push ecx
		mov edx,[ebp+0x0C] // clientData
		push edx
		mov eax,[ebp+0x08] // functionID
		push eax
		call FunctionTailcall2Global

		popad // Restore all registers
#ifdef DEBUG
		pop ebp // Restore EBP
#endif
		ret 12 //SIZE functionID + SIZE clientData + SIZE func
	}
}
//=========================================================================
void _declspec(naked) FunctionEnter3Naked(FunctionIDOrClientID functionIDOrClientID)
{
	__asm
	{
		pushad // Preserve all registers
		push [esp + 0x10]
		call FunctionEnter3Global
		popad // Restore all registers
		ret 4 //SIZE functionIDOrClientID
		//push eax
		//push ecx
		//push edx
		//push [esp + 0x10]
		//call FunctionEnter3Global
		//pop edx
		//pop ecx
		//pop eax
		//ret 4 //SIZE functionIDOrClientID
	}
}

void _declspec(naked) FunctionLeave3Naked(FunctionIDOrClientID functionIDOrClientID)
{
	__asm
	{
		pushad // Preserve all registers
		push [esp + 0x10]
		call FunctionLeave3Global
		popad // Restore all registers
		ret 4 //SIZE functionIDOrClientID
		//push eax
		//push ecx
		//push edx
		//push [esp + 0x10]
		//call FunctionLeave3Global
		//pop edx
		//pop ecx
		//pop eax
		//ret 4 //SIZE functionIDOrClientID
	}
}

void _declspec(naked) FunctionTailcall3Naked(FunctionIDOrClientID functionIDOrClientID)
{
	__asm
	{
		pushad // Preserve all registers
		push [esp + 0x10]
		call FunctionTailcall3Global
		popad // Restore all registers
		ret 4 //SIZE functionIDOrClientID
		//push eax
		//push ecx
		//push edx
		//push [esp + 0x10]
		//call FunctionTailcall3Global
		//pop edx
		//pop ecx
		//pop eax
		//ret 4 //SIZE functionIDOrClientID
	}
}
//=========================================================================
void _declspec(naked) FunctionEnter3WithInfoNaked(FunctionIDOrClientID functionIDOrClientID, COR_PRF_ELT_INFO eltInfo)
{

}

void _declspec(naked) FunctionLeave3WithInfoNaked(FunctionIDOrClientID functionIDOrClientID, COR_PRF_ELT_INFO eltInfo)
{

}

void _declspec(naked) FunctionTailcall3WithInfoNaked(FunctionIDOrClientID functionIDOrClientID, COR_PRF_ELT_INFO eltInfo)
{

}
//=========================================================================