#pragma once
class CResultCode
{
public:
	enum
	{
		Ok = 0,
		UnknownError = 1,
		DaemonPoolNotAvailable = 2,
		SessionNotFound = 3,
		InvalidSessionSettings = 4,
		NotSupported = 7,
	};
};