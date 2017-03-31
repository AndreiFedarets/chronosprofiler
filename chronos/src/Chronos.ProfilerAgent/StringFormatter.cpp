#include "StdAfx.h"
#include "StringFormatter.h"

std::vector<std::wstring>* CStringFormatter::Split(std::wstring string, std::wstring recordSeparator)
{
	std::vector<std::wstring>* list = new std::vector<std::wstring>();
	const __wchar* nativeSeparator = recordSeparator.c_str();
	__wchar* nativeString = const_cast<__wchar*>(string.c_str());
	__wchar* context = null;
	__wchar* token = wcstok_s(nativeString, nativeSeparator, &context);
	while(token != null)
	{
		std::wstring part;
		part.assign(token);
		list->push_back(part);
		token = wcstok_s(null, nativeSeparator, &context);
	}
	return list;
}

std::map<std::wstring, std::wstring>* CStringFormatter::Split(std::wstring string, std::wstring recordSeparator, std::wstring keyValueSeparator)
{
	std::map<std::wstring, std::wstring>* map = new std::map<std::wstring, std::wstring>();
	std::vector<std::wstring>* list = Split(string, recordSeparator);
	for(std::vector<std::wstring>::iterator i = list->begin(); i != list->end(); i++)
	{
		std::wstring variable = *i;
		std::vector<std::wstring>* variableParts = Split(variable, keyValueSeparator);
		if (variableParts->size() == static_cast<size_t>(2))
		{
			std::wstring key = variableParts->front();
			std::wstring value = variableParts->back();
			map->insert(std::pair<std::wstring, std::wstring>(key, value));
		}
	}
	return map;
}

__bool CStringFormatter::Equals(std::wstring* string1, std::wstring* string2)
{
	size_t size = string1->size();
	if (size != string2->size())
	{
		return false;
	}
	const __wchar* native1 = string1->c_str();
	const __wchar* native2 = string2->c_str();
	for (size_t i = 0; i < size; i++)
	{
		if (tolower(native1[i]) != tolower(native2[i]))
		{
			return false;
		}
	}
	return true;
}

__bool CStringFormatter::Contains(std::wstring* where, std::wstring* what)
{
	int index = where->find(what->c_str());
	return index >= 0;
}
