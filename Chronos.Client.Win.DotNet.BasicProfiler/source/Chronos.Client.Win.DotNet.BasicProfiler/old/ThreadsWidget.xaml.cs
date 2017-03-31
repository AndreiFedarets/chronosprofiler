using Chronos.Core;

namespace Chronos.DotNet.BasicProfiler.Client.Win
{
    public partial class ThreadsWidget
    {
        public UnitCollectionModel<ThreadInfo> Model { get; private set; }

        public ISession Session
        {
            get { return Context.Session; }
        }

        protected override void Initialize()
        {
            base.Initialize();
            IThreadCollection unitCollection = Session.Daemon.Container.Resolve<IThreadCollection>();
            Model = new UnitCollectionModel<ThreadInfo>(unitCollection);
        }
    }
}
