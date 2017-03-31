#pragma once
#include "ProfilerController.h"
#include "ProcessShadow.h"
#include "SessionState.h"
#include "CoreThread.h"
#include "NamedPipeServerStream.h"
#include "AgentOperationCode.h"


class CRequestsServer
{
public:
	CRequestsServer(CProfilerController* controller, CProcessShadow* processShadow);
	~CRequestsServer(void);
	HRESULT Initialize();
	void ProcessRequest();
	void Dispose();
private:
	void FlushUnits(__uint unitTypes);
	void SetState(__byte state);
	__byte GetState();
private:
	CProfilerController* _controller;
	CProcessShadow* _processShadow;
	CSingleCoreThread* _thread;
	CNamedPipeServerStream* _stream;
};

