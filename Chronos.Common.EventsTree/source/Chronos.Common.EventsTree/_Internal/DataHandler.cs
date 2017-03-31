using System;
using System.IO;
using System.Runtime.InteropServices;
using Chronos.Communication.Native;
using Chronos.Win32;
using System.Diagnostics;

namespace Chronos.Common.EventsTree
{
    internal sealed class DataHandler : INativeDataHandler
    {
        private readonly ManagedCallbackHolder<EventsTreeMergeCompletedCallback> _callbackHolder;
        private readonly AgentLibrary _agentLibrary;
        private readonly EventTreeCollection _eventsTrees;
        private IntPtr _handlerToken;

        public DataHandler(AgentLibrary agentLibrary, EventTreeCollection eventsTrees)
        {
            _eventsTrees = eventsTrees;
            _agentLibrary = agentLibrary;
            _callbackHolder = new ManagedCallbackHolder<EventsTreeMergeCompletedCallback>(OnMergeCompleted);
            _handlerToken = _agentLibrary.CreateDataHandler(_callbackHolder.Callback);
        }

        public IntPtr DataHandlerPointer
        {
            get { return _handlerToken; }
        }

        public void Dispose()
        {
            if (_handlerToken != IntPtr.Zero)
            {
                _callbackHolder.Dispose();
                _agentLibrary.DestroyDataHandler(_handlerToken);
                _handlerToken = IntPtr.Zero;
            }
        }

        private unsafe void OnMergeCompleted(IntPtr buffer, int size)
        {
            EventPageHeader header = (EventPageHeader)Marshal.PtrToStructure(buffer, typeof(EventPageHeader));
            try
            {
                using (UnmanagedMemoryStream bufferStream = new UnmanagedMemoryStream((byte*)buffer.ToPointer(), size))
                {
                    bufferStream.Seek(sizeof(EventPageHeader), SeekOrigin.Begin);
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        bufferStream.CopyTo(memoryStream);
                        _eventsTrees.Add(header, memoryStream.ToArray());
                    }
                }
            }
            catch (Exception exception)
            {
                LoggingProvider.Current.Log(TraceEventType.Error, exception);
                throw;
            }
            finally
            {
                NativeMethods.Free(buffer);
            }
        }
    }
}
