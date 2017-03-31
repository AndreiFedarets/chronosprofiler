using Chronos.Core;

namespace Chronos.Daemon.Proxy
{
	internal class AssemblyCollection : UnitCollection<AssemblyInfo>, IAssemblyCollection
	{
        public AssemblyCollection(IProcessShadow processShadow, IUnitCollection<AssemblyInfo> collection)
            : base(processShadow, collection, (uint)Core.UnitType.Assembly)
		{
		}
	}
}
