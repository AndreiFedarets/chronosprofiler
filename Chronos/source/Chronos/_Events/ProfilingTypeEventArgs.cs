using System;

namespace Chronos
{
    [Serializable]
    public sealed class ProfilingTypeEventArgs : EventArgs
    {
        public ProfilingTypeEventArgs(IProfilingType profilingType)
        {
            ProfilingType = profilingType;
        }

        public IProfilingType ProfilingType { get; private set; }

        internal static void RaiseEvent(EventHandler<ProfilingTypeEventArgs> handler, object sender, IProfilingType profilingType)
        {
            if (handler != null)
            {
                handler(sender, new ProfilingTypeEventArgs(profilingType));
            }
        }
    }
}
