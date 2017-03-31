using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Chronos.Settings
{
    internal abstract class SettingsCollectionElement<T> : SettingsElement, IEnumerable<T>
    {
        private readonly List<T> _collection;

        protected SettingsCollectionElement(XElement element, string itemElementName)
            : base(element)
        {
            _collection = new List<T>();
            foreach (XElement child in element.Elements(itemElementName))
            {
                T item = CreateItem(child);
                _collection.Add(item);
            }
        }

        protected abstract T CreateItem(XElement element);

        public IEnumerator<T> GetEnumerator()
        {
            return _collection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
