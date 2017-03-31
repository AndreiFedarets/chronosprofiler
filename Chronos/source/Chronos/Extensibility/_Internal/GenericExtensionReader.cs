using System;
using System.IO;

namespace Chronos.Extensibility
{
    internal sealed class GenericExtensionReader : IExtensionReader
    {
        private XmlExtensionReader _xmlExtensionReader;
        private JsonExtensionReader _jsonExtensionReader;

        private IExtensionReader XmlExtensionReader
        {
            get { return _xmlExtensionReader ?? (_xmlExtensionReader = new XmlExtensionReader()); }
        }

        private IExtensionReader JsonExtensionReader
        {
            get { return _jsonExtensionReader ?? (_jsonExtensionReader = new JsonExtensionReader()); }
        }

        public ExtensionDefinition ReadExtension(string extensionPath)
        {
            string fileExtension = Path.GetExtension(extensionPath);
            ExtensionDefinition definition;
            if (string.Equals(fileExtension, Constants.XChronexExtension, StringComparison.InvariantCultureIgnoreCase))
            {
                definition = XmlExtensionReader.ReadExtension(extensionPath);
            }
            else if (string.Equals(fileExtension, Constants.JChronexExtension, StringComparison.InvariantCultureIgnoreCase))
            {
                definition = JsonExtensionReader.ReadExtension(extensionPath);
            }
            else
            {
                throw new TempException();
            }
            return definition;
        }
    }
}
