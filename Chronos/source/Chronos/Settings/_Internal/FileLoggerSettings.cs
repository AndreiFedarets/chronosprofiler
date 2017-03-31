using System.Xml.Linq;

namespace Chronos.Settings
{
    internal sealed class FileLoggerSettings : LoggerSettings, IFileLoggerSettings
    {
        private const string LogsDirectoryElementName = "LogsDirectory";

        public FileLoggerSettings(XElement element)
            : base(element)
        {
            LogsDirectory = new DirectorySettings(Element.Element(LogsDirectoryElementName));
        }

        public IDirectorySettings LogsDirectory { get; private set; }
    }
}
