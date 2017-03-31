using System;
using System.Collections.Generic;
using System.Linq;

namespace Chronos.Core
{
    [Serializable]
    public sealed class ModuleInfo : UnitBase
    {
        public ModuleInfo(uint id, ulong managedId, uint beginLifetime, uint endLifetime, string fullName, Result loadResult, ulong assemblyManagedId)
            : base(id, managedId, beginLifetime, endLifetime, fullName)
        {
            LoadResult = loadResult;
            AssemblyManagedId = assemblyManagedId;
        }

        public ModuleInfo()
        {
            LoadResult = Result.E_FAIL;
            AssemblyManagedId = 0;
        }

        public Result LoadResult { get; set; }

        public ulong AssemblyManagedId { get; set; }

        public AssemblyInfo Assembly
        {
            get
            {
                List<AssemblyInfo> assemblies = ProcessShadow.Assemblies.Snapshot();
                AssemblyInfo assemblyInfo = assemblies.FirstOrDefault(BelongsTo);
                return assemblyInfo;
            }
        }

        public List<ClassInfo> Classes
        {
            get
            {
                //TODO: cache this value
                List<ClassInfo> classes = ProcessShadow.Classes.Snapshot();
                classes = classes.Where(x => x.BelongsTo(this)).ToList();
                return classes;
            }
        }

		public List<FunctionInfo> Functions
		{
			get
			{
				List<FunctionInfo> functions = Classes.SelectMany(x => x.Functions).ToList();
				return functions;
			}
		}


        public bool BelongsTo(AssemblyInfo assemblyInfo)
        {
            return assemblyInfo.ManagedId == AssemblyManagedId && BeginLifetime >= assemblyInfo.BeginLifetime &&
                   (EndLifetime <= assemblyInfo.EndLifetime || EndLifetime == 0);
        }
    }
}
