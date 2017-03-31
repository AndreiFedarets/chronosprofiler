#include "stdafx.h"
#include "Chronos.Common.EventsTree.Agent.Internal.h"

namespace Chronos
{
	namespace Agent
	{
		namespace Common
		{
			namespace EventsTree
			{
				void EventsTreeLoggerCollection_LogEventEnter(__byte eventType, __uint unit)
				{
					//EventsTreeLoggerCollection::Instance->LogEventEnter(eventType, unit);
					EventsTreeLogger* logger = EventsTreeLoggerCollection::Instance->GetOrCreateLogger();
					logger->Enter(eventType, unit);
				}

				void EventsTreeLoggerCollection_LogEventLeave(__byte eventType, __uint unit)
				{
					//EventsTreeLoggerCollection::Instance->LogEventLeave(eventType, unit);
					EventsTreeLogger* logger = EventsTreeLoggerCollection::Instance->GetOrCreateLogger();
					logger->Leave(eventType, unit);
				}

				void EventsTreeLoggerCollection_LogEventEnterLeave(__byte eventType, __uint unit)
				{
					//EventsTreeLoggerCollection::Instance->LogEventEnterLeave(eventType, unit);
					EventsTreeLogger* logger = EventsTreeLoggerCollection::Instance->GetOrCreateLogger();
					logger->EnterLeave(eventType, unit);
				}

				/*void EventsTreeLoggerCollection_LogEventEnter(__uint osThreadId, __byte eventType, __uint unit)
				{
					EventsTreeLoggerCollection::Instance->LogEventEnter(osThreadId, eventType, unit);
				}

				void EventsTreeLoggerCollection_LogEventLeave(__uint osThreadId, __byte eventType, __uint unit)
				{
					EventsTreeLoggerCollection::Instance->LogEventLeave(osThreadId, eventType, unit);
				}

				void EventsTreeLoggerCollection_LogEventEnterLeave(__uint osThreadId, __byte eventType, __uint unit)
				{
					EventsTreeLoggerCollection::Instance->LogEventEnterLeave(osThreadId, eventType, unit);
				}*/


				EventsTreeLoggerCollection::EventsTreeLoggerCollection(void)
				{
					Instance = this;
					_gatewayClient = null;
					_dataMarker = 0;
					_profilingTimer = null;
					_eventsBufferSize = 0;
					_eventsMaxDepth = 0;
					_lastThreadUid = 0;
				}
		
				EventsTreeLoggerCollection::~EventsTreeLoggerCollection(void)
				{
					Instance = null;
				}

				HRESULT EventsTreeLoggerCollection::Initialize(GatewayClient* gatewayClient, ProfilingTimer* profilingTimer, __byte dataMarker, __uint eventsBufferSize, __int eventsMaxDepth)
				{
					_gatewayClient = gatewayClient;
					_dataMarker = dataMarker;
					_profilingTimer = profilingTimer;
					_eventsBufferSize = eventsBufferSize;
					_eventsMaxDepth = eventsMaxDepth;
					return S_OK;
				}
		
				EventsTreeLogger* EventsTreeLoggerCollection::GetOrCreateLogger()
				{
					EventsTreeLogger* logger = CurrentLogger;
					if (logger == null)
					{
						__uint threadOsId = GetCurrentThreadId();
						Lock lock(&_criticalSection);
						std::map<__uint, EventsTreeLogger*>::iterator i = _loggers.find(threadOsId);
						if (i == _loggers.end())
						{
							__uint threadUid = InterlockedIncrement(&_lastThreadUid);
							logger = new EventsTreeLogger(_gatewayClient, _profilingTimer, _dataMarker, threadUid, threadOsId, _eventsBufferSize, _eventsMaxDepth);
							_loggers.insert(std::pair<__uint, EventsTreeLogger*>(threadOsId, logger));
						}
						else
						{
							logger = i->second;
						}
						CurrentLogger = logger;
					}
					return logger;
				}
				
				EventsTreeLogger* EventsTreeLoggerCollection::GetOrCreateLogger(__uint threadOsId)
				{
					EventsTreeLogger* logger = null;
					Lock lock(&_criticalSection);
					std::map<__uint, EventsTreeLogger*>::iterator i = _loggers.find(threadOsId);
					if (i == _loggers.end())
					{
						__uint threadUid = InterlockedIncrement(&_lastThreadUid);
						logger = new EventsTreeLogger(_gatewayClient, _profilingTimer, _dataMarker, threadUid, threadOsId, _eventsBufferSize, _eventsMaxDepth);
						_loggers.insert(std::pair<__uint, EventsTreeLogger*>(threadOsId, logger));
					}
					else
					{
						logger = i->second;
					}
					return logger;
				}

				void EventsTreeLoggerCollection::DestroyLogger()
				{
					__uint threadOsId = GetCurrentThreadId();
					Lock lock(&_criticalSection);
					std::map<__uint, EventsTreeLogger*>::iterator i = _loggers.find(threadOsId);
					if (i != _loggers.end())
					{
						EventsTreeLogger* logger = i->second;
						__FREEOBJ(logger);
						_loggers.erase(threadOsId);
					}
					CurrentLogger = null;
				}

				void EventsTreeLoggerCollection::LogEventEnter(__byte eventType, __uint unit)
				{
					EventsTreeLogger* logger = GetOrCreateLogger();
					logger->Enter(eventType, unit);
				}

				void EventsTreeLoggerCollection::LogEventLeave(__byte eventType, __uint unit)
				{
					EventsTreeLogger* logger = GetOrCreateLogger();
					logger->Leave(eventType, unit);
				}

				void EventsTreeLoggerCollection::LogEventEnterLeave(__byte eventType, __uint unit)
				{
					EventsTreeLogger* logger = GetOrCreateLogger();
					logger->EnterLeave(eventType, unit);
				}
				
				void EventsTreeLoggerCollection::LogEventEnter(__uint threadOsId, __byte eventType, __uint unit)
				{
					EventsTreeLogger* logger = GetOrCreateLogger(threadOsId);
					logger->Enter(eventType, unit);
				}

				void EventsTreeLoggerCollection::LogEventLeave(__uint threadOsId, __byte eventType, __uint unit)
				{
					EventsTreeLogger* logger = GetOrCreateLogger(threadOsId);
					logger->Leave(eventType, unit);
				}

				void EventsTreeLoggerCollection::LogEventEnterLeave(__uint threadOsId, __byte eventType, __uint unit)
				{
					EventsTreeLogger* logger = GetOrCreateLogger(threadOsId);
					logger->EnterLeave(eventType, unit);
				}
				
				EventsTreeLoggerCollection* EventsTreeLoggerCollection::Instance = null;
				EventsTreeLogger* EventsTreeLoggerCollection::CurrentLogger = null;
				const __guid IEventsTreeLoggerCollection::ServiceToken = Converter::ConvertStringToGuid(L"{D47A2680-33AF-4BB9-BBBA-22BB177B39D7}");
			}
		}
	}
}