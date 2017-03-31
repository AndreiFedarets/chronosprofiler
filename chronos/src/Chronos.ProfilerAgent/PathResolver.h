#pragma once
class CPathResolver
{
public:
	static std::wstring GetFileName(std::wstring path);
	static std::wstring Combine(std::wstring path1, std::wstring path2);
};

