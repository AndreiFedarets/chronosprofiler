#include "StdAfx.h"
#include "ByteMarshaler.h"

void CByteMarshaler::Marshal(__byte value, CBaseStream* stream)
{
	stream->Write(&value, TypeSize::_BYTE);
}

__byte CByteMarshaler::Demarshal(CBaseStream* stream)
{
	__byte value = 0;
	stream->Read(&value, TypeSize::_BYTE);
	return value;
}
