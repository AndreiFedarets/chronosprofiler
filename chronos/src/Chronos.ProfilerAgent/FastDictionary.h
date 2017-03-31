#pragma once
#include "FastDictionaryContainer.h"

template<typename T>
class CFastDictionary
{
public:
	CFastDictionary<T>::CFastDictionary(void)
	{
		_entryPoint = new CFastDictionaryContainer<T>();
	}
	CFastDictionary<T>::~CFastDictionary(void)
	{
		__FREEOBJ(_entryPoint);
	}
	T CFastDictionary<T>::find(__uint key)
	{
		CFastDictionaryContainer<T>* current = _entryPoint;
		__short part = 0;

		part = (key >> 0) & 0x000000FF;
		current = current->Containers[part];
	
		part = (key >> 8) & 0x000000FF;
		current = current->Containers[part];
	
		part = (key >> 16) & 0x000000FF;
		current = current->Containers[part];
	
		part = (key >> 24) & 0x000000FF;
		current = current->Containers[part];

		return current->Content;
	}
	void CFastDictionary<T>::insert(__uint key, T value)
	{
		CFastDictionaryContainer<T>* current = _entryPoint;
		__short part = 0;

		part = (key >> 0) & 0x000000FF;
		if (current->Containers[part] == null)
		{
			current->Containers[part] = new CFastDictionaryContainer<T>();
		}
		current = current->Containers[part];
	
		part = (key >> 8) & 0x000000FF;
		if (current->Containers[part] == null)
		{
			current->Containers[part] = new CFastDictionaryContainer<T>();
		}
		current = current->Containers[part];
	
		part = (key >> 16) & 0x000000FF;
		if (current->Containers[part] == null)
		{
			current->Containers[part] = new CFastDictionaryContainer<T>();
		}
		current = current->Containers[part];
	
		part = (key >> 24) & 0x000000FF;
		if (current->Containers[part] == null)
		{
			current->Containers[part] = new CFastDictionaryContainer<T>();
		}
		current = current->Containers[part];
	
		current->Content = value;
	}
	void CFastDictionary<T>::erase(__uint key)
	{

	}
private:
	CFastDictionaryContainer<T>* _entryPoint;
};

