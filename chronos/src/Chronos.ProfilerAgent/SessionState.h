#pragma once
class CSessionState
{
public:
	enum
	{
		Closed = 0,
		Profiling = 1,
		Paused = 2,
		Decoding = 3,
	};
};