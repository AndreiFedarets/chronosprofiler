using Chronos.Client.Win.Models;
using Chronos.Client.Win.ViewModels;
using Chronos.Client.Win.ViewModels.Profiling;

namespace Chronos.Client.Win.Menu.Specialized
{
    public abstract class UnitsMenuItemBase : ProfilingMenuItemBase
    {
        protected UnitsMenuItemBase(ProfilingViewModel profilingViewModel)
            : base(profilingViewModel)
        {
        }
        
        protected abstract IUnitsModel GetModel();

        protected sealed override ViewModel GetViewModel()
        {
            IUnitsModel model = GetModel();
            return new UnitsViewModel(model);
        }
    }
}
