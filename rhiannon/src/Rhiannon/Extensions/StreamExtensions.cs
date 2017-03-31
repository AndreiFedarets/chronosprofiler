using System;
using System.IO;

namespace Rhiannon.Extensions
{
	public static class StreamExtensions
	{
		[ThreadStatic]
		private static byte[] _buffer;

		//Copy of .net 4 CopyTo
		public static void CopyTo(this Stream source, Stream destination)
		{
			if (destination == null)
			{
				throw new ArgumentNullException("destination");
			}
			if (!source.CanRead)
			{
				throw new NotSupportedException();
			}
			if (!destination.CanWrite)
			{
				throw new NotSupportedException();
			}
			const int bufferSize = 4096;
			if (_buffer == null)
			{
				_buffer = new byte[bufferSize];
			}
			int count;
			while ((count = source.Read(_buffer, 0, _buffer.Length)) != 0)
			{
				destination.Write(_buffer, 0, count);
			}
		}

		//private static byte[] _readBuffer;

		//public static IntPtr ReadNative(this Stream stream, int length)
		//{
		//    lock (_readBuffer)
		//    {
		//        IntPtr pointer = Marshal.AllocHGlobal(length);
		//        byte[] buffer = GetBuffer(length);
		//        stream.Read(buffer, 0, length);
		//        Marshal.Copy(buffer, 0, pointer, length);
		//        return pointer;
		//    }
		//}

		//private static byte[] GetBuffer(int dataSize)
		//{
		//    if (_readBuffer == null)
		//    {
		//        _readBuffer = new byte[dataSize];
		//        return _readBuffer;
		//    }
		//    if (_readBuffer.Length < dataSize)
		//    {
		//        _readBuffer = new byte[dataSize];
		//    }
		//    return _readBuffer;
		//}

		//public static UIntPtr ReadUnsignedPtr(this Stream stream)
		//{
		//    return ReadStruct<UIntPtr>(stream, UIntPtr.Size);
		//}

		//public static uint ReadUnsignedInt(this Stream stream)
		//{
		//    return ReadStruct<uint>(stream, sizeof(uint));
		//}

		//public static int ReadInt(this Stream stream)
		//{
		//    return ReadStruct<int>(stream, sizeof(int));
		//}

		//public static void WriteBool(this Stream stream, bool value)
		//{
		//    WriteStruct(stream, value, sizeof(bool));
		//}

		//public static void WriteInt(this Stream stream, int value)
		//{
		//    WriteStruct(stream, value, sizeof(int));
		//}

		//public static void WriteUnsignedInt(this Stream stream, uint value)
		//{
		//    WriteStruct(stream, value, sizeof(uint));
		//}

		//public static bool ReadBool(this Stream stream)
		//{
		//    return ReadStruct<bool>(stream, sizeof(bool));
		//}

		//public static string ReadString(this Stream stream, int size)
		//{
		//    byte[] valueBuffer = new byte[size];
		//    stream.Read(valueBuffer, 0, valueBuffer.Length);
		//    string value = System.Text.Encoding.Unicode.GetString(valueBuffer);
		//    return value;
		//}

		//public static T ReadStruct<T>(this Stream stream, int size) where T : struct
		//{
		//    byte[] buffer = new byte[size];
		//    stream.Read(buffer, 0, buffer.Length);
		//    T data = MarshalExtensions.ConvertToStruct<T>(buffer);
		//    return data;
		//}

		//public static void WriteStruct<T>(this Stream stream, T @struct, int size) where T : struct
		//{
		//    byte[] buffer = MarshalExtensions.ConvertToByteArray(@struct, size);
		//    stream.Write(buffer, 0, buffer.Length);
		//}
	}
}
