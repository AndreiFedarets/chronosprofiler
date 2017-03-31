using System;

namespace Chronos.Daemon.Internal
{
    internal static class PipeNameFormatter
    {
        public static string GetDaemonServerThreadPipeName(Guid daemonToken, uint index)
        {
            return string.Format("chronosprofiler-{0}-thread-{1}", daemonToken, index);
        }

        public static string GetDaemonServerUnitPipeName(Guid daemonToken, uint unitType)
        {
            return string.Format("chronosprofiler-{0}-unit-{1}", daemonToken, unitType);
        }

        public static string GetAgentServerPipeName(int processId)
        {
            return string.Format("chronosprofiler-agent-{0}", processId);
        }
    }
}
