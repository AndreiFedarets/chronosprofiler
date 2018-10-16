using Chronos.Client.Win.DotNet.BasicProfiler.Properties;
using Chronos.Client.Win.Menu.Specialized;
using Chronos.Client.Win.Models;
using Chronos.Client.Win.Models.DotNet.BasicProfiler;
using Chronos.DotNet.BasicProfiler;

namespace Chronos.Client.Win.Menu.DotNet.BasicProfiler
{
    internal sealed class AssembliesMenuItem : UnitsMenuItemBase
    {
        public AssembliesMenuItem(IProfilingApplication application)
            : base(application)
        {
        }
        
        public override string Text
        {
            get { return Resources.AssembliesMenuItem_Text; }
            protected set { }
        }

        protected override IUnitsModel GetModel()
        {
            IAssemblyCollection collection = Application.ServiceContainer.Resolve<IAssemblyCollection>();
            AssembliesModel model = new AssembliesModel(collection);
            return model;
        }
    }
}
