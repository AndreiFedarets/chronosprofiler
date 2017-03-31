using System;
using System.Collections.Generic;
using System.Linq;

namespace Chronos.Core
{
	[Serializable]
	public sealed class ClassInfo : UnitBase
	{
        public static readonly ClassInfo Unknown;

        static ClassInfo()
        {
            Unknown = new ClassInfo();
        }

		public ClassInfo(uint id, ulong managedId, uint beginLifetime, uint endLifetime, string fullName, Result loadResult, ulong moduleManangedId)
			: base(id, managedId, beginLifetime, endLifetime, fullName)
		{
			LoadResult = loadResult;
			ModuleManangedId = moduleManangedId;
		}

		public ClassInfo()
        {
            LoadResult = Result.E_FAIL;
            ModuleManangedId = 0;
		}

		public Result LoadResult { get; set; }

		public ulong ModuleManangedId { get; set; }

        public ModuleInfo Module
        {
            get
            {
                List<ModuleInfo> modules = ProcessShadow.Modules.Snapshot();
                ModuleInfo moduleInfo = modules.FirstOrDefault(BelongsTo);
                return moduleInfo;
            }
        }

        public List<FunctionInfo> Functions
        {
            get
            {
                //TODO: cache this value
                List<FunctionInfo> functions = ProcessShadow.Functions.Snapshot();
                functions = functions.Where(x => x.BelongsTo(this)).ToList();
                return functions;
            }
        }

        public bool BelongsTo(ModuleInfo moduleInfo)
        {
            return moduleInfo.ManagedId == ModuleManangedId && BeginLifetime >= moduleInfo.BeginLifetime &&
                   (EndLifetime <= moduleInfo.EndLifetime || EndLifetime == 0);
        }
	}
}
