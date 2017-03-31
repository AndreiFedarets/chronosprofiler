namespace Chronos.Core
{
    public sealed class SqlRequestCollection : UnitCollection<SqlRequestInfo>, ISqlRequestCollection
	{
        public override uint UnitType
		{
            get { return (uint)Core.UnitType.SqlRequest; }
		}
	}
}
