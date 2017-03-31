using System.Collections.Generic;
using System.Linq;

namespace Chronos.Core
{
	public sealed class ModuleCollection : UnitCollection<ModuleInfo>, IModuleCollection
	{
        public override uint UnitType
		{
            get { return (uint)Core.UnitType.Module; }
		}

        public List<ModuleInfo> GetAll(AssemblyInfo assemblyInfo)
        {
            List<ModuleInfo> modules = DictionaryById.Values.Where(x => x.BelongsTo(assemblyInfo)).ToList();
            return modules;
        }
	}
}
