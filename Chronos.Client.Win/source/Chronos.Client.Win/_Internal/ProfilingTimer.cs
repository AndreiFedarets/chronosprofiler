namespace Chronos.Client.Win
{
    internal sealed class ProfilingTimer : IProfilingTimer
    {
        private readonly ISession _session;

        public ProfilingTimer(ISession session)
        {
            _session = session;
        }

        public uint CurrentTime
        {
            get { return _session.CurrentProflingTime; }
        }
    }
}
