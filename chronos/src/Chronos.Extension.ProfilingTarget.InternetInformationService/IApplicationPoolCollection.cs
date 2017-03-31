using System.Collections.Generic;

namespace Chronos.Extension.ProfilingTarget.InternetInformationService
{
    public interface IApplicationPoolCollection : IEnumerable<IApplicationPool>
    {
        IApplicationPool this[string name] { get; }

        void Refresh();
    }
}
