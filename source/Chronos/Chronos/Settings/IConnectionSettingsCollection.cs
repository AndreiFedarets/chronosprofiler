using System.Collections.Generic;

namespace Chronos.Settings
{
    public interface IConnectionSettingsCollection : IEnumerable<IConnectionSettings>
    {
        bool RunLocal { get; set; }
    }
}
