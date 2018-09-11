using Chronos.DotNet.BasicProfiler;

namespace Chronos.Client.Win.DotNet.TracingProfiler
{
    internal class FunctionCallEventFormatter : IEventFormatter
    {
        private readonly IFunctionCollection _functions;

        public FunctionCallEventFormatter(IFunctionCollection functions)
        {
            _functions = functions;
        }

        public byte EventType
        {
            get { return 0x0D; }
        }

        public string FormatName(IEvent @event)
        {
            FunctionInfo functionInfo = _functions[@event.Unit];
            if (functionInfo == null)
            {
                return "<UNKNOWN FUNCTION>";
            }
            return functionInfo.FullName;
        }
    }
}
