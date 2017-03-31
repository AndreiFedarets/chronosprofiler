using System;

namespace Chronos
{
    public class SessionNotFoundException : ProfilerException
    {
        public SessionNotFoundException(Guid sessionToken)
            : base(string.Format("Session with token '{0}' was not found.", sessionToken))
        {
            
        }
    }
}
