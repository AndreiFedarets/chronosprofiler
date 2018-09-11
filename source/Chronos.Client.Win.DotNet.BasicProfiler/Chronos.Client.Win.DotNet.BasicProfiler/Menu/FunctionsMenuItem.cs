using Chronos.Client.Win.DotNet.BasicProfiler.Properties;
using Chronos.Client.Win.Menu.Specialized;
using Chronos.Client.Win.Models;
using Chronos.Client.Win.Models.DotNet.BasicProfiler;
using Chronos.Client.Win.ViewModels.Profiling;
using Chronos.DotNet.BasicProfiler;

namespace Chronos.Client.Win.Menu.DotNet.BasicProfiler
{
    internal sealed class FunctionsMenuItem : UnitsMenuItemBase
    {
        public FunctionsMenuItem(ProfilingViewModel profilingViewModel)
            : base(profilingViewModel)
        {
        }
        
        public override string Text
        {
            get { return Resources.FunctionsMenuItem_Text; }
            protected set { }
        }

        protected override IUnitsModel GetModel()
        {
            IFunctionCollection collection = ProfilingViewModel.Application.ServiceContainer.Resolve<IFunctionCollection>();
            FunctionsModel model = new FunctionsModel(collection);
            return model;
        }
    }
}
