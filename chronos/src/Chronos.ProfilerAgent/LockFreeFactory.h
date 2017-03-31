#pragma once
#include "Monitor.h"
#include "Lock.h"

template<typename T>
class CBlockPage
{
public:
	CBlockPage<T>(__uint capacity, __int pageIndex)
	{
		Capacity = capacity;
		Items = new T[Capacity];
		NextPage = null;
		PrevPage = null;
		CurrentIndex = -1;
		Full = false;
		LastIndex = Capacity - 1;
		PageIndex = pageIndex;
	}
	~CBlockPage<T>()
	{
		__FREEARR(Items);
	}
	T* Next()
	{
		long currentIndex = InterlockedIncrement(&CurrentIndex);
		Full = currentIndex >= LastIndex;
		if (currentIndex > LastIndex)
		{
			return null;
		}
		T* item = &(Items[currentIndex]);
		return item;
	}
	T* Next(long* index)
	{
		long currentIndex = InterlockedIncrement(&CurrentIndex);
		Full = currentIndex >= LastIndex;
		if (currentIndex > LastIndex)
		{
			return null;
		}
		T* item = &(Items[currentIndex]);
		*index = currentIndex;
		return item;
	}
	CBlockPage<T>* NextPage;
	CBlockPage<T>* PrevPage;
	__bool Full;
	__uint Capacity;
	volatile long CurrentIndex;
	__int LastIndex;
	T* Items;
	__int PageIndex;
};

template<typename T>
class CDynamicBlockFactory
{
public:
	CDynamicBlockFactory<T>()
		: _onePageSize(ONE_PAGE_SIZE), _lastPageIndex(0)
	{
		FirstPage = new CBlockPage<T>(_onePageSize, _lastPageIndex);
		LastPage = FirstPage;
	}
	CDynamicBlockFactory<T>(__uint onePageSize)
		: _onePageSize(onePageSize), _lastPageIndex(0)
	{
		FirstPage = new CBlockPage<T>(_onePageSize, _lastPageIndex);
		LastPage = FirstPage;
	}
	~CDynamicBlockFactory<T>()
	{
		CBlockPage<T>* currentPage = FirstPage;
		while (currentPage != null)
		{
			CBlockPage<T>* nextPage = currentPage->NextPage;
			__FREEOBJ(currentPage);
			currentPage = nextPage;
		}
		FirstPage = null;
		LastPage = null;
	}
	T* Next()
	{
		while (true)
		{
			CBlockPage<T>* currentPage = LastPage;
			if (currentPage->Full)
			{
				currentPage = CreatePage();
			}
			T* item = currentPage->Next();
			if (item != null)
			{
				return item;
			}
		}
	}
	T* Next(__uint* globalIndex)
	{
		while (true)
		{
			CBlockPage<T>* currentPage = LastPage;
			if (currentPage->Full)
			{
				currentPage = CreatePage();
			}
			long localIndex = 0;
			T* item = currentPage->Next(&localIndex);
			if (item != null)
			{
				*globalIndex = (currentPage->PageIndex * _onePageSize + localIndex);
				return item;
			}
		}
	}
	CBlockPage<T>* FirstPage;
	CBlockPage<T>* LastPage;
private:
	CBlockPage<T>* CreatePage()
	{
		CLock lock(&_monitor);
		if (LastPage->Full)
		{
			_lastPageIndex++;
			CBlockPage<T>* newpage = new CBlockPage<T>(_onePageSize, _lastPageIndex);
			LastPage->NextPage = newpage;
			newpage->PrevPage = LastPage;
			LastPage = newpage;
		}
		return LastPage;
	}
	volatile __int _lastPageIndex;
	CMonitor _monitor;
	__uint _onePageSize;
	static const __uint ONE_PAGE_SIZE = 16384;
};
//=========================================================
template<typename T>
class CSLFFactory
{
public:
	CSLFFactory<T>(__int capacity)
	{
		_items = new T[capacity];
		_currentIndex = 0;
	}
	~CSLFFactory<T>()
	{
		__FREEARR(_items);
	}
	T* Next()
	{
		T* item = &(_items[_currentIndex]);
		_currentIndex++;
		return item;
	}
private:
	T* _items;
	__int _currentIndex;
};

