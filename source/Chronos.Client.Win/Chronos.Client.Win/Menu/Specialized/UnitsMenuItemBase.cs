using Adenium;
using Chronos.Client.Win.Models;
using Chronos.Client.Win.ViewModels;

namespace Chronos.Client.Win.Menu.Specialized
{
    public abstract class UnitsMenuItemBase : ProfilingMenuItemBase
    {
        protected UnitsMenuItemBase(IProfilingApplication application)
            : base(application)
        {
        }
        
        protected abstract IUnitsListModel GetModel();

        protected sealed override IViewModel GetViewModel()
        {
            IUnitsListModel model = GetModel();
            return new UnitsListViewModel(model);
        }
    }
}
