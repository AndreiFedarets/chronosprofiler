#pragma once
#include "StdAfx.h"
#include "Units.h"
#include "Timer.h"
#include "Lock.h"
#include "Convert.h"
#include "TypeSize.h"
#include "MemoryStream.h"
#include "ProfilerController.h"

#ifdef LOCK_FREE_UNIT_MANAGER
#include "LockFreeFactory.h"

template<typename T>
class CUnitDictionaryEnumerator
{
public:
	CUnitDictionaryEnumerator<T>(CDynamicBlockFactory<T>* factory)
	{
		Current = null;
		_currentPage = factory->LastPage;
		_currentIndex = __min((__int)_currentPage->CurrentIndex, _currentPage->LastIndex);
		if (_currentIndex < 0)
		{
			_currentPage = _currentPage->PrevPage;
			if (_currentPage != null)
			{
				_currentIndex = __min((__int)_currentPage->CurrentIndex, _currentPage->LastIndex);
			}
		}
		if (_currentPage != null)
		{
			Current = &(_currentPage->Items[_currentIndex]);
		}
	}
	void MoveNext()
	{
		if (Current == null)
		{
			return;
		}
		_currentIndex--;
		if (_currentIndex < 0)
		{
			_currentPage = _currentPage->PrevPage;
			if (_currentPage == null)
			{
				Current = null;
				return;
			}
			else
			{
				_currentIndex = __min((__int)_currentPage->CurrentIndex, _currentPage->LastIndex);
			}
		}
		Current = &(_currentPage->Items[_currentIndex]);
	}
	T* Current;
private:
	CBlockPage<T>* _currentPage;
	__int _currentIndex;
};
#endif


template<typename T>
class CUnitManagerBase
{
public:
	CUnitManagerBase<T>(ICorProfilerInfo2* corProfilerInfo2)
		: _corProfilerInfo2(corProfilerInfo2), _revision(0)
	{
#ifdef LOCK_FREE_UNIT_MANAGER

#else
	_lastId = 0;
#endif
	}
	
	~CUnitManagerBase<T>()
	{
		__FREEOBJ(_stream);
	}

	void Connect(CProfilerController* profilerController)
	{
		_stream = profilerController->CreateUnitStream(GetUnitType());
	}

	T* Create(UINT_PTR managedId)
	{
		T* unit = null;
#ifdef LOCK_FREE_UNIT_MANAGER
		__uint id = 0;
		unit = _factory.Next(&id);
		unit->Initialize(id, managedId, CTimer::CurrentTime);
#else
		CLock lock(&_monitor);
		unit = new T(_lastId, managedId, CTimer::CurrentTime);
		_units.insert(std::pair<UINT_PTR, T*>(managedId, unit));
		_lastId++;
#endif
		return unit;
	}

	T* Get(UINT_PTR managedId)
	{
		T* unit = null;
#ifdef LOCK_FREE_UNIT_MANAGER
		CUnitDictionaryEnumerator<T> enumerator(&_factory);
		while((unit = enumerator.Current) != null)
		{
			if (unit->ManagedId == managedId && unit->Alive)
			{
				return unit;
			}
			enumerator.MoveNext();
		}
#else
		CLock lock(&_monitor);
		std::map<UINT_PTR, T*>::iterator i = _units.find(managedId);
		if (i == _units.end())
		{
			//__debugbreak();
			return null;
		}
		unit = i->second;
#endif
		return unit;
	}

	__int UnitExceptionFilter(__int code, struct _EXCEPTION_POINTERS *ep, __wchar* source)
	{
		_ASSERT(false);
		return 0;
	}

	__bool Contains(UINT_PTR managedId)
	{
		return Get(managedId) != null;
	}

