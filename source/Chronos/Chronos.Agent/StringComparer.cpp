#include "stdafx.h"
#include "Chronos.Agent.h"
#include <Objbase.h>

namespace Chronos
{
	namespace Agent
	{
		bool StringComparer::Equals(__string* value1, __string* value2, __bool ignoreCase)
		{
			if (!ignoreCase)
			{
				return value1->compare(value2->c_str()) == 0;
			}
			if (value1->size() != value2->size())
			{
				return false;
			}
			return _wcsnicmp(value1->c_str(), value2->c_str(), value1->size()) == 0;
		}
	}
}