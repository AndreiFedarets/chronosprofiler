using System.Collections.Generic;
using System.Linq;

namespace Chronos.Core
{
	public sealed class FunctionCollection : UnitCollection<FunctionInfo>, IFunctionCollection
	{
        public override uint UnitType
		{
            get { return (uint)Core.UnitType.Function; }
		}

        public List<FunctionInfo> GetAll(ClassInfo classInfo)
        {
            List<FunctionInfo> functions = DictionaryById.Values.Where(x => x.BelongsTo(classInfo)).ToList();
            return functions;
        }
	}
}
