using System.Collections.Generic;
using Chronos.Communication.Native;
using Chronos.Extensibility;
using System;
using Chronos.Storage;

namespace Chronos.Daemon
{
    internal sealed class ProfilingTypeManager
    {
        private readonly IProfilingTypeCollection _profilingTypes;
        private readonly List<ProfilingTypeFacade> _adapters;

        public ProfilingTypeManager(IProfilingTypeCollection profilingTypes, ProfilingTypeSettingsCollection profilingTypesSettings)
        {
            _profilingTypes = profilingTypes;
            _adapters = new List<ProfilingTypeFacade>();
            Initialize(profilingTypesSettings);
        }
        
        private void Initialize(ProfilingTypeSettingsCollection profilingTypesSettings)
        {
            foreach (IProfilingType profilingType in _profilingTypes)
            {
                ProfilingTypeSettings settings = profilingTypesSettings[profilingType.Definition.Uid];
                if (settings != null)
                {
                    ProfilingTypeFacade facade = new ProfilingTypeFacade(profilingType, settings);
                    _adapters.Add(facade);
                }
            }
            SortAdapters();
        }

        public void AttachStorage(IDataStorage storage)
        {
            foreach (ProfilingTypeFacade facade in _adapters)
            {
                facade.AttachStorage(storage);
            }
        }

        public void StartProfiling(IGatewayServer gatewayServer)
        {
            foreach (ProfilingTypeFacade facade in _adapters)
            {
                facade.StartProfiling();
            }
            foreach (ProfilingTypeFacade facade in _adapters)
            {
                IDataHandler dataHandler = facade.GetDataHandler();
                gatewayServer.Register(facade.DataMarker, dataHandler);
            }
        }

        public void StopProfiling()
        {
            foreach (ProfilingTypeFacade pair in _adapters)
            {
                pair.StopProfiling();
            }
        }

        public void LoadData()
        {
            foreach (ProfilingTypeFacade pair in _adapters)
            {
                pair.LoadData();
            }
        }

        public void SaveData()
        {
            foreach (ProfilingTypeFacade pair in _adapters)
            {
                pair.SaveData();
            }
        }

        public void ReloadData()
        {
            foreach (ProfilingTypeFacade pair in _adapters)
            {
                pair.ReloadData();
            }
        }

        public void ExportServices(IServiceContainer container)
        {
            foreach (ProfilingTypeFacade pair in _adapters)
            {
                pair.ExportServices(container);
            }
        }

        public void ImportServices(IServiceContainer container)
        {
            foreach (ProfilingTypeFacade pair in _adapters)
            {
                pair.ImportServices(container);
            }
        }

        private void SortAdapters()
        {
            List<ProfilingTypeFacade> sortedItems = new List<ProfilingTypeFacade>();
            do
            {
                if (_adapters.Count == 0)
                {
                    break;
                }
                int lastIndex = _adapters.Count - 1;
                ProfilingTypeFacade item = _adapters[lastIndex]; // Take last
                _adapters.RemoveAt(lastIndex);
                if (CanBeInitialized(item, sortedItems))
                {
                    sortedItems.Add(item);
                }
                else
                {
                    _adapters.Insert(0, item);
                }
            } 
            while (true);
            _adapters.Clear();
            _adapters.AddRange(sortedItems);
        }

        private bool CanBeInitialized(ProfilingTypeFacade item, List<ProfilingTypeFacade> sortedItems)
        {
            DependencyDefinitionCollection dependencies = item.Definition.Dependencies;
            //Item doesn't have dependencies
            if (dependencies.Count == 0)
            {
                return true;
            }
            foreach (DependencyDefinition dependency in dependencies)
            {
                //Some of the dependencies are still not in the list of sorted items - we cannot initialize current ProfilingType
                if (!ContainsItem(sortedItems, dependency))
                {
                    return false;
                }
            }
            return true;
        }

        private bool ContainsItem(List<ProfilingTypeFacade> sortedItems, DependencyDefinition dependency)
        {
            foreach (ProfilingTypeFacade item in sortedItems)
            {
                if (item.Definition.Uid == dependency.Uid)
                {
                    return true;
                }
            }
            return false;
        }

    }
}
