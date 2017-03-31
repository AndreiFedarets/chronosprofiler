#include "StdAfx.h"
#include "Convert.h"


std::wstring CConvert::ToString(__int value)
{
	const size_t bufferSize = 16;
	__wchar nativeResult[bufferSize];
	_itow_s(value, nativeResult, bufferSize, 10);
	return nativeResult;
}

__int CConvert::ToInt(std::wstring value)
{
	__int result = _wtoi(value.c_str());
	return result;
}
