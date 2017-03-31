using Chronos.Settings;
using System.Threading;

namespace Chronos
{
    public static class LoggingProvider
    {
        private static ILogger _logger;

        static LoggingProvider()
        {
            _logger = new DefaultLogger();
        }

        internal static void Initialize(ILoggingSettings settings, string applicationName)
        {
            CompositeLogger logger = new CompositeLogger();
            logger.Initialize(settings, applicationName);
            Interlocked.Exchange(ref _logger, logger);
        }

        public static ILogger Current
        {
            get { return _logger; }
        }
    }
}
