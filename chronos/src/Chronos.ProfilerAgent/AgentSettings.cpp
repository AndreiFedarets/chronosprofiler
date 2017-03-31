#include "StdAfx.h"
#include "AgentSettings.h"

CAgentSettings::CAgentSettings(void)
{
}


CAgentSettings::~CAgentSettings(void)
{
	
}

__bool CAgentSettings::Initialize(CBaseStream* stream)
{
	//Seesion Token
	SessionToken = CStringMarshaler::Demarshal(stream);
	//CallPageSize
	stream->Read(&CallPageSize, TypeSize::_INT32);
	//ThreadStreamsCount
	stream->Read(&ThreadStreamsCount, TypeSize::_INT32);
	return true;
}
