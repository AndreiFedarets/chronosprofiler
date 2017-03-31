using System.Threading;
using Chronos.Core;
using Chronos.Storage;
using Rhiannon.Extensions;

namespace Chronos.Daemon.Internal
{
    internal class NativeThreadStreamProcessor : IThreadStreamProcessor
    {
        private readonly ICallstackCollection _callstacks;
        private readonly ISessionStorage _sessionStorage;
        private readonly IFunctionCollection _functions;
        private readonly IProcessor _processor;
        private volatile bool _saving;
        private readonly ConcurrentQueue<IConvertedPage> _mergedCallstacks;
        private readonly Thread _saveProcessThread;
        private readonly OnMergeCompletedCallback _onMergeCompleted;

        public NativeThreadStreamProcessor(ICallstackCollection callstacks, IFunctionCollection functions, IProcessor processor, ISessionStorage sessionStorage, uint threadMergersCount)
        {
            _callstacks = callstacks;
            _sessionStorage = sessionStorage;
            _functions = functions;
            _saving = true;
            _processor = processor;
            _onMergeCompleted = new OnMergeCompletedCallback(OnMergeCompleted);
            _mergedCallstacks = new ConcurrentQueue<IConvertedPage>();
            _processor.LoadProcessor(threadMergersCount, _onMergeCompleted);
            _saveProcessThread = new Thread(DoSaveData);
            _saveProcessThread.Start();
        }

        public void ProcessPage(ISourcePage sourcePage)
        {
            _processor.PushPage(sourcePage);
        }

        private void OnMergeCompleted(ConvertedPage convertedPage)
        {
            _mergedCallstacks.Enqueue(convertedPage);
        }

        private void DoSaveData()
        {
            while (true)
            {
                IConvertedPage callstack = _mergedCallstacks.DequeueOrDefault();
                if (callstack == null)
                {
                    if (_saving)
                    {
                        Thread.Sleep(1);
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    using (callstack)
                    {
                        CallstackInfo callstackInfo = callstack.GetInfo();
                        _callstacks.Update(callstackInfo);
                        _sessionStorage.WriteCallstack(callstackInfo.Id, callstack.OpenRead());
                    }
                }
            }
        }

        public void Dispose()
        {
            //TODO: Get state of pageProcessor and wait until all pages merging
            _saving = false;
            while (_saveProcessThread.IsAlive)
            {
                Thread.Sleep(10);
            }
            _processor.UnloadProcessor();
        }
    }
}
