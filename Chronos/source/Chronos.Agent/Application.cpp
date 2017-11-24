#include "StdAfx.h"
#include "Chronos.Agent.Internal.h"
#include <Rpc.h>

namespace Chronos
{
	namespace Agent
	{
		Application* CurrentApplication = null;

		__int FlushDataAgentCallback(Buffer* argumentsBuffer, Buffer** returnBuffer)
		{
			CurrentApplication->FlushData();
			return 0;
		}

		Application::Application(void)
		{
			//MessageBox(null, L"asdasd", null, MB_OK);
			__ASSERT(CurrentApplication == null, L"Application::Application: attempt to create second instance of Application");
			UuidCreate(&_applicationUid);
			CurrentApplication = this;
			_streamFactory = new NamedPipeStreamFactory();
			_gatewayClient = new GatewayClient(_streamFactory);
			_frameworks = new FrameworkCollection();
			_profilingTypes = new ProfilingTypeCollection();
			_runtimeController = new RuntimeController(_gatewayClient);
			_invokeServer = new AgentInvokeServer();
			Container = new ServiceContainer();
			_profilingTimer = new ProfilingTimer();
			_profilingTarget = null;
		}

		Application::~Application(void)
		{
			__FREEOBJ(_gatewayClient);
			__FREEOBJ(_frameworks);
			__FREEOBJ(_profilingTypes);
			__FREEOBJ(_runtimeController);
			__FREEOBJ(_streamFactory);
			__FREEOBJ(_invokeServer);
			__FREEOBJ(_profilingTarget);
			CurrentApplication = null;
		}

		// STARTUP ============================================================================================================
		HRESULT Application::Run()
		{
			__RETURN_IF_FAILED(BeginInitialize());
			__RETURN_IF_FAILED(ExportServices());
			__RETURN_IF_FAILED(LoadExtensions());
			__RETURN_IF_FAILED(EndInitialize());
			return S_OK;
		}

		HRESULT Application::Shutdown()
		{
			__RETURN_IF_FAILED(ShutdownInternal());
			return S_OK;
		}

		// ATTACH =============================================================================================================
		HRESULT Application::Attach()
		{
			__RETURN_IF_FAILED(BeginInitialize());
			__RETURN_IF_FAILED(ExportServices());
			__RETURN_IF_FAILED(LoadExtensions());
			__RETURN_IF_FAILED(EndInitialize());
			return S_OK;
		}

		HRESULT Application::Attached()
		{
			return S_OK;
		}

		HRESULT Application::Deattach()
		{
			__RETURN_IF_FAILED(ShutdownInternal());
			return S_OK;
		}

		// FAULT ==============================================================================================================
		HRESULT Application::Fault(EXCEPTION_POINTERS* exceptionInfo)
		{
			//GenerateDump(CurrentProcess::GetProcessId(), L"C:\\dumps\\crash.dmp", exceptionInfo);
			return S_OK;
		}

		void Application::FlushData()
		{
			//Prevent data corruption/hangs in sync gatewayClient - when thread is sending data, it's acquired internal lock on handle (probably),
			//but we are doing suspend of this thread. As result writing in this handle may have unexpected behavoiur.
			GatewayClientSettings::DisableSyncClient();
			_runtimeController->SuspendRuntime();
			_profilingTypes->FlushData();
			_gatewayClient->WaitWhileSending();
			//Now we can unlock sync gatewayClient, all data was send via async gatewayClient
			GatewayClientSettings::EnableSyncClient();
			_runtimeController->ResumeRuntime();
		}

