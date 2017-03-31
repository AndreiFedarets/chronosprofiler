#pragma once
class CStringFormatter
{
public:
	static std::vector<std::wstring>* Split(std::wstring string, std::wstring recordSeparator);
	static std::map<std::wstring, std::wstring>* Split(std::wstring string, std::wstring recordSeparator, std::wstring keyValueSeparator);
	static __bool Equals(std::wstring* string1, std::wstring* string2);
	static __bool Contains(std::wstring* where, std::wstring* what);
};