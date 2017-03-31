// dllmain.cpp : Defines the entry point for the DLL application.
#include "stdafx.h"
#include "Chronos.Agent.Internal.h"

BOOL APIENTRY DllMain( HMODULE hModule,
                       DWORD  ul_reason_for_call,
                       LPVOID lpReserved
					 )
{
	switch (ul_reason_for_call)
	{
	case DLL_PROCESS_ATTACH:
	case DLL_THREAD_ATTACH:
	case DLL_THREAD_DETACH:
	case DLL_PROCESS_DETACH:
		break;
	}
	return TRUE;
}


//=====================================================================================================================
EXTERN_C __declspec(dllexport) __byte* Alloc(__int size)
{
	return new __byte[size];
}

//=====================================================================================================================
EXTERN_C __declspec(dllexport) void Free(__byte* pointer)
{
	__FREEARR(pointer);
}

//=====================================================================================================================
EXTERN_C __declspec(dllexport) void SetupCrashDumpLogger(__wchar* dumpsDirectoryPath)
{
	__string path(dumpsDirectoryPath);
	Chronos::Agent::CrashDumpLogger::Setup(path);
}

//=====================================================================================================================
EXTERN_C __declspec(dllexport) UINT_PTR GatewayServer_Create(GUID sessionUid)
{
	Chronos::Agent::NamedPipeStreamFactory* factory = new Chronos::Agent::NamedPipeStreamFactory();
	factory->InitializeDaemonStreams(sessionUid);
	Chronos::Agent::GatewayServer* server = new Chronos::Agent::GatewayServer(factory);
	return reinterpret_cast<UINT_PTR>(server);
}

//=====================================================================================================================
EXTERN_C __declspec(dllexport) HRESULT GatewayServer_Start(UINT_PTR gatewayToken, __byte streamsCount)
{
	Chronos::Agent::GatewayServer* server = reinterpret_cast<Chronos::Agent::GatewayServer*>(gatewayToken);
	__RETURN_IF_FAILED( server->Start(streamsCount) );
	server->StartReading();
	return S_OK;
}

//=====================================================================================================================
EXTERN_C __declspec(dllexport) HRESULT GatewayServer_RegisterHandler(UINT_PTR gatewayToken, __byte dataMarker, UINT_PTR dataHandlerToken)
{
	Chronos::Agent::GatewayServer* server = reinterpret_cast<Chronos::Agent::GatewayServer*>(gatewayToken);
	Chronos::Agent::IDataHandler* handler = reinterpret_cast<Chronos::Agent::IDataHandler*>(dataHandlerToken);
	return server->RegisterHandler(dataMarker, handler);
}

//=====================================================================================================================
EXTERN_C __declspec(dllexport) __bool GatewayServer_IsLocked(UINT_PTR gatewayToken)
{
	//Chronos::Agent::GatewayServer* server = reinterpret_cast<Chronos::Agent::GatewayServer*>(gatewayToken);
	//__bool locked = server->IsLocked();
	
	return false;
}

//=====================================================================================================================
EXTERN_C __declspec(dllexport) void GatewayServer_Lock(UINT_PTR gatewayToken)
{
	Chronos::Agent::GatewayServer* server = reinterpret_cast<Chronos::Agent::GatewayServer*>(gatewayToken);
	server->Lock();
}

//=====================================================================================================================
EXTERN_C __declspec(dllexport) void GatewayServer_Destroy(UINT_PTR gatewayToken)
{
	Chronos::Agent::GatewayServer* server = reinterpret_cast<Chronos::Agent::GatewayServer*>(gatewayToken);
	__FREEOBJ(server);
}

//=====================================================================================================================
EXTERN_C __declspec(dllexport) UINT_PTR DataHandlerRouter_Create(Chronos::Agent::DataHandlerCallback callback)
{
	Chronos::Agent::DataHandlerRouter* router = new Chronos::Agent::DataHandlerRouter(callback);
	return reinterpret_cast<UINT_PTR>(router);
}

//=====================================================================================================================
EXTERN_C __declspec(dllexport) void DataHandlerRouter_Destroy(UINT_PTR routerToken)
{
	Chronos::Agent::DataHandlerRouter* router = reinterpret_cast<Chronos::Agent::DataHandlerRouter*>(routerToken);
	__FREEOBJ(router);
}

//=====================================================================================================================
EXTERN_C __declspec(dllexport) __bool DataHandler_HandlePackage(UINT_PTR handlerToken, __byte* data, __uint dataSize)
{
	Chronos::Agent::IDataHandler* handler = reinterpret_cast<Chronos::Agent::IDataHandler*>(handlerToken);
	return handler->HandlePackage(data, dataSize);
}