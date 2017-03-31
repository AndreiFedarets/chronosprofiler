#include "stdafx.h"
#include "Chronos.Agent.h"

namespace Chronos
{
	namespace Agent
	{
		SimpleCallback::SimpleCallback(void (*callbackFunction)(void*))
		{
			_callbackFunction = callbackFunction;
		}

		SimpleCallback::SimpleCallback(SimpleCallback& callback)
		{
			_callbackFunction = callback._callbackFunction;
		}

		void SimpleCallback::Call(void* parameter)
		{
			_callbackFunction(parameter);
		}
	}
}