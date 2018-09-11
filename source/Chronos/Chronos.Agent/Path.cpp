#include "stdafx.h"
#include "Chronos.Agent.h"

namespace Chronos
{
	namespace Agent
	{
		__string Path::GetFileName(__string path)
		{
			__wchar fileName[2048];
			__wchar extension[128];
			_wsplitpath_s(path.c_str(), 0, 0, 0, 0, fileName, 2048, extension, 128);
			__string name;
			name.assign(fileName);
			name.append(extension);
			return name;
		}

		__string Path::Combine(__string path1, __string path2)
		{
			__string result;
			result.append(path1);
			//TODO: check if path1 ends with "\\" and if not - append
			result.append(L"\\");
			result.append(path2);
			return result;
		}
	}
}