using System;
using System.Collections.Generic;
using Adenium;
using Chronos.Client.Win.ViewModels;
using Chronos.DotNet.BasicProfiler;
using Chronos.Model;

namespace Chronos.Client.Win.DotNet.BasicProfiler.ViewModels
{
    [ViewModelAttribute(Constants.ViewModels.AppDomainsViewModel, Constants.Views.UnitsListView)]
    public sealed class AppDomainsViewModel : UnitsListViewModel<AppDomainInfo>
    {
        public AppDomainsViewModel(IAppDomainCollection units)
            : base(units, GetColumns(), Constants.Menus.AppDomainContentMenu)
        {
        }

        public override string DisplayName
        {
            get { return "AppDomains"; }
            set { }
        }

        private static IEnumerable<GridViewDynamicColumn> GetColumns()
        {
            return new List<GridViewDynamicColumn>
            {
                new GridViewDynamicColumn("Name", "Name", FilterByName)
            };
        }

        private static bool FilterByName(object item, string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return true;
            }
            UnitBase unit = (UnitBase)item;
            return unit.Name.IndexOf(text, StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }
}
