using System;
using System.Collections;
using System.Collections.Generic;
using Adenium;
using Chronos.DotNet.SqlProfiler;
using Chronos.Model;

namespace Chronos.Client.Win.Models.DotNet.SqlProfiler
{
    public sealed class SqlQueriesModel : IUnitsListModel
    {
        public SqlQueriesModel(ISqlQueryCollection units)
        {
            Units = units;
            Columns = new List<GridViewDynamicColumn>
            {
                new GridViewDynamicColumn("Name", "Name")
            };
        }

        public string DisplayName
        {
            get { return "SQL Queries"; }
        }

        public Type UnitType
        {
            get { return typeof(SqlQueryInfo); }
        }

        public IEnumerable<GridViewDynamicColumn> Columns { get; private set; }

        public IEnumerable Units { get; private set; }

        public UnitBase SelectedUnit { get; set; }
    }
}
