#pragma once

#define ThreadLoggerContainerSize 0xFF

template<typename T>
struct CFastDictionaryContainer
{
	CFastDictionaryContainer<T>::CFastDictionaryContainer()
	{
		Content = null;
		for (__int i = 0; i <= ThreadLoggerContainerSize; i++)
		{
			Containers[i] = null;
		}
	}
	CFastDictionaryContainer<T>::~CFastDictionaryContainer()
	{
		__FREEOBJ(Content);
		for (__int i = 0; i <= ThreadLoggerContainerSize; i++)
		{
			__FREEOBJ(Containers[i]);
			Containers[i] = null;
		}
	}
	CFastDictionaryContainer<T>* Containers[ThreadLoggerContainerSize];
	T Content;
};