using Chronos.Extensibility;
using System;

namespace Chronos.Client
{
    internal sealed class ProductivityCollection : DictionaryChangedBase<Guid, IProductivity>, IProductivityCollection
    {
        public ProductivityCollection(ProductivityDefinitionCollection definitions, IExportLoader exportLoader,
            string applicationCode)
        {
            InitializeCollection(definitions, exportLoader, applicationCode);
        }

        public IProductivity this[Guid uid]
        {
            get
            {
                lock (Lock)
                {
                    VerifyDisposed();
                    IProductivity item;
                    if (!TryGetValue(uid, out item))
                    {
                        throw new ProductivityNotFoundException(uid);
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

        private void InitializeCollection(ProductivityDefinitionCollection definitions, IExportLoader exportLoader,
                string applicationCode)
        {
            foreach (ProductivityDefinition definition in definitions)
            {
                Productivity item = new Productivity(definition, exportLoader, applicationCode);
                Add(item.Definition.Uid, item);
            }
        }
    }
}
