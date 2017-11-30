using Chronos.Client.Win.Java.BasicProfiler.Properties;
using Chronos.Client.Win.Menu.Specialized;
using Chronos.Client.Win.Models;
using Chronos.Client.Win.Models.Java.BasicProfiler;
using Chronos.Client.Win.ViewModels.Profiling;
using Chronos.Java.BasicProfiler;

namespace Chronos.Client.Win.Menu.Java.BasicProfiler
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
            //IFunctionCollection collection = ProfilingViewModel.Application.ServiceContainer.Resolve<IFunctionCollection>();
            //FunctionsModel model = new FunctionsModel(collection);
            //return model;
            return null;
        }
    }
}
