using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using Rhiannon.Extensions;

namespace Rhiannon.Serialization.Marshaling
{
	public static class MarshalingManager
	{
		private static readonly IDictionary<Type, Marshaler> Marshalers;

		static MarshalingManager()
		{
			Marshalers = new Dictionary<Type, Marshaler>();
		    AllowUnknowTypeMarshaling = false;
			Initialize();
		}

		private static void Initialize()
		{
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
			RegisterMarshaler(new ByteArrayMarshaler());
		}

        public static bool AllowUnknowTypeMarshaling { get; set; }

		//public static void RegisterMarshaler<T>(Marshaler marshaler)
		//{
		//    Marshalers.Add(typeof(T), marshaler);
		//}

		public static void RegisterMarshaler(Marshaler marshaler)
		{
			if (!Marshalers.ContainsKey(marshaler.ManagedType))
			{
				Marshalers.Add(marshaler.ManagedType, marshaler);
			}
		}

		public static void Marshal(object value, Stream stream)
		{
			Debug.Assert(value != null, "MarshalingManager: value cannot be null");
			Marshaler marshaler = GetMarshaler(value.GetType());
			marshaler.MarshalObject(value, stream);
		}

		public static object Demarshal(Type targetType, Stream stream)
		{
			Marshaler marshaler = GetMarshaler(targetType);
			return marshaler.DemarshalObject(stream);
		}

		public static T Demarshal<T>(Stream stream)
		{
			return (T)Demarshal(typeof (T), stream);
		}

		public static Marshaler GetMarshaler(Type type)
		{
			lock (Marshalers)
			{
				Marshaler marshaler;
				if (!Marshalers.TryGetValue(type, out marshaler))
				{
					if (IsEnum(type))
					{
						marshaler = new EnumMarshaler(type);
					}
					else if (IsArray(type))
					{
						marshaler = new ArrayMarshaler(type);
					}
					else if (IsList(type))
					{
						marshaler = new ListMarshaler(type);
					}
					else
					{
                        if (AllowUnknowTypeMarshaling)
                        {
                            marshaler = new ObjectMarshaler(type);   
                        }
                        else
                        {
                            throw new InvalidOperationException();
                        }
					}
					Marshalers.Add(type, marshaler);
				}
				return marshaler;
			}
		}

		private static bool IsEnum(Type type)
		{
			return type.IsEnum;
		}

		private static bool IsList(Type type)
		{
			return type.ImplementsInterface<IList>();
		}

		public static Marshaler GetMarshaler<T>()
		{
			return GetMarshaler(typeof (T));
		}

		private static bool IsArray(Type type)
		{
			return type.IsArray;
		}
	}
}
