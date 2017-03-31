#include "StdAfx.h"
#include "Chronos.Agent.Internal.h"

namespace Chronos
{
	namespace Agent
	{
		__string Formatter::Format(__string format, ...)
		{
			const size_t bufferLength = 1024;
			__wchar buffer[bufferLength] = {0};
			va_list arglist;
			_crt_va_start(arglist, format);
			int returnCount = _vswprintf_c_l(buffer, bufferLength, format.c_str(), NULL, arglist);
			_crt_va_end(arglist);
			__string result(buffer);
			return result;
		}
	}
}