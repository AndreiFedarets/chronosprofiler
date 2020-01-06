using Chronos.Client.Win.Common.ViewModels;
using Chronos.Common;
using Chronos.DotNet.BasicProfiler;
using Layex.Extensions;
using Layex.ViewModels;
using System;
using System.Collections.Generic;

namespace Chronos.Client.Win.DotNet.BasicProfiler.ViewModels
{
    [ViewModel(Constants.ViewModels.AppDomainsViewModel, Constants.Views.UnitsListView)]
    public sealed class AppDomainsViewModel : UnitsListViewModel<AppDomainInfo>
    {
        public AppDomainsViewModel(IAppDomainCollection units)
            : base(units, GetColumns(), Constants.Menus.AppDomainContentMenu)
        {
        }

        public override string DisplayName
        {
            get { return "AppDomains"; }
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
