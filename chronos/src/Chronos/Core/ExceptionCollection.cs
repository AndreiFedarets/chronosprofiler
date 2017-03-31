namespace Chronos.Core
{
	public sealed class ExceptionCollection : UnitCollection<ExceptionInfo>, IExceptionCollection
	{
        public override uint UnitType
		{
            get { return (uint)Core.UnitType.Exception; }
		}
	}
}
