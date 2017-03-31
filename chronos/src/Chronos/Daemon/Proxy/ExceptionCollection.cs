using Chronos.Core;

namespace Chronos.Daemon.Proxy
{
	internal class ExceptionCollection : UnitCollection<ExceptionInfo>, IExceptionCollection
	{
        public ExceptionCollection(IProcessShadow processShadow, IUnitCollection<ExceptionInfo> collection)
            : base(processShadow, collection, (uint)Core.UnitType.Exception)
		{
		}
	}
}
