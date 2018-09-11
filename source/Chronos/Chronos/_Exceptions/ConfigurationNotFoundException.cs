using System;

namespace Chronos
{
    public class ConfigurationNotFoundException : ProfilerException
    {
        public ConfigurationNotFoundException(Guid configurationToken)
            : base(string.Format("Configuration with token '{0}' was not found.", configurationToken))
        {
            
        }
    }
}
