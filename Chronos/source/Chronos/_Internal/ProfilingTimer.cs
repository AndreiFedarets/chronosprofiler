using Chronos.Win32;
namespace Chronos
{
    internal sealed class ProfilingTimer : IProfilingTimer
    {
        public ProfilingTimer(uint profilingBeginTime)
        {
            BeginProfilingTime = profilingBeginTime;
        }

        public uint BeginProfilingTime { get; private set; }

        public uint CurrentTime
        {
            get
            {
                return GetCurrentAbsoluteTime() - BeginProfilingTime;
            }
        }

        private uint GetCurrentAbsoluteTime()
        {
            long performanceFrequency = Kernel32.QueryPerformanceFrequency();
			long performanceCounter = Kernel32.QueryPerformanceCounter();
			double freq = 1000.0 / (double)performanceFrequency;
			return (uint)(performanceCounter * freq); 
        }

    }
}
