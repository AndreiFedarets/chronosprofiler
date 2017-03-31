using System;
using System.IO;
using System.Runtime.InteropServices;
using Chronos.Core;

namespace Chronos.Daemon.Internal
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct ConvertedPage : IConvertedPage
    {
        private uint _threadId;
        private uint _callstackId;
        private uint _pageIndex;
        private uint _beginPageRange;
        private uint _endPageRange;
        private uint _beginLifetime;
        private uint _endLifetime;
        private PageState _flag;
        private IntPtr _data;
        private int _dataSize;
        private IntPtr _rootEvent;

        public ConvertedPage(int dataSize)
        {
            _threadId = 0;
            _callstackId = 0;
            _pageIndex = 0;
            _beginPageRange = 0;
            _endPageRange = 0;
            _beginLifetime = 0;
            _endLifetime = 0;
            _flag = PageState.Close;
            _data = Processor.Alloc(dataSize);
            _dataSize = dataSize;
            _rootEvent = IntPtr.Zero;
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

        public uint BeginPageRange
        {
            get { return _beginPageRange; }
            set { _beginPageRange = value; }
        }

        public uint EndPageRange
        {
            get { return _endPageRange; }
            set { _endPageRange = value; }
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
        
        public IntPtr RootEvent
        {
            get { return _rootEvent; }
            set { _rootEvent = value; }
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

        public CallstackInfo GetInfo()
        {
            uint index = CallstackCollection.GenerateCallstackId();
            //TODO: Move to constants
            byte[] rootEvent = new byte[15];
            Marshal.Copy(RootEvent, rootEvent, 0, rootEvent.Length);
            return new CallstackInfo(index, ThreadId, rootEvent, BeginLifetime, EndLifetime);
        }

        public void Dispose()
        {
            if (Data != IntPtr.Zero)
            {
                Processor.Free(Data);
                Data = IntPtr.Zero;
                DataSize = 0;
            }
            if (RootEvent != IntPtr.Zero)
            {
                Processor.Free(RootEvent);
                RootEvent = IntPtr.Zero;
            }
        }

        public static bool operator ==(ConvertedPage page1, ConvertedPage page2)
        {
            return page1.Equals(page2);
        }

        public static bool operator !=(ConvertedPage page1, ConvertedPage page2)
        {
            return !(page1 == page2);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;   
            }
            if (obj.GetType() != typeof(ConvertedPage))
            {
                return false;
            }
            return Equals((ConvertedPage) obj);
        }

        public bool Equals(ConvertedPage other)
        {
            return other.ThreadId == ThreadId && other.BeginPageRange == BeginPageRange && other.EndPageRange == EndPageRange && other.Flag == Flag && other.Data.Equals(Data) && other.DataSize == DataSize;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = ThreadId.GetHashCode();
                result = (result*397) ^ BeginPageRange.GetHashCode();
                result = (result*397) ^ EndPageRange.GetHashCode();
                result = (result*397) ^ Flag.GetHashCode();
                result = (result*397) ^ Data.GetHashCode();
                result = (result*397) ^ DataSize;
                return result;
            }
        }
    }
}
