#include "stdafx.h"
#include "Chronos.Agent.h"

namespace Chronos
{
	namespace Agent
	{
		ProfilingTypeSettingsCollection::ProfilingTypeSettingsCollection(void)
		{
		}

		ProfilingTypeSettingsCollection::~ProfilingTypeSettingsCollection(void)
		{
		}
		
		HRESULT ProfilingTypeSettingsCollection::ValidateAndSort()
		{
			__RETURN_IF_FAILED(Validate());
			__RETURN_IF_FAILED(Sort());
			return S_OK;
		}
		
		HRESULT ProfilingTypeSettingsCollection::Validate()
		{
			__vector<__guid> allDependenciesUids;
			__vector<__guid> allItemsUids;
			//Fill both collections
			for (__vector<ProfilingTypeSettings*>::iterator i = _items->begin(); i != _items->end(); ++i)
			{
				__guid itemUid;
				ProfilingTypeSettings* item = *i;
				if (!item->GetUid(&itemUid))
				{
					return E_FAIL;
				}
				//Item is already in the collection
				if (ContainsItem(&allItemsUids, itemUid))
				{
					return E_FAIL;
				}
				allItemsUids.push_back(itemUid);

				__vector<__guid> dependenciesUids;
				if (!item->GetDependencies(&dependenciesUids))
				{
					return E_FAIL;
				}
				for (__vector<__guid>::iterator j = dependenciesUids.begin(); j != dependenciesUids.end(); ++j)
				{
					__guid dependencyUid = *j;
					if (!ContainsItem(&allDependenciesUids, dependencyUid))
					{
						allDependenciesUids.push_back(dependencyUid);
					}
				}
			}
			//Verify that all dependencies are valid - presented in allItemsUids
			for (__vector<__guid>::iterator i = allDependenciesUids.begin(); i != allDependenciesUids.end(); ++i)
			{
				__guid dependencyUid = *i;
				if (!ContainsItem(&allItemsUids, dependencyUid))
				{
					return E_FAIL;
				}
			}
			return S_OK;
		}

		HRESULT ProfilingTypeSettingsCollection::Sort()
		{
			__vector<ProfilingTypeSettings*> sortedItems;
			do
			{
				if (_items->empty())
				{
					break;
				}
				ProfilingTypeSettings* item = _items->back();
				_items->pop_back();
				if (CanBeInitialized(item, &sortedItems))
				{
					sortedItems.push_back(item);
				}
				else
				{
					_items->insert(_items->begin(), item);
				}
			}
			while (true);

			//Re-fill items from sorted collection
			for (__vector<ProfilingTypeSettings*>::iterator i = sortedItems.begin(); i != sortedItems.end(); ++i)
			{
				ProfilingTypeSettings* item = *i;
				_items->push_back(item);
			}
			return S_OK;
		}
		
		__bool ProfilingTypeSettingsCollection::CanBeInitialized(ProfilingTypeSettings* item, __vector<ProfilingTypeSettings*>* sortedItems)
		{
			//Item doesn't have dependencies
			std::vector<__guid> dependencies;
			if (item->GetDependencies(&dependencies) && dependencies.empty())
			{
				return true;
			}
			for (std::vector<__guid>::iterator i = dependencies.begin(); i != dependencies.end(); ++i)
			{
				__guid dependency = (*i);
				//Some of the dependencies are still not in the list of sorted items - we cannot initialize current ProfilingType
				if (!ContainsItem(sortedItems, dependency))
				{
					return false;
				}
			}
			return true;
		}

		__bool ProfilingTypeSettingsCollection::ContainsItem(__vector<ProfilingTypeSettings*>* items, __guid itemUid)
		{
			for (std::vector<ProfilingTypeSettings*>::iterator i = items->begin(); i != items->end(); ++i)
			{
				ProfilingTypeSettings* item = (*i);
				__guid currentItemUid;
				if (item->GetUid(&currentItemUid) && currentItemUid == itemUid)
				{
					return true;
				}
			}
			return false;
		}
		
		__bool ProfilingTypeSettingsCollection::ContainsItem(__vector<__guid>* items, __guid itemUid)
		{
			for (std::vector<__guid>::iterator i = items->begin(); i != items->end(); ++i)
			{
				__guid currentItemUid = (*i);
				if (currentItemUid == itemUid)
				{
					return true;
				}
			}
			return false;
		}
	}
}