using System.Xml.Linq;

namespace Chronos.Settings
{
    internal sealed class ApplicationSettings : IApplicationSettings
    {
        private const string LoggingElementName = "Logging";
        private const string CrashDumpElementName = "CrashDump";
        private const string ExtensionsElementName = "Extensions";
        private const string ProfilingResultsElementName = "ProfilingResults";
        private const string HostConnectionsElementName = "HostConnections";

        public ApplicationSettings()
        {
            XDocument document = SettingsLoader.Load();
            XElement element = document.Root;
            Logging = new LoggingSettings(element.Element(LoggingElementName));
            CrashDump = new CrashDumpSettings(element.Element(CrashDumpElementName));
            Extensions = new ExtensionSettingsCollection(element.Element(ExtensionsElementName));
            ProfilingResults = new ProfilingResultsSettings(element.Element(ProfilingResultsElementName));
            HostConnections = new HostConnectionSettingsCollection(element.Element(HostConnectionsElementName));
        }

        public ILoggingSettings Logging { get; private set; }

        public ICrashDumpSettings CrashDump { get; private set; }

        public IExtensionSettingsCollection Extensions { get; private set; }

        public IProfilingResultsSettings ProfilingResults { get; private set; }

        public IConnectionSettingsCollection HostConnections { get; private set; }
    }
}
