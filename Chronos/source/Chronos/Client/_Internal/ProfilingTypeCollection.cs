using System;
using Chronos.Extensibility;

namespace Chronos.Client
{
    internal sealed class ProfilingTypeCollection : DictionaryChangedBase<Guid, IProfilingType>, IProfilingTypeCollection
    {
        private IFrameworkCollection _frameworks;
        private readonly ProfilingTypeDefinitionCollection _definitions;
        private readonly IExportLoader _exportLoader;
        private readonly string _applicationCode;
        private readonly Host.ApplicationCollection _hostApplications;

        public ProfilingTypeCollection(ProfilingTypeDefinitionCollection definitions, IExportLoader exportLoader,
            string applicationCode, Host.ApplicationCollection hostApplications)
        {
            _definitions = definitions;
            _exportLoader = exportLoader;
            _applicationCode = applicationCode;
            _hostApplications = hostApplications;
        }

        public IProfilingType this[Guid uid]
        {
            get
            {
                lock (Lock)
                {
                    VerifyDisposed();
                    IProfilingType item;
                    if (!TryGetValue(uid, out item))
                    {
                        throw new ProfilingTypeNotFoundException(uid);
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

        internal void SetupDependencies(IFrameworkCollection frameworks)
        {
            _frameworks = frameworks;
            InitializeCollection();
        }

        private void InitializeCollection()
        {
            foreach (ProfilingTypeDefinition definition in _definitions)
            {
                ProfilingType item = new ProfilingType(definition, _exportLoader, _applicationCode, _frameworks, _hostApplications);
                Add(item.Definition.Uid, item);
            }
        }
    }
}
