using System;

namespace Chronos
{
    public interface IProfilingTargetController
    {
        bool IsActive { get; }

        void Stop();

        void Start();

        event EventHandler<ProfilingTargetControllerEventArgs> TargetStopped;
    }
}
