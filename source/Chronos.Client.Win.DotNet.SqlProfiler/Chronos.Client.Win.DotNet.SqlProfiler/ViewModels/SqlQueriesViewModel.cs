using Chronos.Client.Win.Common.ViewModels;
using Chronos.Common;
using Chronos.DotNet.SqlProfiler;
using Layex.Extensions;
using Layex.ViewModels;
using System;
using System.Collections.Generic;

namespace Chronos.Client.Win.DotNet.SqlProfiler.ViewModels
{
    [ViewModel(Constants.ViewModels.SqlQueriesViewModel, Constants.Views.UnitsListView)]
    public sealed class SqlQueriesViewModel : UnitsListViewModel<SqlQueryInfo>
    {
        public SqlQueriesViewModel(ISqlQueryCollection units)
            : base(units, GetColumns(), Constants.Menus.SqlQueryContentMenu)
        {
        }

        public override string DisplayName
        {
            get { return "SQL Queries"; }
        }

        private static IEnumerable<GridViewDynamicColumn> GetColumns()
        {
            return new List<GridViewDynamicColumn>
            {
                new GridViewDynamicColumn("Name", "Name", FilterByName),
                new GridViewDynamicColumn("Execution Time (ms)", "Lifetime", null)
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
