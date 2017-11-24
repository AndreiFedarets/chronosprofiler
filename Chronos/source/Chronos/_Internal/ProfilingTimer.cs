using Chronos.Win32;
namespace Chronos
{
    internal sealed class ProfilingTimer
    {
        private readonly uint _profilingBeginTime;

        public ProfilingTimer(uint profilingBeginTime)
        {
            _profilingBeginTime = profilingBeginTime;
        }

        public uint CurrentTime
        {
            get
            {
                return GetCurrentAbsoluteTime() - _profilingBeginTime;
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
