using System.Collections;
using Chronos.Model;

namespace Chronos.Client.Win.Models
{
    public interface IUnitsTreeModel
    {
        string DisplayName { get; }

        IEnumerable RootUnits { get; }

        UnitBase SelectedUnit { get; set; }

        IEnumerable GetChildUnits(UnitBase unit);
    }
}
