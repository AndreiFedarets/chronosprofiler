using Chronos.Core;

namespace Chronos.DotNet.BasicProfiler.Client.Win
{
    public partial class AssembliesWidget
    {
        public UnitCollectionModel<AssemblyInfo> Model { get; private set; }

        public ISession Session
        {
            get { return Context.Session; }
        }

        protected override void Initialize()
        {
            base.Initialize();
            IAssemblyCollection unitCollection = Session.Daemon.Container.Resolve<IAssemblyCollection>();
            Model = new UnitCollectionModel<AssemblyInfo>(unitCollection);
        }
    }
}