	void CloseAll()
	{
#ifdef LOCK_FREE_UNIT_MANAGER
		CLock lock(&_monitor);
		__int revision = _revision;
		CUnitDictionaryEnumerator<T> enumerator(&_factory);
		T* unit = null;
		while((unit = enumerator.Current) != null)
		{
			if (unit->Alive)
			{
				unit->EndLifetime = CTimer::CurrentTime;
				unit->Revision = revision;
				unit->Alive = false;
			}
			enumerator.MoveNext();
		}
#else
		CLock lock(&_monitor);
		for (std::map<UINT_PTR, T*>::iterator i = _units.begin(); i != _units.end(); i++)
		{
			T* unit = i->second;
			unit->EndLifetime = CTimer::CurrentTime;
			_updates.push_back(unit);
		}
		_units.clear();
#endif
	}
	
	void Close(T* unit)
	{
		if (unit == null)
		{
			return;
		}
#ifdef LOCK_FREE_UNIT_MANAGER
		unit->Revision = _revision;
		unit->Alive = false;
		unit->EndLifetime = CTimer::CurrentTime;
#else
		CLock lock(&_monitor);
		unit->EndLifetime = CTimer::CurrentTime;
		_updates.push_back(unit);
		_units.erase(unit->ManagedId);
#endif
	}
	
	void Update(T* unit)
	{
#ifdef LOCK_FREE_UNIT_MANAGER
		unit->Revision = _revision;
#else
		CLock lock(&_monitor);
		_updates.push_back(unit);
#endif
	}
	
	virtual void Initialize(T* unit) = 0;

	virtual __uint GetUnitType() = 0;

	void virtual Serialize(T* unit, CBaseStream* stream)
	{
		//Id
		stream->Write(&(unit->Id), TypeSize::_INT32);
		//ManagedId
		stream->Write(&(unit->ManagedId), TypeSize::_INT64);
		//BeginLifetime
		stream->Write(&(unit->BeginLifetime), TypeSize::_INT32);
		//EndLifetime
		stream->Write(&(unit->EndLifetime), TypeSize::_INT32);
		//Name
		CStringMarshaler::Marshal(&(unit->Name), stream);
		//Revision (compatibility)
		__int revision = unit->Revision;
		stream->Write(&revision, TypeSize::_INT32);
	}

#ifdef LOCK_FREE_UNIT_MANAGER
	std::list<T>* GetUpdates()
	{
		__int revision = (__int)InterlockedIncrement(&_revision) - 1;
		CLock lock(&_monitor);
		std::list<T>* updates = new std::list<T>();
		CUnitDictionaryEnumerator<T> enumerator(&_factory);
		T* unit = null;
		while((unit = enumerator.Current) != null)
		{
			T copy = *unit;
			if (copy.Revision == revision)
			{
				updates->push_back(copy);
			}
			enumerator.MoveNext();
		}
		return updates;
	}
#endif

	void Flush()
	{
#ifdef LOCK_FREE_UNIT_MANAGER
		std::list<T>* updates = GetUpdates();
		size_t size = updates->size();
		if (size != 0)
		{
			CMemoryStream memoryStream;
			memoryStream.Write(&size, TypeSize::_INT32);
			for (std::list<T>::iterator i = updates->begin(); i != updates->end(); i++)
			{
				T* unit = &(*i);
				Serialize(unit, &memoryStream);
			}
			_stream->Write(memoryStream.ToArray(), memoryStream.GetLength());
		}
		__FREEOBJ(updates);
#else
		CLock lock(&_monitor);
		__uint size = _updates.size();
		if (size == 0)
		{
			return;
		}
		CMemoryStream memoryStream;
		memoryStream.Write(&size, TypeSize::_INT32);
		for (std::list<T*>::iterator i = _updates.begin(); i != _updates.end(); i++)
		{
			Serialize(*i, &memoryStream);
		}
		_stream->Write(memoryStream.ToArray(), memoryStream.GetLength());
		_updates.clear();
#endif
	}

protected:
	ICorProfilerInfo2* _corProfilerInfo2;
	volatile long _revision;
	CMonitor _monitor;
	CBaseStream* _stream;
#ifdef LOCK_FREE_UNIT_MANAGER
	CDynamicBlockFactory<T> _factory;
#else
	std::map<UINT_PTR, T*> _units;
	std::list<T*> _updates;
	volatile long _lastId;
#endif
};
