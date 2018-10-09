using System;

namespace Chronos.Messaging
{
    [AttributeUsage(AttributeTargets.Method)]
    public class MessageHandlerAttribute : Attribute
    {
        public MessageHandlerAttribute(uint message)
        {
            Message = message;
        }

        public uint Message { get; private set; }
    }
}
