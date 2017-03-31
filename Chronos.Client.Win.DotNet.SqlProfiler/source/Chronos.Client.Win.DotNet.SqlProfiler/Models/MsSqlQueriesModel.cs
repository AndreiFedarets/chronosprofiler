using System.Collections;
using System.Collections.Generic;
using Chronos.Client.Win.Model;

namespace Chronos.Client.Win.DotNet.SqlProfiler.Models
{
    public sealed class MsSqlQueriesModel : IUnitsModel
    {
        public MsSqlQueriesModel(IMsSqlQueryCollection units)
        {
            Units = units;
            Columns = new List<GridViewDynamicColumn>
            {
                new GridViewDynamicColumn("Name", "Name")
            };
        }

        public string DisplayName
        {
            get { return "MS SQL Queries"; }
        }

        public IEnumerable<GridViewDynamicColumn> Columns { get; private set; }

        public IEnumerable Units { get; private set; }

        public UnitBase SelectedUnit { get; set; }
    }
}
