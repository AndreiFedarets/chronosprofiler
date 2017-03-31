#include "StdAfx.h"
#include "DaemonClient.h"

CDaemonClient::CDaemonClient(std::wstring* sessionToken) : CClientBase(CPipeNameFormatter::GetDaemonServerPipeName(sessionToken))
{
	_sessionToken = sessionToken;
	_currentThreadStreamIndex = 0;
}

CBaseStream* CDaemonClient::QueryThreadStream()
{
	std::wstring pipeName = CPipeNameFormatter::GetDaemonThreadPipeName(_sessionToken, _currentThreadStreamIndex);
	CBaseStream* stream = new CNamedPipeClientStream(pipeName, GENERIC_WRITE, FILE_SHARE_READ,
												null, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL);
	_currentThreadStreamIndex++;
	return stream;
}

CBaseStream* CDaemonClient::QueryUnitStream(__uint unitType)
{
	std::wstring pipeName = CPipeNameFormatter::GetDaemonUnitPipeName(_sessionToken, unitType);
	CBaseStream* threadStream = new CNamedPipeClientStream(pipeName,  GENERIC_WRITE, FILE_SHARE_READ,
													null, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL);
	return threadStream;
}