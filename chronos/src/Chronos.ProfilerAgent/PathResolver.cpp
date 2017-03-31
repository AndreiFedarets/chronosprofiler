#include "StdAfx.h"
#include "PathResolver.h"

std::wstring CPathResolver::GetFileName(std::wstring path)
{
	const size_t nameBufferLength = 2048;
	__wchar* fileName = new __wchar[nameBufferLength];
	__wchar* extension = new __wchar[nameBufferLength];
	_wsplitpath_s(path.c_str(), 0, 0, 0, 0, fileName, nameBufferLength, extension, nameBufferLength);
	std::wstring name;
	name.assign(fileName);
	name.append(extension);
	return name;
}

std::wstring CPathResolver::Combine(std::wstring path1, std::wstring path2)
{
	std::wstring result;
	result.append(path1);
	//TODO: replace with locale-depended separator
	result.append(L"\\");
	result.append(path2);
	return result;
}
