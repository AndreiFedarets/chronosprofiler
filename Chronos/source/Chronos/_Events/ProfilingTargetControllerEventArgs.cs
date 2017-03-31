using System;

namespace Chronos
{
    [Serializable]
    public sealed class ProfilingTargetControllerEventArgs : EventArgs
    {
        public static void RaiseEvent(EventHandler<ProfilingTargetControllerEventArgs> handler, IProfilingTargetController sender)
        {
            EventExtensions.RaiseEventSafeAndSilent(handler, sender, () => new ProfilingTargetControllerEventArgs());
        }
    }
}
