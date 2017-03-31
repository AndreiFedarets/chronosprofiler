#include "StdAfx.h"
#include "UInt32Marshaler.h"


void CUInt32Marshaler::Marshal(__uint value, CBaseStream* stream)
{
	stream->Write(&value, TypeSize::_INT32);
}

__uint CUInt32Marshaler::Demarshal(CBaseStream* stream)
{
	__uint value = 0;
	stream->Read(&value, TypeSize::_INT32);
	return value;
}
