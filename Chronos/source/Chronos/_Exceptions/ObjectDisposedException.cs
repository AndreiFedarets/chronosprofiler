using System;
using System.Diagnostics;
using System.Text;

namespace Chronos
{
    public class ObjectDisposedException : Exception
    {
        public ObjectDisposedException(StackTrace disposedStackTrace)
        {
            DisposedStackTrace = disposedStackTrace;
        }

        public StackTrace DisposedStackTrace { get; private set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(base.ToString());
            builder.AppendLine("Disposed StackTrace:");
            builder.Append(DisposedStackTrace);
            return builder.ToString();
        }
    }
}
