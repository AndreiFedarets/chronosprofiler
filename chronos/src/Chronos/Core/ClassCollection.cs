using System.Collections.Generic;
using System.Linq;

namespace Chronos.Core
{
	public sealed class ClassCollection : UnitCollection<ClassInfo>, IClassCollection
	{
        public override uint UnitType
		{
            get { return (uint)Core.UnitType.Class; }
		}

        public List<ClassInfo> GetAll(ModuleInfo moduleInfo)
        {
            List<ClassInfo> classes = DictionaryById.Values.Where(x => x.BelongsTo(moduleInfo)).ToList();
            return classes;
        }
	}
}
