using System;
using System.Collections.Generic;
using Adenium;
using Chronos.Client.Win.ViewModels;
using Chronos.DotNet.SqlProfiler;
using Chronos.Model;

namespace Chronos.Client.Win.DotNet.SqlProfiler.ViewModels
{
    [ViewModelAttribute(Constants.ViewModels.SqlQueriesViewModel, Constants.Views.UnitsListView)]
    public sealed class SqlQueriesViewModel : UnitsListViewModel<SqlQueryInfo>
    {
        public SqlQueriesViewModel(ISqlQueryCollection units)
            : base(units, GetColumns(), Constants.Menus.SqlQueryContentMenu)
        {
        }

        public override string DisplayName
        {
            get { return "SQL Queries"; }
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
