#include "StdAfx.h"
#include "Int32Marshaler.h"


void CInt32Marshaler::Marshal(__int value, CBaseStream* stream)
{
	stream->Write(&value, TypeSize::_INT32);
}

__int CInt32Marshaler::Demarshal(CBaseStream* stream)
{
	__int value = 0;
	stream->Read(&value, TypeSize::_INT32);
	return value;
}
