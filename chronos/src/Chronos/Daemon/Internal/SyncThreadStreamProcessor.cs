using Chronos.Core;
using Chronos.Storage;
using Rhiannon.Extensions;

namespace Chronos.Daemon.Internal
{
    internal class SyncThreadStreamProcessor : ThreadStreamProcessorBase
	{
        private readonly ConcurrentDictionary<uint, IConvertedPage> _openedCallstacks;

		public SyncThreadStreamProcessor(ICallstackCollection callstacks, IFunctionCollection functions, IProcessor processor, ISessionStorage sessionStorage)
            : base(callstacks, functions, processor, sessionStorage)
		{
            _openedCallstacks = new ConcurrentDictionary<uint, IConvertedPage>();
		}

        protected override void MergeInternal(ISourcePage sourcePage)
        {
            IConvertedPage originalCallstack;
            using (sourcePage)
            {
                using (IConvertedPage convertedPage = Processor.ConvertPage(sourcePage))
                {
                    if (_openedCallstacks.TryGetValue(sourcePage.ThreadId, out originalCallstack))
                    {
                        IConvertedPage mergedCallstack;
                        using (originalCallstack)
                        {
                            mergedCallstack = Processor.MergePages(originalCallstack, convertedPage);
                        }
                        originalCallstack = mergedCallstack;
                        _openedCallstacks[sourcePage.ThreadId] = originalCallstack;
                    }
                    else
                    {
                        originalCallstack = Processor.MergePage(convertedPage);
                        _openedCallstacks.Add(sourcePage.ThreadId, originalCallstack);
                    }   
                }
                switch ((PageState)sourcePage.Flag)
                {
                    case PageState.Close:
                        _openedCallstacks.Remove(originalCallstack.ThreadId);
                        //UpdatedCallstacks.Enqueue(pagedCallstack);
                        MergedCallstacks.Enqueue(originalCallstack);
                        break;
                    case PageState.Break:
                        _openedCallstacks.Remove(originalCallstack.ThreadId);
                        originalCallstack.Dispose();
                        break;
                }
            }
        }
    }
}
