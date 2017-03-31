using System.Collections.Generic;
using System.Linq;

namespace Chronos.Extension.ProfilingTarget.InternetInformationService
{
    internal class ApplicationPoolCollection : IApplicationPoolCollection
    {
        private readonly Microsoft.Web.Administration.ApplicationPoolCollection _underlyingObject;
        private IList<IApplicationPool> _applicationPools;

        public ApplicationPoolCollection(Microsoft.Web.Administration.ApplicationPoolCollection applicationPools)
        {
            _underlyingObject = applicationPools;
            Refresh();
        }

        public IApplicationPool this[string name]
        {
            get
            {
                IApplicationPool applicationPool = _applicationPools.FirstOrDefault(x => string.Equals(x.Name, name));
                if (applicationPool == null)
                {
                    Refresh();
                    applicationPool = _applicationPools.FirstOrDefault(x => string.Equals(x.Name, name));
                }
                return applicationPool;
            }
        }


        public void Refresh()
        {
            _applicationPools = _underlyingObject.Select(x => (IApplicationPool)new ApplicationPool(x)).ToList();
        }

        public IEnumerator<IApplicationPool> GetEnumerator()
        {
            return _applicationPools.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
