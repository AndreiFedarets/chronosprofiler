using System.IO;
using System.Runtime.InteropServices;

namespace Chronos
{
    public class StructureExtensions
    {
        public static T Read<T>(Stream stream) where T : struct
        {
            int bufferSize = Marshal.SizeOf(typeof(T));
            byte[] buffer = new byte[bufferSize];
            stream.Read(buffer, 0, buffer.Length);
            GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            T structure = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            handle.Free();
            return structure;
        }
    }
}
