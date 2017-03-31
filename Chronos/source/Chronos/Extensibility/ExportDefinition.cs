using System.IO;

namespace Chronos.Extensibility
{
    public sealed class ExportDefinition
    {
        private readonly string _entryPoint;
        private readonly string _entryPoint32;
        private readonly string _entryPoint64;

        internal ExportDefinition(string application, string baseDirectory, string entryPoint, string entryPoint32, string entryPoint64, LoadBehavior loadBehavior)
        {
            Application = application;
            _entryPoint = entryPoint;
            _entryPoint32 = entryPoint32;
            _entryPoint64 = entryPoint64;
            BaseDirectory = baseDirectory;
            LoadBehavior = loadBehavior;
        }

        public string Application { get; private set; }

        public string BaseDirectory { get; private set; }

        public LoadBehavior LoadBehavior { get; private set; }

        public string GetEntryPoint(ProcessPlatform processPlatform, bool fullPath)
        {
            string entryPoint = string.Empty;
            if (string.IsNullOrEmpty(_entryPoint))
            {
                switch (processPlatform)
                {
                    case ProcessPlatform.AnyCPU:
                        entryPoint = _entryPoint;
                        break;
                    case ProcessPlatform.I386:
                        entryPoint = _entryPoint32;
                        break;
                    case ProcessPlatform.X64:
                        entryPoint = _entryPoint64;
                        break;
                }
            }
            else
            {
                entryPoint = _entryPoint;
            }
            if (string.IsNullOrEmpty(entryPoint))
            {
                throw new TempException();
            }
            if (fullPath)
            {
                entryPoint = Path.Combine(BaseDirectory, entryPoint);
            }
            return entryPoint;
        }
    }
}
