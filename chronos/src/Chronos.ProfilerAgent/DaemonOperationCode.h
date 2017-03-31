#pragma once
class CDaemonOperationCode
{
public:
	enum 
	{
		StartProfilingSession = 1,
		NotifyClientInitialized = 4,
		NotifyClientShutdown = 27,
	};
};