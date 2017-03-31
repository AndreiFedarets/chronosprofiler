#pragma once
class CMonitor
{
public:
	CMonitor(void);
	~CMonitor(void);
	void Lock();
	void Unlock();
private:
	CRITICAL_SECTION _criticalSection;
};

