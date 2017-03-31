namespace Chronos.Core
{
	public sealed class ThreadCollection : UnitCollection<ThreadInfo>, IThreadCollection
	{
        public override uint UnitType
		{
            get { return (uint)Core.UnitType.Thread; }
		}
	}
}
