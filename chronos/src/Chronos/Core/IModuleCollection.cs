using System.Collections.Generic;

namespace Chronos.Core
{
	public interface IModuleCollection : IUnitCollection<ModuleInfo>
	{
        List<ModuleInfo> GetAll(AssemblyInfo assemblyInfo);
	}
}
