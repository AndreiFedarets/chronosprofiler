using System.Collections.Generic;
using System.Linq;
using Chronos.Core;

namespace Chronos.Daemon.Proxy
{
	internal class FunctionCollection : UnitCollection<FunctionInfo>, IFunctionCollection
	{
        public FunctionCollection(IProcessShadow processShadow, IUnitCollection<FunctionInfo> collection)
            : base(processShadow, collection, (uint)Core.UnitType.Function)
		{
		}

        public List<FunctionInfo> GetAll(ClassInfo classInfo)
        {
            List<FunctionInfo> functions = DictionaryById.Values.Where(x => x.BelongsTo(classInfo)).ToList();
            return functions;
        }
	}
}
