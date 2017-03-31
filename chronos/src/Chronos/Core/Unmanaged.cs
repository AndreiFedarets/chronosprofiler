using System;
using System.Runtime.InteropServices;

namespace Chronos.Core
{
	unsafe public static class Unmanaged
	{
		public static void* New<T>(int elementCount) where T : struct
		{
			return Marshal.AllocHGlobal(Marshal.SizeOf(typeof(T)) * elementCount).ToPointer();
		}

		public static void Free(void* pointer)
		{
			Marshal.FreeHGlobal(new IntPtr(pointer));
		}
	}
}
