#pragma once
#include "ClientBase.h"
#include "BaseStream.h"
#include "PipeNameFormatter.h"
#include "CurrentProcess.h"
#include "DaemonOperationCode.h"
#include "ResultCode.h"
#include "ByteMarshaler.h"
#include "Int32Marshaler.h"
#include "UInt32Marshaler.h"
#include "StringMarshaler.h"
#include "Exceptions.h"
#include "Int64Marshaler.h"

class CDaemonClient : public CClientBase
{
public:
	CDaemonClient(std::wstring* daemonToken);
	CBaseStream* QueryThreadStream();
	CBaseStream* QueryUnitStream(__uint unitType);
	__uint CallPageSize;
private:
	std::wstring* _sessionToken;
	__uint _currentThreadStreamIndex;
};

