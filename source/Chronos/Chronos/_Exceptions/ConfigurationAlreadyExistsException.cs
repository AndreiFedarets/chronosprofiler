using System;
using System.Runtime.Serialization;

namespace Chronos
{
    public class ConfigurationAlreadyExistsException : ProfilerException
    {
        public ConfigurationAlreadyExistsException(Guid configurationToken)
            : base(string.Format("Configuration with token '{0}' already exists.", configurationToken))
        {
        }

        public ConfigurationAlreadyExistsException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
