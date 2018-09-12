using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace Chronos.Common.EventsTree
{
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    internal delegate void EventsTreeMergeCompletedCallback(IntPtr buffer, int size);

    internal sealed class AgentLibrary : Win32.UnmangedLibrary
    {
        private const string AgentLibraryName = "Chronos.Common.EventsTree.Agent.dll";

        protected override string GetLibraryFullName()
        {
            string path = GetType().GetAssemblyPath();
            string fullName = Path.Combine(path, AgentLibraryName);
            return fullName;
        }

        public IntPtr CreateDataHandler(EventsTreeMergeCompletedCallback callback)
        {
            CreateDataHandlerFunction function = GetFunction<CreateDataHandlerFunction>("DataHandler_Create");
            return function(callback);
        }

        public void DestroyDataHandler(IntPtr handlerToken)
        {
            DestroyDataHandlerFunction function = GetFunction<DestroyDataHandlerFunction>("DataHandler_Destroy");
            function(handlerToken);
        }

        //public unsafe byte[] MergeEventTrees(byte[] source)
        //{
        //    MergeEventTreeMergerFunction function = GetFunction<MergeEventTreeMergerFunction>("EventTreeMerger_Merge");
        //    IntPtr nativeResult = IntPtr.Zero;
        //    int resultSize = 0;
        //    function(source, source.Length, ref nativeResult, ref resultSize);
        //    using (UnmanagedMemoryStream nativeStream = new UnmanagedMemoryStream((byte*)nativeResult.ToPointer(), resultSize))
        //    {
        //        using (MemoryStream memoryStream = new MemoryStream())
        //        {
        //            nativeStream.CopyTo(memoryStream);
        //            return memoryStream.ToArray();
        //        }
        //    }
        //}

        public unsafe byte[] MergeEventTrees(IEnumerable<byte[]> source, byte[] rootEvent = null)
        {
            byte[] combinedSource;
            using (MemoryStream sourceStream = new MemoryStream())
            {
                foreach (byte[] data in source)
                {
                    sourceStream.Write(data, 0, data.Length);
                }
                combinedSource = sourceStream.ToArray();
            }
            MergeEventTreeMergerFunction function = GetFunction<MergeEventTreeMergerFunction>("EventTreeMerger_Merge");
            IntPtr nativeResult = IntPtr.Zero;
            int resultSize = 0;
            int rootEventSize = rootEvent == null ? 0 : rootEvent.Length;
            function(combinedSource, combinedSource.Length, rootEvent, rootEventSize, ref nativeResult, ref resultSize);
            using (UnmanagedMemoryStream nativeStream = new UnmanagedMemoryStream((byte*)nativeResult.ToPointer(), resultSize))
            {
                using (MemoryStream resultStream = new MemoryStream())
                {
                    nativeStream.CopyTo(resultStream);
                    return resultStream.ToArray();
                }
            }
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr CreateDataHandlerFunction([MarshalAs(UnmanagedType.FunctionPtr)]EventsTreeMergeCompletedCallback callback);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DestroyDataHandlerFunction(IntPtr handlerToken);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void MergeEventTreeMergerFunction(byte[] source, int sourceSize, byte[] rootEvent, int rootEventSize, ref IntPtr result, ref int resultSize);
    }
}
