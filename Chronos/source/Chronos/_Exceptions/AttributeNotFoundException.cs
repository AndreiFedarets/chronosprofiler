using System.Runtime.Serialization;

namespace Chronos
{
    public class AttributeNotFoundException : ProfilerException
    {
        public AttributeNotFoundException(string name)
            : base(string.Format("Attribute with name '{0}' was not found", name))
        {
            AttributeName = name;
        }

        public AttributeNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            
        }

        public string AttributeName { get; private set; }
    }
}
