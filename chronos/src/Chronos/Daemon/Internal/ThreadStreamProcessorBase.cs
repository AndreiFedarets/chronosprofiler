using System.Threading;
using Chronos.Core;
using Chronos.Storage;
using Rhiannon.Extensions;

namespace Chronos.Daemon.Internal
{
    internal abstract class ThreadStreamProcessorBase : IThreadStreamProcessor
    {
        protected readonly ICallstackCollection Callstacks;
        protected readonly IFunctionCollection Functions;
        protected readonly ISessionStorage SessionStorage;
        protected readonly IProcessor Processor;
        protected readonly ConcurrentQueue<ISourcePage> Pages;
        protected readonly ConcurrentQueue<IConvertedPage> MergedCallstacks;
        private readonly Thread _mergeProcessThread;
        private readonly Thread _saveProcessThread;
        private volatile bool _merging;
        private volatile bool _saving;

        protected ThreadStreamProcessorBase(ICallstackCollection callstacks, IFunctionCollection functions, IProcessor processor, ISessionStorage sessionStorage)
        {
            _merging = true;
            _saving = true;
            Callstacks = callstacks;
            Processor = processor;
            SessionStorage = sessionStorage;
            Functions = functions;
            Pages = new ConcurrentQueue<ISourcePage>();
            MergedCallstacks = new ConcurrentQueue<IConvertedPage>();
            _mergeProcessThread = new Thread(DoMergeData);
            _saveProcessThread = new Thread(DoSaveData);
            _mergeProcessThread.Start();
            _saveProcessThread.Start();
            //_updatedPagedCallstacks = new ConcurrentQueue<PagedCallstack>();
            //_updating = true;
            //_updateProcessThread = new Thread(UpdateDataInternal);
            //_updateProcessThread.Start();
        }

        public void ProcessPage(ISourcePage sourcePage)
        {
            Pages.Enqueue(sourcePage);
        }

        private void DoMergeData()
        {
            while (true)
            {
                ISourcePage sourcePage = Pages.DequeueOrDefault();
                if (sourcePage == null)
                {
                    if (_merging)
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
                    if (!sourcePage.IsEmpty)
                    {
                        MergeInternal(sourcePage);   
                    }
                }
            }
        }

        protected abstract void MergeInternal(ISourcePage sourcePage);

        private void DoSaveData()
        {
            while (true)
            {
                IConvertedPage callstack = MergedCallstacks.DequeueOrDefault();
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
                        Callstacks.Update(callstackInfo);
                        SessionStorage.WriteCallstack(callstackInfo.Id, callstack.OpenRead());
                    }
                }
            }
        }

        //unsafe private void UpdateDataInternal()
        //{
        //    const int bufferSize = 1000 * IntermediateEvent.ImSize;
        //    byte[] buffer = new byte[bufferSize];
        //    while (true)
        //    {
        //        PagedCallstack pagedCallstack = _updatedPagedCallstacks.DequeueOrDefault();
        //        if (pagedCallstack == null)
        //        {
        //            if (_updating)
        //            {
        //                Thread.Sleep(1);
        //            }
        //            else
        //            {
        //                return;
        //            }
        //        }
        //        else
        //        {
        //            IList<FunctionInfo> updatedFunctions = new List<FunctionInfo>();
        //            using (Stream source = pagedCallstack.OpenRead())
        //            {
        //                int actualSize;
        //                while ((actualSize = source.Read(buffer, 0, buffer.Length)) != 0)
        //                {
        //                    int count = actualSize / IntermediateEvent.ImSize;
        //                    fixed (byte* bufferPointer = buffer)
        //                    {
        //                        byte* tempPointer = bufferPointer;
        //                        for (int i = 0; i < count; i++, tempPointer += IntermediateEvent.ImSize)
        //                        {
        //                            EventType eventType = (EventType)(*tempPointer);
        //                            if (eventType == EventType.FunctionCall)
        //                            {
        //                                uint unit = IntermediateEvent.GetUnit(tempPointer);
        //                                uint hits = IntermediateEvent.GetHits(tempPointer);
        //                                uint time = IntermediateEvent.GetTime(tempPointer);
        //                                FunctionInfo functionInfo = _functions[unit];
        //                                if (functionInfo != null)
        //                                {
        //                                    functionInfo.Hits += hits;
        //                                    functionInfo.TotalTime += time;
        //                                    updatedFunctions.Add(functionInfo);
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //            _functions.Update(updatedFunctions.ToArray());
        //            _closedPagedCallstacks.Enqueue(pagedCallstack);
        //        }
        //    }
        //}

        public void Dispose()
        {
            _merging = false;
            while (_mergeProcessThread.IsAlive)
            {
                Thread.Sleep(10);
            }
            //_updating = false;
            //while (_updateProcessThread.IsAlive)
            //{
            //    Thread.Sleep(10);
            //}
            _saving = false;
            while (_saveProcessThread.IsAlive)
            {
                Thread.Sleep(10);
            }
        }
    }
}
