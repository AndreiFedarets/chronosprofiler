#pragma once
#include "BaseStream.h"
#include "TypeSize.h"

class CInt64Marshaler
{
public:
	static void Marshal(__long value, CBaseStream* stream);
	static __long Demarshal(CBaseStream* stream);
};

