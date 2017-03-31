using System;

namespace Chronos.Marshaling
{
    public interface ITypeMarshalerFactory
    {
        bool CanMarshal(Type type);
         
        bool TryCreate(Type type, out ITypeMarshaler typeMarshaler);
    }
}
