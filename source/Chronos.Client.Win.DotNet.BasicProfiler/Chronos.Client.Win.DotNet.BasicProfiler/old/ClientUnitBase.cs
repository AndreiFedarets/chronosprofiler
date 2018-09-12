using Chronos.Daemon.DotNet.BasicProfiler;

namespace Chronos.Client.Win.DotNet.BasicProfiler
{
    public abstract class ClientUnitBase
    {
        protected DaemonUnitBase DaemonUnit;

        protected ClientUnitBase(DaemonUnitBase unitBase)
        {
            DaemonUnit = unitBase;
        }

        public uint Uid
        {
            get { return DaemonUnit.Uid; }
        }

        public ulong Id
        {
            get { return DaemonUnit.Id; }
        }

        public uint BeginLifetime
        {
            get { return DaemonUnit.BeginLifetime; }
        }

        public uint EndLifetime
        {
            get { return DaemonUnit.EndLifetime; }
        }

        public string Name
        {
            get { return DaemonUnit.Name; }
        }

        internal void Update(DaemonUnitBase unitBase)
        {
            DaemonUnit = unitBase;
        }
    }
}
