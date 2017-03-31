#pragma once
#include "BaseStream.h"
#include "TypeSize.h"

class CStringMarshaler
{
public:
	static void Marshal(std::wstring* value, CBaseStream* stream);
	static std::wstring* Demarshal(CBaseStream* stream);
};

