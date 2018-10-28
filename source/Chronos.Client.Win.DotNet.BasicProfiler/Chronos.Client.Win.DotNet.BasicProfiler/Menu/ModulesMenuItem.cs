using Chronos.Client.Win.DotNet.BasicProfiler.Properties;
using Chronos.Client.Win.Menu.Specialized;
using Chronos.Client.Win.Models;
using Chronos.Client.Win.Models.DotNet.BasicProfiler;
using Chronos.DotNet.BasicProfiler;

namespace Chronos.Client.Win.Menu.DotNet.BasicProfiler
{
    internal sealed class ModulesMenuItem : UnitsMenuItemBase
    {
        public ModulesMenuItem(IProfilingApplication application)
            : base(application)
        {
        }

        public override string GetText()
        {
            return Resources.ModulesMenuItem_Text;
        }

        protected override IUnitsListModel GetModel()
        {
            IModuleCollection collection = Application.ServiceContainer.Resolve<IModuleCollection>();
            ModulesModel model = new ModulesModel(collection);
            return model;
        }
    }
}
