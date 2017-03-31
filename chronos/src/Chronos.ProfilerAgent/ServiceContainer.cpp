#include "ServiceContainer.h"


CServiceContainer::CServiceContainer(void)
{
}

CServiceContainer::~CServiceContainer(void)
{
}

bool CServiceContainer::ResolveService(GUID serviceToken, void** service)
{
	//CLock lock(&_monitor);
	//std::map<GUID, void*>::iterator i = _services.find(serviceToken);
	////container has no service registered as 'serviceToken'
	//if (i == _services.end())
	//{
	//	return false;
	//}
	//*service = i->second;
	return true;
}

bool CServiceContainer::RegisterService(GUID serviceToken, void* service)
{
	//CLock lock(&_monitor);
	//std::map<GUID, void*>::iterator i = _services.find(serviceToken);
	////container already has service registered as 'serviceToken'
	//if (i != _services.end())
	//{
	//	return false;
	//}
	//_services.insert(std::pair<GUID, void*>(serviceToken, service));
	return true;
}

bool CServiceContainer::UnregisterService(GUID serviceToken)
{
	//CLock lock(&_monitor);
	//std::map<GUID, void*>::iterator i = _services.find(serviceToken);
	////container has no service registered as 'serviceToken'
	//if (i == _services.end())
	//{
	//	return false;
	//}
	//_services.erase(serviceToken);
	return true;
}
