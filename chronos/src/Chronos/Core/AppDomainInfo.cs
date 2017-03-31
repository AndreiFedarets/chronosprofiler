using System;
using System.Collections.Generic;
using System.Linq;

namespace Chronos.Core
{
	[Serializable]
	public sealed class AppDomainInfo : UnitBase
	{
		public AppDomainInfo(uint id, ulong managedId, uint beginLifetime, uint endLifetime, string fullName, Result loadResult)
			: base(id, managedId, beginLifetime, endLifetime, fullName)
		{
			LoadResult = loadResult;
		}

		public AppDomainInfo()
        {
            LoadResult = Result.E_FAIL;
		}

		public Result LoadResult { get; set; }

	    public List<AssemblyInfo> Assemblies
	    {
            get
            {
                //TODO: cache this value
                List<AssemblyInfo> assemblies = ProcessShadow.Assemblies.ToList();
                assemblies = assemblies.Where(x => x.BelongsTo(this)).ToList();
                return assemblies;
            }
	    }
	}
}
