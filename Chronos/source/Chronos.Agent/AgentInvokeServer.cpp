#include "stdafx.h"
#include "Chronos.Agent.h"

namespace Chronos
{
	namespace Agent
	{
		AgentInvokeServer::AgentInvokeServer()
			: _callbacks(new std::map<__guid, AgentInvokeCallback>()), _locked(false), _invokeServerThread(null)
		{
		}

		AgentInvokeServer::~AgentInvokeServer()
		{
			__FREEOBJ(_invokeServerThread);
			__FREEOBJ(_stream);
			__FREEOBJ(_callbacks);
		}
		
		void AgentInvokeServer::StartMessagesProcessing(void* parameter)
		{
			while (true)
			{
				ReadAndInvoke();
			}
		}

		HRESULT AgentInvokeServer::Initialize(IStreamFactory* factory, GUID agentApplicationUid)
		{
			HRESULT result = factory->CreateAgentInvokeStream(&_stream, agentApplicationUid);
			ThisCallback<AgentInvokeServer>* callback = new ThisCallback<AgentInvokeServer>(this, &AgentInvokeServer::StartMessagesProcessing);
			_invokeServerThread = new SingleCoreThread(callback);
			_invokeServerThread->Start(this);
			return result;
		}

		void AgentInvokeServer::LockChanges()
		{
			_locked = true;
		}

		__bool AgentInvokeServer::RegisterCallback(__guid operationId, AgentInvokeCallback callback)
		{
			if (_locked)
			{
				return false;
			}
			std::map<__guid, AgentInvokeCallback>::iterator i = _callbacks->find(operationId);
			if (i != _callbacks->end())
			{
				return false;
			}
			_callbacks->insert(std::pair<__guid, AgentInvokeCallback>(operationId, callback));
			return true;
		}

		void AgentInvokeServer::ReadAndInvoke()
		{
			Lock lock(&_criticalSection);
			if (_stream->WaitForConnection())
			{
				__guid operationId = Marshaler::DemarshalGuid(_stream);
				Buffer* argumentsBuffer = Marshaler::DemarshalBuffer(_stream);
				
				AgentInvokeCallback callback = null;
				std::map<__guid, AgentInvokeCallback>::iterator i = _callbacks->find(operationId);
				if (i != _callbacks->end())
				{
					callback = i->second;
				}
				Buffer* returnBuffer = null;
				__int result = 0;
				if (callback == null)
				{
					result = E_NOTIMPL;
				}
				else
				{
					result = callback(argumentsBuffer, &returnBuffer);
				}
				Marshaler::MarshalInt(result, _stream);
				if (result == 0 && returnBuffer != null)
				{
					Marshaler::MarshalBuffer(returnBuffer, _stream);
				}
				else
				{
					Marshaler::MarshalInt(0, _stream);
				}
				__FREEOBJ(argumentsBuffer);
				__FREEOBJ(returnBuffer);
				_stream->Disconnect();
			}
		}

		const __guid AgentInvokeServer::ServiceToken = Converter::ConvertStringToGuid(L"{8E7B6855-E3FE-4279-879D-67A62DEC92C1}");
	}
}