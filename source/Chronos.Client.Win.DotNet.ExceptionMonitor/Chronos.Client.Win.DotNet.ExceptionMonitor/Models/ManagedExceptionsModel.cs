using Chronos.DotNet.ExceptionMonitor;
using Chronos.Model;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Chronos.Client.Win.Models.DotNet.ExceptionMonitor
{
    public sealed class ManagedExceptionsModel : IUnitsModel
    {
        public ManagedExceptionsModel(IManagedExceptionCollection units)
        {
            Units = units;
            Columns = new List<GridViewDynamicColumn>
            {
                new GridViewDynamicColumn("Exception Type", "ExceptionClass.Name"),
                new GridViewDynamicColumn("Exception Message", "Name")
            };
        }

        public string DisplayName
        {
            get { return "Managed Exceptions"; }
        }

        public Type UnitType
        {
            get { return typeof(ManagedExceptionInfo); }
        }

        public IEnumerable<GridViewDynamicColumn> Columns { get; private set; }

        public IEnumerable Units { get; private set; }

        public UnitBase SelectedUnit { get; set; }
    }
}
