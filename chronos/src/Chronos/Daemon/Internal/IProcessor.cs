using System.Runtime.InteropServices;

namespace Chronos.Daemon.Internal
{
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    internal delegate void OnMergeCompletedCallback(ConvertedPage convertedPage);

    internal interface IProcessor
    {
        IConvertedPage ConvertPage(ISourcePage page);

        IConvertedPage MergePage(IConvertedPage page);

        IConvertedPage MergePages(IConvertedPage page1, IConvertedPage page2);

        IConvertedPage SortPage(IConvertedPage page);

        void LoadProcessor(uint threadsCount, OnMergeCompletedCallback callback);

        void UnloadProcessor();

        void PushPage(ISourcePage sourcePage);
    }
}
