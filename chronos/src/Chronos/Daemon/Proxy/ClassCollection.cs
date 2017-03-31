using System.Collections.Generic;
using System.Linq;
using Chronos.Core;

namespace Chronos.Daemon.Proxy
{
	internal class ClassCollection : UnitCollection<ClassInfo>, IClassCollection
	{
        public ClassCollection(IProcessShadow processShadow, IUnitCollection<ClassInfo> collection)
            : base(processShadow, collection, (uint)Core.UnitType.Class)
		{
		}

        public List<ClassInfo> GetAll(ModuleInfo moduleInfo)
        {
            List<ClassInfo> classes = DictionaryById.Values.Where(x => x.BelongsTo(moduleInfo)).ToList();
            return classes;
        }
	}
}
