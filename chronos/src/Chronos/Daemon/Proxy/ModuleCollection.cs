using System.Collections.Generic;
using System.Linq;
using Chronos.Core;

namespace Chronos.Daemon.Proxy
{
	internal class ModuleCollection : UnitCollection<ModuleInfo>, IModuleCollection
	{
        public ModuleCollection(IProcessShadow processShadow, IUnitCollection<ModuleInfo> collection)
            : base(processShadow, collection, (uint)Core.UnitType.Module)
		{
		}

        public List<ModuleInfo> GetAll(AssemblyInfo assemblyInfo)
        {
            List<ModuleInfo> modules = DictionaryById.Values.Where(x => x.BelongsTo(assemblyInfo)).ToList();
            return modules;
        }
    }
}
