using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Chronos.Communication.Native
{
    public sealed class NativeArray : IDisposable
    {
        private IntPtr _dataBuffer;
        private int _dataSize;

        public NativeArray(IntPtr data, int size)
        {
            _dataSize = size;
            _dataBuffer = data;
        }

        private NativeArray(int size)
        {
            _dataSize = size;
            _dataBuffer = Win32.NativeMethods.Alloc(size);
        }

        private NativeArray(byte[] data, int dataSize)
            : this(dataSize)
        {
            Marshal.Copy(data, 0, _dataBuffer, _dataSize);
        }

        ~NativeArray()
        {
            Dispose();
        }

        public int Length
        {
            get { return _dataSize; }
        }

        public IntPtr Data
        {
            get { return _dataBuffer; }
            internal set { _dataBuffer = value; }
        }

        unsafe public Stream OpenRead()
        {
            return new UnmanagedMemoryStream((byte*)_dataBuffer.ToPointer(), _dataSize);
        }

        public void Dispose()
        {
            if (_dataBuffer != IntPtr.Zero)
            {
                Win32.NativeMethods.Free(_dataBuffer);
                _dataBuffer = IntPtr.Zero;
                _dataSize = 0;
            }
        }

        public static NativeArray Alloc(int size)
        {
            return new NativeArray(size);
        }

        public static NativeArray Copy(byte[] data, int dataSize)
        {
            return new NativeArray(data , dataSize);
        }
    }
}
