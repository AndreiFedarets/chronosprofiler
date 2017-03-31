#pragma once
#include "BaseStream.h"
#include "TypeSize.h"

class CUInt32Marshaler
{
public:
	static void Marshal(__uint value, CBaseStream* stream);
	static __uint Demarshal(CBaseStream* stream);
};

