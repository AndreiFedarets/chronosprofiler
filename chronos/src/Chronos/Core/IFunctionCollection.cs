using System.Collections.Generic;

namespace Chronos.Core
{
	public interface IFunctionCollection : IUnitCollection<FunctionInfo>
	{
	    List<FunctionInfo> GetAll(ClassInfo classInfo);
	}
}
