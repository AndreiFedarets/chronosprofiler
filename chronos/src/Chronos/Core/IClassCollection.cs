using System.Collections.Generic;

namespace Chronos.Core
{
	public interface IClassCollection : IUnitCollection<ClassInfo>
	{
	    List<ClassInfo> GetAll(ModuleInfo moduleInfo);
	}
}
