#include "stdafx.h"
#include "Chronos.Agent.h"

namespace Chronos
{
	namespace Agent
	{
		DataHandlerRouter::DataHandlerRouter(DataHandlerCallback callback)
		{
			_callback = callback;
		}

		DataHandlerRouter::~DataHandlerRouter()
		{
			_callback = null;
		}

		__bool DataHandlerRouter::HandlePackage(__byte* data, __uint size)
		{
			return _callback(data, size);
		}
	}
}