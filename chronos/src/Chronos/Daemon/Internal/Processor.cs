using System;
using System.Runtime.InteropServices;

namespace Chronos.Daemon.Internal
{
    internal class Processor : IProcessor
    {
        [DllImport("Chronos.Processor.dll", CharSet = CharSet.Auto, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr Alloc(int length);

        [DllImport("Chronos.Processor.dll", CharSet = CharSet.Auto, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr Free(IntPtr pointer);

        [DllImport("Chronos.Processor.dll", CharSet = CharSet.Auto, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        private static extern ConvertedPage ConvertPage(SourcePage sourcePage);

        [DllImport("Chronos.Processor.dll", CharSet = CharSet.Auto, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        private static extern ConvertedPage MergePage(ConvertedPage convertedPage);

        [DllImport("Chronos.Processor.dll", CharSet = CharSet.Auto, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        private static extern ConvertedPage MergePages(ConvertedPage convertedPage1, ConvertedPage convertedPage2);

        [DllImport("Chronos.Processor.dll", CharSet = CharSet.Auto, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        private static extern ConvertedPage SortPage(ConvertedPage convertedPage);

        [DllImport("Chronos.Processor.dll", CharSet = CharSet.Auto, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        private static extern bool ComparePages(ConvertedPage convertedPage1, ConvertedPage convertedPage2);

        [DllImport("Chronos.Processor.dll", CharSet = CharSet.Auto, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        private static extern void LoadProcessor(uint threadsCount, [MarshalAs(UnmanagedType.FunctionPtr)]OnMergeCompletedCallback callback);

        [DllImport("Chronos.Processor.dll", CharSet = CharSet.Auto, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        private static extern void UnloadProcessor();

        [DllImport("Chronos.Processor.dll", CharSet = CharSet.Auto, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        private static extern void PushPage(SourcePage sourcePage);


        public IConvertedPage ConvertPage(ISourcePage page)
        {
            SourcePage sourcePage = (SourcePage) page;
            ConvertedPage convertedPage = ConvertPage(sourcePage);
            return convertedPage;
        }

        public IConvertedPage MergePage(IConvertedPage page)
        {
            ConvertedPage convertedPage = (ConvertedPage)page;
            ConvertedPage mergedPage = MergePage(convertedPage);
            return mergedPage;
        }

        public IConvertedPage MergePages(IConvertedPage page1, IConvertedPage page2)
        {
            ConvertedPage convertedPage1 = (ConvertedPage)page1;
            ConvertedPage convertedPage2 = (ConvertedPage)page2;
            ConvertedPage mergedPage = MergePages(convertedPage1, convertedPage2);
            return mergedPage;
        }

        public IConvertedPage SortPage(IConvertedPage page)
        {
            ConvertedPage convertedPage = (ConvertedPage) page;
            ConvertedPage sortedPage = SortPage(convertedPage);
            return sortedPage;
        }

        public bool ComparePages(IConvertedPage page1, IConvertedPage page2)
        {
            ConvertedPage convertedPage1 = (ConvertedPage) page1;
            ConvertedPage convertedPage2 = (ConvertedPage)page2;
            return ComparePages(convertedPage1, convertedPage2);
        }


        void IProcessor.LoadProcessor(uint threadsCount, OnMergeCompletedCallback callback)
        {
            LoadProcessor(threadsCount, callback);
        }

        void IProcessor.UnloadProcessor()
        {
            UnloadProcessor();
        }

        public void PushPage(ISourcePage page)
        {
            SourcePage sourcePage = (SourcePage)page;
            PushPage(sourcePage);
        }

        //public NativeArray ConvertSource(NativeArray source)
        //{
        //    if (source.IsEmpty)
        //    {
        //        return NativeArray.Empty;
        //    }
        //    int resultSize = 0;
        //    IntPtr resultPointer = ConvertSource(source.Pointer, source.Size, ref resultSize);
        //    return new NativeArray(resultPointer, resultSize);
        //}

        //public NativeArray MergeCallstack(NativeArray data)
        //{
        //    if (data.IsEmpty)
        //    {
        //        return NativeArray.Empty;
        //    }
        //    int resultSize = 0;
        //    IntPtr resultPointer = MergeCallstack(data.Pointer, data.Size, ref resultSize);
        //    return new NativeArray(resultPointer, resultSize);
        //}

        //public NativeArray SortCallstack(NativeArray callstack)
        //{
        //    if (callstack.IsEmpty)
        //    {
        //        return NativeArray.Empty;
        //    }
        //    IntPtr resultPointer = SortCallstack(callstack.Pointer, callstack.Size);
        //    return new NativeArray(resultPointer, callstack.Size);
        //}

        //public NativeArray ConcatPages(NativeArray data1, NativeArray data2)
        //{
        //    if (data1.IsEmpty && data2.IsEmpty)
        //    {
        //        return NativeArray.Empty;
        //    }
        //    if (data1.IsEmpty)
        //    {
        //        return data2;
        //    }
        //    if (data2.IsEmpty)
        //    {
        //        return data1;
        //    }
        //    int resultSize = 0;
        //    IntPtr resultPointer = ConcatPages(data1.Pointer, data1.Size, data2.Pointer, data2.Size, ref resultSize);
        //    return new NativeArray(resultPointer, resultSize);
        //}

        ////private NativeArray ConvertAndMergePage(NativeArray data)
        ////{
        ////    using (NativeArray convertedData = ConvertPage(data))
        ////    {
        ////        return MergePage(convertedData);
        ////    }
        ////}

        ////public void AppendPage(PagedCallstack callstack, CallstackPage callstackPage)
        ////{
        ////    if (callstackPage.Data.IsEmpty)
        ////    {
        ////        return;
        ////    }
        ////    try
        ////    {
        ////        if (callstack.IsEmpty)
        ////        {
        ////            callstack.Data = ConvertAndMergePage(callstackPage.Data);
        ////        }
        ////        else
        ////        {
        ////            NativeArray concatedPageData;
        ////            using (callstack.Data)
        ////            {
        ////                using (NativeArray convertedPage = ConvertPage(callstackPage.Data))
        ////                {
        ////                    concatedPageData = ConcatPages(callstack.Data, convertedPage);
        ////                }
        ////            }
        ////            callstack.Data = MergePage(concatedPageData);
        ////            concatedPageData.Dispose();
        ////        }
        ////    }
        ////    catch (Exception)
        ////    {
        ////        System.Diagnostics.Debugger.Break();
        ////    }
        ////}

        //public void SortCallstack(PagedCallstack pagedCallstack)
        //{
        //    NativeArray sortedUnmagedData;
        //    using (pagedCallstack.Data)
        //    {
        //        sortedUnmagedData = SortCallstack(pagedCallstack.Data);
        //    }
        //    pagedCallstack.Data = sortedUnmagedData;
        //}

        //public NativeArray MergePages(NativeArray page1, NativeArray page2)
        //{
        //    throw new NotImplementedException();
        //}

        //public bool ComparePages(NativeArray page1, NativeArray page2)
        //{
        //    bool result = ComparePages(page1.Pointer, page1.Size, page2.Pointer, page2.Size);
        //    return result;
        //}
    }
}
