using System;
using Chronos.Extensibility;

namespace Chronos.Client
{
    internal sealed class FrameworkCollection : DictionaryChangedBase<Guid, IFramework>, IFrameworkCollection
    {
        private readonly FrameworkDefinitionCollection _definitions;
        private readonly IExportLoader _exportLoader;
        private readonly string _applicationCode;
        private readonly Host.ApplicationCollection _hostApplications;
        private IProfilingTypeCollection _profilingTypes;

        public FrameworkCollection(FrameworkDefinitionCollection definitions, IExportLoader exportLoader,
            string applicationCode, Host.ApplicationCollection hostApplications)
        {
            _definitions = definitions;
            _exportLoader = exportLoader;
            _applicationCode = applicationCode;
            _hostApplications = hostApplications;
        }

        public IFramework this[Guid uid]
        {
            get
            {
                lock (Lock)
                {
                    VerifyDisposed();
                    IFramework item;
                    if (!TryGetValue(uid, out item))
                    {
                        throw new FrameworkNotFoundException(uid);
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

        internal void SetupDependencies(IProfilingTypeCollection profilingTypes)
        {
            _profilingTypes = profilingTypes;
            InitializeCollection();
        }

        private void InitializeCollection()
        {
            foreach (FrameworkDefinition definition in _definitions)
            {
                Framework item = new Framework(definition, _exportLoader, _applicationCode, _profilingTypes, _hostApplications);
                Add(item.Definition.Uid, item);
            }
        }
    }
}
