using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Chronos.Daemon.Internal
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct SourcePage : ISourcePage
    {
        private uint _threadId;
        private uint _callstackId;
        private uint _pageIndex;
        private uint _beginLifetime;
        private uint _endLifetime;
        private PageState _flag;
        private IntPtr _data;
        private int _dataSize;

        public SourcePage(uint threadId, uint callstackId, uint pageIndex, PageState flag, uint beginLifetime, uint endLifetime, IntPtr data, int dataSize)
        {
            _threadId = threadId;
            _callstackId = callstackId;
            _pageIndex = pageIndex;
            _flag = flag;
            _beginLifetime = beginLifetime;
            _endLifetime = endLifetime;
            _data = data;
            _dataSize = dataSize;
        }

        public SourcePage(uint threadId, uint callstackId, uint pageIndex, PageState flag, uint beginLifetime, uint endLifetime, int dataSize)
        {
            _threadId = threadId;
            _callstackId = callstackId;
            _pageIndex = pageIndex;
            _flag = flag;
            _beginLifetime = beginLifetime;
            _endLifetime = endLifetime;
            _data = Processor.Alloc(dataSize);
            _dataSize = dataSize;
        }

        public uint ThreadId
        {
            get { return _threadId; }
            set { _threadId = value; }
        }

        public uint CallstackId
        {
            get { return _callstackId; }
            set { _callstackId = value; }
        }

        public uint PageIndex
        {
            get { return _pageIndex; }
            set { _pageIndex = value; }
        }

        public uint BeginLifetime
        {
            get { return _beginLifetime; }
            set { _beginLifetime = value; }
        }

        public uint EndLifetime
        {
            get { return _endLifetime; }
            set { _endLifetime = value; }
        }

        public PageState Flag
        {
            get { return _flag; }
            set { _flag = value; }
        }

        public IntPtr Data
        {
            get { return _data; }
            set { _data = value; }
        }

        public int DataSize
        {
            get { return _dataSize; }
            set { _dataSize = value; }
        }

        public bool IsEmpty
        {
            get { return _data == IntPtr.Zero || _dataSize == 0; }
        }

        unsafe public Stream OpenRead()
        {
            return new UnmanagedMemoryStream((byte*)Data.ToPointer(), DataSize);
        }

        unsafe public Stream OpenWrite()
        {
            return new UnmanagedMemoryStream((byte*)Data.ToPointer(), DataSize, DataSize, FileAccess.Write);
        }

        public void Dispose()
        {
            if (Data != IntPtr.Zero)
            {
                Processor.Free(Data);
                Data = IntPtr.Zero;
                DataSize = 0;
            }
        }
    }
}
