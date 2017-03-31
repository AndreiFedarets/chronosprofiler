using Chronos.Client.Win.DotNet.BasicProfiler.Properties;
using Chronos.Client.Win.Menu.Specialized;
using Chronos.Client.Win.Models;
using Chronos.Client.Win.Models.DotNet.BasicProfiler;
using Chronos.Client.Win.ViewModels.Profiling;
using Chronos.DotNet.BasicProfiler;

namespace Chronos.Client.Win.Menu.DotNet.BasicProfiler
{
    internal sealed class ModulesMenuItem : UnitsMenuItemBase
    {
        public ModulesMenuItem(ProfilingViewModel profilingViewModel)
            : base(profilingViewModel)
        {
        }

        public override string Text
        {
            get { return Resources.ModulesMenuItem_Text; }
            protected set { }
        }

        protected override IUnitsModel GetModel()
        {
            IModuleCollection collection = ProfilingViewModel.Application.ServiceContainer.Resolve<IModuleCollection>();
            ModulesModel model = new ModulesModel(collection);
            return model;
        }
    }
}
