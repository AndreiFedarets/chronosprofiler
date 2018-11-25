#include "stdafx.h"
#include "Chronos.Agent.h"
#include <Objbase.h>

namespace Chronos
{
	namespace Agent
	{
		HRESULT Converter::TryConvertStringToGuid(__string value, __guid* result)
		{
			value = NormalizeGuidString(value);
			return CLSIDFromString(value.c_str(), result);
		}

		__guid Converter::ConvertStringToGuid(__string value)
		{
			__guid result;
			value = NormalizeGuidString(value);
			CLSIDFromString(value.c_str(), &result);
			return result;
		}

		__string Converter::NormalizeGuidString(__string value)
		{
			__wchar start = value.at(0);
			if (start == '{')
			{
				return value;
			}
			else
			{
				__string normalized = L"";
				normalized.append(L"{");
				normalized.append(value);
				normalized.append(L"}");
				return normalized;
			}
		}

		__string Converter::ConvertGuidToString(__guid value)
		{
			BSTR buffer;
			StringFromCLSID(value, &buffer);
			return __string(buffer);
		}

		__string Converter::ConvertGuidToString(__guid value, __wchar code)
		{
			BSTR buffer;
			StringFromCLSID(value, &buffer);
			__string raw = __string(buffer);
			__string result;
			if (code == 'N')
			{
				for (__uint i = 0; i < raw.size(); ++i)
				{
					__wchar c = raw[i];
					if (c != '-' && c != '{' && c != '}')
					{
						result += c;
					}
				}
			}
			return result;
		}

		__string Converter::ConvertIntToString(__int value)
		{
			__wchar buffer[20];
			_itow_s(value, buffer, 10);
			return __string(buffer);
		}

		__string Converter::ConvertLongToString(__ulong value)
		{
			__wchar buffer[30];
			_ltow_s(value, buffer, 10);
			return __string(buffer);
		}

		std::string Converter::ConvertStringToStringA(__string value)
		{
			std::string str(value.begin(), value.end());
			return str;
		}
	}
}