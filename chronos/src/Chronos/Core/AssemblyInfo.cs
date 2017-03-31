using System;
using System.Collections.Generic;
using System.Linq;

namespace Chronos.Core
{
	[Serializable]
	public sealed class AssemblyInfo : UnitBase
	{
		public AssemblyInfo(uint id, ulong managedId, uint beginLifetime, uint endLifetime, string fullName, Result loadResult, ulong appDomainManangedId)
			: base(id, managedId, beginLifetime, endLifetime, fullName)
		{
			LoadResult = loadResult;
			AppDomainManagedId = appDomainManangedId;
		}

		public AssemblyInfo()
        {
            LoadResult = Result.E_FAIL;
            AppDomainManagedId = 0;
		}

		public Result LoadResult { get; set; }

		public ulong AppDomainManagedId { get; set; }

        public AppDomainInfo AppDomain
        {
            get
            {
                List<AppDomainInfo> appDomains = ProcessShadow.AppDomains.Snapshot();
                AppDomainInfo appDomainInfo = appDomains.FirstOrDefault(BelongsTo);
                if (appDomainInfo == null && string.Equals(Name, "mscorlib", StringComparison.InvariantCultureIgnoreCase))
                {
                    appDomainInfo = appDomains.FirstOrDefault(x => x.Id == 0);
                }
                return appDomainInfo;
            }
        }

        public List<ModuleInfo> Modules
        {
            get
            {
                //TODO: cache this value
                List<ModuleInfo> modules = ProcessShadow.Modules.Snapshot();
                modules = modules.Where(x => x.BelongsTo(this)).ToList();
                return modules;
            }
        }

		public List<ClassInfo> Classes
		{
			get
			{
				List<ClassInfo> classes = Modules.SelectMany(x => x.Classes).ToList();
				return classes;
			}
		}

		public List<FunctionInfo> Functions
		{
			get
			{
				List<FunctionInfo> functions = Modules.SelectMany(x => x.Functions).ToList();
				return functions;
			}
		}

        public bool BelongsTo(AppDomainInfo appDomainInfo)
        {
            return appDomainInfo.ManagedId == AppDomainManagedId && BeginLifetime >= appDomainInfo.BeginLifetime &&
                   (EndLifetime <= appDomainInfo.EndLifetime || EndLifetime == 0);
        }
	}
}
