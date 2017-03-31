#include "StdAfx.h"
#include "HostClient.h"

CHostClient::CHostClient() : CClientBase(CPipeNameFormatter::GetHostServerPipeName())
{ 
}
	
#pragma warning(push)
#pragma warning(disable:4101)
__bool CHostClient::GetConfigurationSettings(std::wstring* configurationToken, CConfigurationSettings* settings)
{
	try
	{
		if (!Connect())
		{
			return false;
		}
		CInt64Marshaler::Marshal(CHostOperationCode::GetConfigurationSettings, _stream);
		CStringMarshaler::Marshal(configurationToken, _stream);
		__long resultCode = CInt64Marshaler::Demarshal(_stream);
		if (resultCode != CResultCode::Ok)
		{
			std::wstring* message = CStringMarshaler::Demarshal(_stream);
			return false;
		}
		if (!settings->Initialize(_stream))
		{
			return false;
		}
	}
	catch(BrokenStreamException* exception)
	{
		return false;
	}
	return true;
}
#pragma warning(pop)

#pragma warning(push)
#pragma warning(disable:4101)
__bool CHostClient::StartProfilingSession(std::wstring* configurationToken, CAgentSettings* settings)
{
	try
	{
		if (!Connect())
		{
			return false;
		}
		__int currentProcessId = CCurrentProcess::GetProcessId();
		CInt64Marshaler::Marshal(CHostOperationCode::StartProfilingSession, _stream);
		CStringMarshaler::Marshal(configurationToken, _stream);
		CInt32Marshaler::Marshal(currentProcessId, _stream);
		CUInt32Marshaler::Marshal(CTimer::BeginTime, _stream);
		__long resultCode = CInt64Marshaler::Demarshal(_stream);
		if (resultCode != CResultCode::Ok)
		{
			std::wstring* message = CStringMarshaler::Demarshal(_stream);
			return false;
		}
		if (!settings->Initialize(_stream))
		{
			return false;
		}
	}
	catch(BrokenStreamException* exception)
	{
		return false;
	}
	return true;
}
#pragma warning(pop)
