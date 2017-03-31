using Chronos.Core;

namespace Chronos.DotNet.BasicProfiler.Client.Win
{
    public partial class FunctionsWidget
    {
        public UnitCollectionModel<FunctionInfo> Model { get; private set; }

        public ISession Session
        {
            get { return Context.Session; }
        }

        protected override void Initialize()
        {
            base.Initialize();
            IFunctionCollection unitCollection = Session.Daemon.Container.Resolve<IFunctionCollection>();
            Model = new UnitCollectionModel<FunctionInfo>(unitCollection);
        }
    }
}
