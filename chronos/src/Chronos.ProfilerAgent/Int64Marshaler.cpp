#include "StdAfx.h"
#include "Int64Marshaler.h"


void CInt64Marshaler::Marshal(__long value, CBaseStream* stream)
{
	stream->Write(&value, TypeSize::_INT64);
}

__long CInt64Marshaler::Demarshal(CBaseStream* stream)
{
	__long value = 0;
	stream->Read(&value, TypeSize::_INT64);
	return value;
}
