using Chronos.Client.Win.Java.BasicProfiler.Properties;
using Chronos.Client.Win.Menu.Specialized;
using Chronos.Client.Win.Models;
using Chronos.Client.Win.Models.Java.BasicProfiler;
using Chronos.Client.Win.ViewModels.Profiling;
using Chronos.Java.BasicProfiler;

namespace Chronos.Client.Win.Menu.Java.BasicProfiler
{
    internal sealed class AssembliesMenuItem : UnitsMenuItemBase
    {
        public AssembliesMenuItem(ProfilingViewModel profilingViewModel)
            : base(profilingViewModel)
        {
        }
        
        public override string Text
        {
            get { return Resources.AssembliesMenuItem_Text; }
            protected set { }
        }

        protected override IUnitsModel GetModel()
        {
            //IAssemblyCollection collection = ProfilingViewModel.Application.ServiceContainer.Resolve<IAssemblyCollection>();
            //AssembliesModel model = new AssembliesModel(collection);
            //return model;
            return null;
        }
    }
}
