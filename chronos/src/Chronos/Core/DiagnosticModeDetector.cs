using System;
using System.Linq;

namespace Chronos.Core
{
    public static class DiagnosticModeDetector
    {
        public static bool IsDiagnosticMode()
        {
            string[] args = Environment.GetCommandLineArgs();
            return args.Any(x => string.Equals(x, "-diag", StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
