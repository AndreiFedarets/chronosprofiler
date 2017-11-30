using Chronos.Client.Win.Java.BasicProfiler.Properties;
using Chronos.Client.Win.Menu.Specialized;
using Chronos.Client.Win.Models;
using Chronos.Client.Win.Models.Java.BasicProfiler;
using Chronos.Client.Win.ViewModels.Profiling;
using Chronos.Java.BasicProfiler;

namespace Chronos.Client.Win.Menu.Java.BasicProfiler
{
    internal sealed class ClassesMenuItem : UnitsMenuItemBase
    {
        public ClassesMenuItem(ProfilingViewModel profilingViewModel)
            : base(profilingViewModel)
        {
        }
        
        public override string Text
        {
            get { return Resources.ClassesMenuItem_Text; }
            protected set { }
        }

        protected override IUnitsModel GetModel()
        {
            //IClassCollection collection = ProfilingViewModel.Application.ServiceContainer.Resolve<IClassCollection>();
            //ClassesModel model = new ClassesModel(collection);
            //return model;
            return null;
        }
    }
}
