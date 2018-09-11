using Chronos.Core;

namespace Chronos.DotNet.BasicProfiler.Client.Win
{
    public partial class ModulesWidget
    {
        public UnitCollectionModel<ModuleInfo> Model { get; private set; }

        public ISession Session
        {
            get { return Context.Session; }
        }

        protected override void Initialize()
        {
            base.Initialize();
            IModuleCollection unitCollection = Session.Daemon.Container.Resolve<IModuleCollection>();
            Model = new UnitCollectionModel<ModuleInfo>(unitCollection);
        }
    }
}
