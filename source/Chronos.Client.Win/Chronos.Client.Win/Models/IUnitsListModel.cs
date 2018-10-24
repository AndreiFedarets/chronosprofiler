using System;
using System.Collections;
using System.Collections.Generic;
using Adenium;
using Chronos.Model;

namespace Chronos.Client.Win.Models
{
    public interface IUnitsListModel
    {
        string DisplayName { get; }

        IEnumerable<GridViewDynamicColumn> Columns { get; }

        IEnumerable Units { get; }

        UnitBase SelectedUnit { get; set; }

        Type UnitType { get; }
    }
}
