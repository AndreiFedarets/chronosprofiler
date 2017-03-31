#pragma once
#include "BaseStream.h"
#include "TypeSize.h"

class CByteMarshaler
{
public:
	static void Marshal(__byte value, CBaseStream* stream);
	static __byte Demarshal(CBaseStream* stream);
};

