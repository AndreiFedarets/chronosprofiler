using Chronos.Core;

namespace Chronos.DotNet.BasicProfiler.Client.Win
{
    public partial class ClassesWidget
    {
        public UnitCollectionModel<ClassInfo> Model { get; private set; }

        public ISession Session
        {
            get { return Context.Session; }
        }

        protected override void Initialize()
        {
            base.Initialize();
            IClassCollection unitCollection = Session.Daemon.Container.Resolve<IClassCollection>();
            Model = new UnitCollectionModel<ClassInfo>(unitCollection);
        }
    }
}
