#pragma once
#include <windows.h>

class EventTypes
{
public:
	static const __byte FunctionCall = 0x01;
	static const __byte AppDomainCreation = 0x02;
	static const __byte AppDomainShutdown = 0x03;
	static const __byte AssemblyLoad = 0x04;
	static const __byte AssemblyUnload = 0x05;
	static const __byte ModuleLoad = 0x06;
	static const __byte ModuleUnload = 0x07;
	static const __byte ClassLoad = 0x08;
	static const __byte ClassUnload = 0x09;
	static const __byte ThreadCreate = 0x0A;
	static const __byte ThreadDestroy = 0x0B;
	static const __byte GarbageCollection = 0x0C;
	static const __byte ExceptionThrown = 0x0D;
	static const __byte SqlQuery = 0xE;
};

