using Chronos.Client.Win.DotNet.BasicProfiler.Properties;
using Chronos.Client.Win.Menu.Specialized;
using Chronos.Client.Win.Models;
using Chronos.Client.Win.Models.DotNet.BasicProfiler;
using Chronos.DotNet.BasicProfiler;

namespace Chronos.Client.Win.Menu.DotNet.BasicProfiler
{
    internal sealed class ClassesMenuItem : UnitsMenuItemBase
    {
        public ClassesMenuItem(IProfilingApplication application)
            : base(application)
        {
        }
        
        public override string Text
        {
            get { return Resources.ClassesMenuItem_Text; }
            protected set { }
        }

        protected override IUnitsModel GetModel()
        {
            IClassCollection collection = Application.ServiceContainer.Resolve<IClassCollection>();
            ClassesModel model = new ClassesModel(collection);
            return model;
        }
    }
}
