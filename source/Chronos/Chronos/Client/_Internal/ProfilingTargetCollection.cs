using Chronos.Extensibility;
using System;

namespace Chronos.Client
{
    internal sealed class ProfilingTargetCollection : ObservableDictionary<Guid, IProfilingTarget>, IProfilingTargetCollection
    {
        public ProfilingTargetCollection(ProfilingTargetDefinitionCollection definitions, IExportLoader exportLoader,
            string applicationCode, Host.ApplicationCollection hostApplications)
        {
            InitializeCollection(definitions, exportLoader, applicationCode, hostApplications);
        }

        public IProfilingTarget this[Guid uid]
        {
            get
            {
                lock (Lock)
                {
                    VerifyDisposed();
                    IProfilingTarget item;
                    if (!TryGetValue(uid, out item))
                    {
                        throw new ProfilingTargetNotFoundException(uid);
                    }
                    return item;
                }
            }
        }

        public bool Contains(Guid uid)
        {
            lock (Lock)
            {
                VerifyDisposed();
                return ContainsKey(uid);
            }
        }

        private void InitializeCollection(ProfilingTargetDefinitionCollection definitions, IExportLoader exportLoader,
                string applicationCode, Host.ApplicationCollection hostApplications)
        {
            foreach (ProfilingTargetDefinition definition in definitions)
            {
                ProfilingTarget item = new ProfilingTarget(definition, exportLoader, applicationCode, hostApplications);
                Add(item.Definition.Uid, item);
            }
        }
    }
}
