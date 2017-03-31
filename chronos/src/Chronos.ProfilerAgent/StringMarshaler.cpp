#include "StdAfx.h"
#include "StringMarshaler.h"

void CStringMarshaler::Marshal(std::wstring* value, CBaseStream* stream)
{
	__wchar* nativeValue = const_cast<__wchar*>(value->c_str());
	__uint size = static_cast<__uint>(value->size()) * TypeSize::_WCHAR;
	stream->Write(&size, TypeSize::_INT32);
	stream->Write(nativeValue, size);
}

std::wstring* CStringMarshaler::Demarshal(CBaseStream* stream)
{
	__uint size = 0;
	stream->Read(&size, 4);
	__uint valueLength = size / TypeSize::_WCHAR;
	valueLength += 1; // 1 for zero-wchar
	__uint valueBufferSize = valueLength * TypeSize::_WCHAR;
	__wchar* valueBuffer = new __wchar[valueLength];
	memset(valueBuffer, 0, valueBufferSize);
	stream->Read(valueBuffer, size);
	return new std::wstring(valueBuffer);
}
