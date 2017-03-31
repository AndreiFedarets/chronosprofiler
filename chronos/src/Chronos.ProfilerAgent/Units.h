#pragma once
#include "StdAfx.h"


//================================================= CUnitType
class CUnitType
{
public:
	enum
	{
		AppDomain = 1,
		Assembly = 2,
		Module = 4,
		Class = 8,
		Function = 16,
		Thread = 32,
		Exception = 64,
		Process = 128,
		Callstack = 256,
		SqlRequest = 512,
	};
};

//================================================= CUnitBase
struct CUnitBase
{
	CUnitBase(__uint id, __ulong managedId, __uint beginLifetime)
		:Id(id), ManagedId(managedId), BeginLifetime(beginLifetime), EndLifetime(0), Revision(-1), OwnerThreadManagedId(-1), Alive(true) { }
	CUnitBase()
		:Id(0), ManagedId(0), BeginLifetime(0), EndLifetime(0), Revision(-1), OwnerThreadManagedId(-1), Alive(false) { }
	void Initialize(__uint id, __ulong managedId, __uint beginLifetime)
	{
		Id = id;
		ManagedId = managedId;
		BeginLifetime = beginLifetime;
		Alive = true;
	}
	__uint Id;
	__ulong ManagedId;
	std::wstring Name;
	__uint BeginLifetime;
	__uint EndLifetime;
	volatile __int Revision;
	__ulong OwnerThreadManagedId;
	__bool Alive;
};

//================================================= CResultUnitBase
struct CResultUnitBase : public CUnitBase
{
	CResultUnitBase(__uint id, __ulong managedId, __uint beginLifeTime)
		: CUnitBase(id, managedId, beginLifeTime), LoadResult(E_FAIL) { }
	CResultUnitBase()
		: LoadResult(E_FAIL) { }
	__int LoadResult;
};

//================================================= CAppDomainInfo
struct CAppDomainInfo : public CResultUnitBase
{
public:
	CAppDomainInfo(__uint id, __ulong managedId, __uint beginLifeTime)
		: CResultUnitBase(id, managedId, beginLifeTime) { }
	CAppDomainInfo() { }
};

//================================================= CAssemblyInfo
struct CAssemblyInfo : public CResultUnitBase
{
	CAssemblyInfo(__uint id, __ulong managedId, __uint beginLifeTime)
		: CResultUnitBase(id, managedId, beginLifeTime), IsExcluded(true), AppDomainManagedId(0) { }
	CAssemblyInfo()
		: IsExcluded(true), AppDomainManagedId(0) { }
	__bool IsExcluded;
	__ulong AppDomainManagedId;
};

//================================================= CModuleInfo
struct CModuleInfo : public CResultUnitBase
{
	CModuleInfo(__uint id, __ulong managedId, __uint beginLifeTime)
		: CResultUnitBase(id, managedId, beginLifeTime), AssemblyManagedId(0) { }
	CModuleInfo()
		: AssemblyManagedId(0) { }
	__ulong AssemblyManagedId;
};

//================================================= CClassInfo
struct CClassInfo : public CResultUnitBase
{
	CClassInfo(__uint id, __ulong managedId, __uint beginLifeTime)
		: CResultUnitBase(id, managedId, beginLifeTime), ModuleManagedId(0) { }
	CClassInfo()
		: ModuleManagedId(0) { }
	__ulong ModuleManagedId;
};

//================================================= CFunctionInfo
struct CFunctionInfo : public CUnitBase
{
	CFunctionInfo(__uint id, __ulong managedId, __uint beginLifeTime)
		: CUnitBase(id, managedId, beginLifeTime), AssemblyManagedId(0), ClassManagedId(0), IsTarget(false), JitCompiled(false), Hits(0), TotalTime(0), IsSqlEntryPoint(false) { }
	CFunctionInfo()
		: AssemblyManagedId(0), ClassManagedId(0), IsTarget(false), JitCompiled(false), Hits(0), TotalTime(0), IsSqlEntryPoint(false) { }
	__ulong AssemblyManagedId;
	__ulong ClassManagedId;
	__bool IsTarget;
	__bool JitCompiled;
	__uint Hits;
	__uint TotalTime;
	__bool IsSqlEntryPoint;
};

//================================================= CSqlRequestInfo
struct CSqlRequestInfo : public CUnitBase
{
	CSqlRequestInfo(__uint id, __ulong managedId, __uint beginLifeTime)
		: CUnitBase(id, managedId, beginLifeTime), Hits(0), TotalTime(0) { }
	CSqlRequestInfo()
		: Hits(0), TotalTime(0) { }
	__uint Hits;
	__uint TotalTime;
};
//================================================= CThreadInfo
struct CThreadInfo : public CUnitBase
{
	CThreadInfo(__uint id, __ulong managedId, __uint beginLifeTime)
		: CUnitBase(id, managedId, beginLifeTime), OsThreadId(0) { }
	CThreadInfo()
		: OsThreadId(0) { }
	__uint OsThreadId;
};

//================================================= CExceptionInfo
struct CExceptionInfo : public CUnitBase
{
	CExceptionInfo(__uint id, __ulong managedId, __uint beginLifeTime)
		: CUnitBase(id, managedId, beginLifeTime), ThreadId(0), ParentException(null), NestedException(null), Stack(new std::queue<CFunctionInfo*>()), Catcher(null), IsCatched(false) { }
	CExceptionInfo()
		: ThreadId(0), ParentException(null), NestedException(null), Stack(new std::queue<CFunctionInfo*>()), Catcher(null), IsCatched(false) { }
	__ulong ThreadId;
	CExceptionInfo* ParentException;
	CExceptionInfo* NestedException;
	std::wstring Message;
	std::queue<CFunctionInfo*>* Stack;
	CFunctionInfo* Catcher;
	__bool IsCatched;
};
