using System.Collections;
using System.Collections.Generic;
using Chronos.Client.Win.Model;

namespace Chronos.Client.Win.DotNet.BasicProfiler.Models
{
    public interface IUnitsModel
    {
        string DisplayName { get; }

        IEnumerable<GridViewDynamicColumn> Columns { get; }

        IEnumerable Units { get; }

        UnitBase SelectedUnit { get; set; }
    }
}
