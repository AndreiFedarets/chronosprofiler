using Chronos.Model;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Chronos.Client.Win.Models
{
    public interface IUnitsModel
    {
        string DisplayName { get; }

        IEnumerable<GridViewDynamicColumn> Columns { get; }

        IEnumerable Units { get; }

        UnitBase SelectedUnit { get; set; }

        Type UnitType { get; }
    }
}
