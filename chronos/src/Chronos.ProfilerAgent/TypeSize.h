#pragma once

class TypeSize
{
public:
	static const size_t _BOOL =  sizeof(__bool);
	static const size_t _BYTE = sizeof(__byte);
	static const size_t _INT32 = sizeof(__uint);
	static const size_t _INT64 = sizeof(__ulong);
	static const size_t _WCHAR = sizeof(__wchar);
};