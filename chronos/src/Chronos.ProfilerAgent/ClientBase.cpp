#include "StdAfx.h"
#include "ClientBase.h"

CClientBase::CClientBase(std::wstring pipeName) : _pipeName(pipeName), _stream(null)
{

}

CClientBase::~CClientBase(void)
{
	Dispose();
}

void CClientBase::Dispose()
{
	_stream->Dispose();
	__FREEOBJ(_stream);
}

bool CClientBase::Connect()
{
	if (_stream != null)
	{
		_stream->Dispose();
		__FREEOBJ(_stream);
	}
	CNamedPipeClientStream* stream = new CNamedPipeClientStream(_pipeName.c_str(),
					GENERIC_READ | GENERIC_WRITE,
					FILE_SHARE_READ | FILE_SHARE_WRITE,
					null, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL);
	if (!stream->Initialized())
	{
		__FREEOBJ(stream);
		return false;
	}
	_stream = stream;
	return true;
}

