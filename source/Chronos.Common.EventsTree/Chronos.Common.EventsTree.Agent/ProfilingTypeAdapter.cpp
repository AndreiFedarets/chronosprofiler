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
				ProfilingTypeAdapter::ProfilingTypeAdapter(void)
				{
					_settings = null;
					_eventsMaxDepth = 0;
					_eventsBufferSize = 0;
					_dataMarker = 0;
				}

				ProfilingTypeAdapter::~ProfilingTypeAdapter(void)
				{
					__FREEOBJ(_loggers);
					//EventsTreeLoggerCollection::DestroyInstance();
				}
				
				HRESULT ProfilingTypeAdapter::BeginInitialize(ProfilingTypeSettings* settings)
				{
					//Initialize local settings
					_settings = settings;
					_loggers = new EventsTreeLoggerCollection();
					__RETURN_IF_FAILED(settings->GetDataMarker(&_dataMarker));
					//TODO: implement and read from settings
					//get EventsMaxDepth
					DynamicSettingBlock* eventsMaxDepthBlock = settings->GetSettingBlock(EventsMaxDepthIndex);
					_eventsMaxDepth = eventsMaxDepthBlock->AsUShort();
					//get EventsBufferSize
					DynamicSettingBlock* eventsBufferSizeBlock = settings->GetSettingBlock(EventsBufferSizeIndex);
					_eventsBufferSize = eventsBufferSizeBlock->AsUInt();
					return S_OK;
				}

				HRESULT ProfilingTypeAdapter::ExportServices(ServiceContainer* container)
				{
					__REGISTER_SERVICE(container, IEventsTreeLoggerCollection, _loggers);
					return S_OK;
				}

				HRESULT ProfilingTypeAdapter::ImportServices(ServiceContainer* container)
				{
					//Query shared services
					__RESOLVE_SERVICE(container, GatewayClient, _gatewayClient);
					__RESOLVE_SERVICE(container, ProfilingTimer, _profilingTimer);
					
					/*MessageBox(NULL, L"", L"", MB_OK);
					__byte eventType = 10;
					__uint unit = 12345678;
					__uint time = 45678;
					__short count = 10000;
					__uint iterations = 100000;


					EventEnter2* stack2 = new EventEnter2[count];
					__uint struct2Time = _profilingTimer->CurrentTime;
					for (__uint c = 0; c < iterations; c++)
					{
						for (__ushort i = 0; i < count; i++)
						{
							EventEnter2 eventEnter;
							eventEnter.EventType = eventType;
							eventEnter.Unit = unit;
							eventEnter.BeginTime = time;
							stack2[i] = eventEnter;
						}
					}
					struct2Time = _profilingTimer->CurrentTime - struct2Time;

					EventEnter* stack = new EventEnter[count];
					__uint structTime = _profilingTimer->CurrentTime;
					for (__uint c = 0; c < iterations; c++)
					{
						for (__ushort i = 0; i < count; i++)
						{
							EventEnter eventEnter;
							eventEnter.EventType = eventType;
							eventEnter.Unit = unit;
							eventEnter.BeginTime = time;
							stack[i] = eventEnter;
						}
					}
					structTime = _profilingTimer->CurrentTime - structTime;

					__byte* buffer = new __byte[count * sizeof(EventEnter)];
					__uint bufferTime = _profilingTimer->CurrentTime;
					for (__uint c = 0; c < iterations; c++)
					{
						__byte* current = buffer;
						for (__ushort i = 0; i < count; i++)
						{
							*current = eventType;
							current += 1;

							*(__uint*)current = unit;
							current += 4;

							*(__uint*)current = time;
							current += 4;
						}
					}
					bufferTime = _profilingTimer->CurrentTime - bufferTime;

					MessageBox(NULL, Converter::ConvertIntToString(struct2Time).c_str(), L"struct2", MB_OK);
					MessageBox(NULL, Converter::ConvertIntToString(structTime).c_str(), L"struct", MB_OK);
					MessageBox(NULL, Converter::ConvertIntToString(bufferTime).c_str(), L"buffer", MB_OK);*/
					return S_OK;
				}

				HRESULT ProfilingTypeAdapter::EndInitialize()
				{
					//Initialize local services
					_loggers->Initialize(_gatewayClient, _profilingTimer, _dataMarker, _eventsBufferSize, _eventsMaxDepth);
					return S_OK;
				}

				HRESULT ProfilingTypeAdapter::SubscribeEvents()
				{
					return S_OK;
				}
			
				HRESULT ProfilingTypeAdapter::FlushData()
				{
					return S_OK;
				}

				const __guid ProfilingTypeAdapter::EventsMaxDepthIndex = Converter::ConvertStringToGuid(L"{4D411B84-7536-441E-8A6E-5839F055119B}");
				const __guid ProfilingTypeAdapter::EventsBufferSizeIndex = Converter::ConvertStringToGuid(L"{1471ECF1-7670-4C4D-B0F8-DB4540003D9A}");
			}
		}
	}
}
