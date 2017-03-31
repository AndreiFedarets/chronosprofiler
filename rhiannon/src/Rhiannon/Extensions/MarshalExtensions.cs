using System;
using System.Runtime.InteropServices;

namespace Rhiannon.Extensions
{
	public static class MarshalExtensions
	{
		public static T ConvertToStruct<T>(byte[] buffer) where T : struct
		{
			Type type = typeof (T);
			GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
			T result = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), type);
			handle.Free();
			return result;
		}

		public static object ConvertToStruct(Type type, byte[] buffer)
		{
			GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
			object result = Marshal.PtrToStructure(handle.AddrOfPinnedObject(), type);
			handle.Free();
			return result;
		}

		public static byte[] ConvertToByteArray<T>(T @struct, int size) where T : struct
		{
			byte[] buffer = new byte[size];
			IntPtr pointer = Marshal.AllocHGlobal(size);

			Marshal.StructureToPtr(@struct, pointer, true);
			Marshal.Copy(pointer, buffer, 0, size);
			Marshal.FreeHGlobal(pointer);

			return buffer;
		}
	}
}
