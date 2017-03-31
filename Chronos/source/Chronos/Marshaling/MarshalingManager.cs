using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Chronos.Marshaling
{
    public static class MarshalingManager
    {
        private static readonly Dictionary<Type, ITypeMarshaler> Marshalers;
        private static readonly List<ITypeMarshalerFactory> Factories;

        static MarshalingManager()
        {
            Marshalers = new Dictionary<Type, ITypeMarshaler>();
            Factories = new List<ITypeMarshalerFactory>();
            Initialize();
        }

        private static void Initialize()
        {
            //Special
            RegisterFactory(new DynamicSettingsMarhalerFactory());
            RegisterFactory(new UniqueSettingsCollectionMarhalerFactory());
            //Collections
            RegisterMarshaler(new ByteArrayMarshaler());
            RegisterMarshaler(new StringDictionaryMarshaler());
            RegisterFactory(new ArrayMarshalerFactory());
            RegisterFactory(new ListMarshalerFactory());
            //Generic
            RegisterMarshaler(new VoidMarshaler());
            RegisterMarshaler(new StringMarshaler());
            RegisterMarshaler(new GuidMarshaler());
            RegisterMarshaler(new BoolMarshaler());
            RegisterMarshaler(new ByteMarshaler());
            RegisterMarshaler(new Int32Marshaler());
            RegisterMarshaler(new UInt32Marshaler());
            RegisterMarshaler(new Int64Marshaler());
            RegisterMarshaler(new UInt64Marshaler());
            RegisterMarshaler(new IntPtrMarshaler());
            RegisterMarshaler(new DateTimeMarshaler());
            RegisterFactory(new EnumMarshalerFactory());
        }

        public static void RegisterMarshaler(ITypeMarshaler marshaler)
        {
            lock (Marshalers)
            {
                if (!Marshalers.ContainsKey(marshaler.ManagedType))
                {
                    Marshalers.Add(marshaler.ManagedType, marshaler);
                }
            }
        }

        public static void RegisterFactory(ITypeMarshalerFactory factory)
        {
            lock (Factories)
            {
                if (!Factories.Contains(factory))
                {
                    Factories.Add(factory);
                }
            }
        }

        public static void Marshal(object value, Stream stream)
        {
            Debug.Assert(value != null, "MarshalingManager: value cannot be null");
            ITypeMarshaler marshaler = GetMarshaler(value.GetType());
            marshaler.MarshalObject(value, stream);
        }

        public static object Demarshal(Type targetType, Stream stream)
        {
            ITypeMarshaler marshaler = GetMarshaler(targetType);
            return marshaler.DemarshalObject(stream);
        }

        public static T Demarshal<T>(Stream stream)
        {
            return (T) Demarshal(typeof (T), stream);
        }

        public static ITypeMarshaler GetMarshaler(Type type)
        {
            ITypeMarshaler marshaler;
            lock (Marshalers)
            {
                if (Marshalers.TryGetValue(type, out marshaler))
                {
                    return marshaler;
                }
            }
            lock (Factories)
            {
                foreach (ITypeMarshalerFactory factory in Factories)
                {
                    if (factory.TryCreate(type, out marshaler))
                    {
                        RegisterMarshaler(marshaler);
                        return marshaler;
                    }
                }
            }
            return null;
        }

        public static bool IsKnownType(Type type)
        {
            lock (Marshalers)
            {
                if (Marshalers.ContainsKey(type))
                {
                    return true;
                }
            }
            lock (Factories)
            {
                foreach (ITypeMarshalerFactory factory in Factories)
                {
                    if (factory.CanMarshal(type))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static ITypeMarshaler GetMarshaler<T>()
        {
            return GetMarshaler(typeof (T));
        }
    }
}
