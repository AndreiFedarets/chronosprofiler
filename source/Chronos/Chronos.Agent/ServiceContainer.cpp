#include "stdafx.h"
#include "Chronos.Agent.h"

namespace Chronos
{
	namespace Agent
	{
		ServiceContainer::ServiceContainer(void)
			: _services(new std::map<__guid, void*>())
		{
		}

		ServiceContainer::~ServiceContainer(void)
		{
			__FREEOBJ(_services);
		}

		__bool ServiceContainer::IsRegistered(__guid serviceToken)
		{
			Lock lock(&_criticalSection);
			std::map<__guid, void*>::iterator i = _services->find(serviceToken);
			return (i != _services->end());
		}

		bool ServiceContainer::ResolveService(__guid serviceToken, void** service)
		{
			Lock lock(&_criticalSection);
			std::map<__guid, void*>::iterator i = _services->find(serviceToken);
			//container has no service registered as 'serviceToken'
			if (i == _services->end())
			{
				*service = null;
				return false;
			}
			*service = i->second;
			return true;
		}

		bool ServiceContainer::RegisterService(__guid serviceToken, void* service)
		{
			return RegisterService(serviceToken, service, false);
		}
		
		bool ServiceContainer::RegisterService(__guid serviceToken, void* service, __bool overrideExisting)
		{
			Lock lock(&_criticalSection);
			std::map<__guid, void*>::iterator i = _services->find(serviceToken);
			//container already has service registered as 'serviceToken'
			if (i != _services->end())
			{
				if (overrideExisting)
				{
					_services->erase(serviceToken);
				}
				else
				{
					return false;
				}
			}
			_services->insert(std::pair<__guid, void*>(serviceToken, service));
			return true;
		}

		bool ServiceContainer::UnregisterService(__guid serviceToken)
		{
			Lock lock(&_criticalSection);
			std::map<__guid, void*>::iterator i = _services->find(serviceToken);
			//container has no service registered as 'serviceToken'
			if (i == _services->end())
			{
				return false;
			}
			_services->erase(serviceToken);
			return true;
		}
	}
}