		HRESULT Application::BeginInitialize()
		{
			//Setup unhandler exception filter to force logs saving.
			//TODO: it's not perfect implementation, because some code may override this filter
			//CrashDumpLogger::Setup();

			//Get session token from environment variables
			__string configurationTokenString = EnvironmentVariables::Get(PROFILER_CONFIGURATION_TOKEN_VARIABLE);
			if (configurationTokenString.empty())
			{
				return E_FAIL; 
			}
			//Convert session token string to guid
			__RETURN_IF_FAILED( Converter::TryConvertStringToGuid(configurationTokenString, &_configurationToken) );

			//Create host client
			HostClient client(_streamFactory);

			//Start session and load settings from Host Server
			__uint profilingBeginTime = _profilingTimer->GetBeginTime();
			if (!client.StartProfilingSession(_configurationToken, _applicationUid, profilingBeginTime,  &_sessionSettings))
			{
				return E_FAIL;
			}

			__RETURN_IF_FAILED( _sessionSettings.ValidateAndSort() );

			//Initialize data streams (out-streams)
			__guid sessionUid;
			if (!_sessionSettings.GetSessionUid(&sessionUid))
			{
				return E_FAIL;
			}

			__RETURN_IF_FAILED( _streamFactory->InitializeDaemonStreams(sessionUid) );
			__RETURN_IF_FAILED( _invokeServer->Initialize(_streamFactory, _applicationUid) );

			//get gateway settings
			GatewaySettings* gatewaySettings = null;
			if (!_sessionSettings.GetGatewaySettings(&gatewaySettings))
			{
				return E_FAIL;
			}

			__RETURN_IF_FAILED( _gatewayClient->Initialize(gatewaySettings) );
			_invokeServer->RegisterCallback(Converter::ConvertStringToGuid(L"{D0A43C7E-DDB7-4BAD-8418-7967EBB85EE5}"), FlushDataAgentCallback);

			return S_OK;
		}

		HRESULT Application::ExportServices()
		{
			__REGISTER_SERVICE(Container, GatewayClient, _gatewayClient);
			__REGISTER_SERVICE(Container, ProfilingTimer, _profilingTimer);
			return S_OK;
		}

		HRESULT Application::LoadExtensions()
		{
			ProfilingTargetSettings* profilingTargetSettings;
			if (!_sessionSettings.GetProfilingTargetSettings(&profilingTargetSettings))
			{
				return E_FAIL;
			}
			ProfilingTypeSettingsCollection* profilingTypesSettings;
			if (!_sessionSettings.GetProfilingTypesSettings(&profilingTypesSettings))
			{
				return E_FAIL;
			}
			FrameworkSettingsCollection* frameworksSettings;
			if (!_sessionSettings.GetFrameworksSettings(&frameworksSettings))
			{
				return E_FAIL;
			}

			
			_profilingTarget = new ProfilingTarget(profilingTargetSettings);
			__RETURN_IF_FAILED( _profilingTarget->LoadAdapter() );
			__RETURN_IF_FAILED( _profilingTarget->BeginInitialize() );
			__RETURN_IF_FAILED( _profilingTarget->ExportServices(Container) );
			__RETURN_IF_FAILED( _profilingTarget->ImportServices(Container) );
			__RETURN_IF_FAILED( _profilingTarget->EndInitialize() );

			//Load and initialize Frameworks
			__RETURN_IF_FAILED( _frameworks->Initialize(frameworksSettings, profilingTargetSettings, Container) );
			__RETURN_IF_FAILED( _frameworks->LoadExtensions() );
			__RETURN_IF_FAILED( _frameworks->BeginInitialize() );
			__RETURN_IF_FAILED( _frameworks->ExportServices() );
			__RETURN_IF_FAILED( _frameworks->ImportServices() );
			__RETURN_IF_FAILED( _frameworks->EndInitialize() );
			
			//Load and initialize Profiling Types
			__RETURN_IF_FAILED(_profilingTypes->Initialize(profilingTypesSettings, Container));
			__RETURN_IF_FAILED(_profilingTypes->LoadExtensions());
			__RETURN_IF_FAILED(_profilingTypes->BeginInitialize());
			__RETURN_IF_FAILED(_profilingTypes->ExportServices());
			__RETURN_IF_FAILED(_profilingTypes->ImportServices());
			__RETURN_IF_FAILED(_profilingTypes->EndInitialize());
			__RETURN_IF_FAILED(_profilingTypes->SubscribeEvents());

			return S_OK;
		}

		HRESULT Application::EndInitialize()
		{
			_invokeServer->LockChanges();
			return S_OK;
		}

		HRESULT Application::ShutdownInternal()
		{
			FlushData();
			return S_OK;
		}
	}
}