#pragma once
#include "BaseStream.h"
#include "TypeSize.h"

class CInt32Marshaler
{
public:
	static void Marshal(__int value, CBaseStream* stream);
	static __int Demarshal(CBaseStream* stream);
};

