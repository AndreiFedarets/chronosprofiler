#include "StdAfx.h"
#include "RequestsServer.h"

DWORD WINAPI ProcessRequests(LPVOID parameter)
{
	CRequestsServer* requestsServer = (CRequestsServer*)parameter;
	while (true)
	{
		requestsServer->ProcessRequest();
	}
}

CRequestsServer::CRequestsServer(CProfilerController* controller, CProcessShadow* processShadow)
	: _controller(controller), _processShadow(processShadow), _thread(null), _stream(null)
{
}

CRequestsServer::~CRequestsServer(void)
{
}

HRESULT CRequestsServer::Initialize()
{
	std::wstring pipeName = CPipeNameFormatter::GetAgentServerPipeName(CCurrentProcess::GetProcessId());
	_stream = new CNamedPipeServerStream(pipeName);
	_thread = new CSingleCoreThread(ProcessRequests);
	_thread->Start(this);
	return S_OK;
}

void CRequestsServer::ProcessRequest()
{
	__bool result = _stream->WaitForConnection();
	if (!result)
	{
		return;
	}
	__long operationCode = CInt64Marshaler::Demarshal(_stream);
	__byte state;
	__uint unitTypes;
	switch(operationCode)
	{
		case CAgentOperationCode::FlushUnits:
			unitTypes = CUInt32Marshaler::Demarshal(_stream);
			FlushUnits(unitTypes);
			CInt64Marshaler::Marshal(CResultCode::Ok, _stream);
			break;
		case CAgentOperationCode::GetState:
			state = GetState();
			CInt64Marshaler::Marshal(CResultCode::Ok, _stream);
			CByteMarshaler::Marshal(state, _stream);
			break;
		case CAgentOperationCode::SetState:
			state = CByteMarshaler::Demarshal(_stream);
			SetState(state);
			CInt64Marshaler::Marshal(CResultCode::Ok, _stream);
			break;
		default:
			CInt64Marshaler::Marshal(CResultCode::NotSupported, _stream);
			break;
	}
	_stream->Disconnect();
}

void CRequestsServer::FlushUnits(__uint unitTypes)
{
	__uint unitType;
	//AppDomains
	unitType = CUnitType::AppDomain;
	if ((unitType & unitTypes) == unitType)
	{
		_processShadow->AppDomainManager->Flush();
	}
	//Assemblies
	unitType = CUnitType::Assembly;
	if ((unitType & unitTypes) == unitType)
	{
		_processShadow->AssemblyManager->Flush();
	}
	//Modules
	unitType = CUnitType::Module;
	if ((unitType & unitTypes) == unitType)
	{
		_processShadow->ModuleManager->Flush();
	}
	//Classes
	unitType = CUnitType::Class;
	if ((unitType & unitTypes) == unitType)
	{
		_processShadow->ClassManager->Flush();
	}
	//Functions
	unitType = CUnitType::Function;
	if ((unitType & unitTypes) == unitType)
	{
		_processShadow->FunctionManager->Flush();
	}
	//Exceptions
	unitType = CUnitType::Exception;
	if ((unitType & unitTypes) == unitType)
	{
		_processShadow->ExceptionManager->Flush();
	}
	//Threads
	unitType = CUnitType::Thread;
	if ((unitType & unitTypes) == unitType)
	{
		_processShadow->ThreadManager->Flush();
	}
}

__byte CRequestsServer::GetState()
{
	__byte state;
	if (_controller->IsEnabled)
	{
		state = (__byte)CSessionState::Profiling;
	}
	else
	{
		state = (__byte)CSessionState::Paused;
	}
	return state;
}

void CRequestsServer::SetState(__byte state)
{
	if (state == (__byte)CSessionState::Profiling)
	{
		_controller->IsEnabled = true;
	}
	else if (state == (__byte)CSessionState::Paused)
	{
		_controller->IsEnabled = false;
	}
}

void CRequestsServer::Dispose()
{
	__FREEOBJ(_thread);
	__FREEOBJ(_stream);
}
