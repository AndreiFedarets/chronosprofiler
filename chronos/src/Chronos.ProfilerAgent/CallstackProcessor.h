#pragma once
#include "BaseStream.h"
#include "CallstackPage.h"
#include "CoreThread.h"
#include "Lock.h"
#include "CallstackStorage.h"

class CCallstackProcessor
{
public:
	CCallstackProcessor(CBaseStream* stream, CCallstackStorage* storage);
	~CCallstackProcessor(void);
	void Flush();
private:
	void WaitForFlush();
	CBaseStream* _stream;
	CCallstackStorage* _storage;
	ICoreThread* _thread;
};

