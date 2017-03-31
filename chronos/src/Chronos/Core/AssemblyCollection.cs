namespace Chronos.Core
{
	public sealed class AssemblyCollection : UnitCollection<AssemblyInfo>, IAssemblyCollection
	{
        public override uint UnitType
		{
            get { return (uint)Core.UnitType.Assembly; }
		}
	}
}
