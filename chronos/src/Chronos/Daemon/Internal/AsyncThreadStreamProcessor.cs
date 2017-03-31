using System;
using Chronos.Core;
using Chronos.Storage;
using Rhiannon.Extensions;

namespace Chronos.Daemon.Internal
{
    internal class AsyncThreadStreamProcessor : ThreadStreamProcessorBase
    {
        private readonly ConcurrentDictionary<uint, CallstackPageCollection> _activeMergeCollections;
        private readonly ConcurrentDictionary<Guid, CallstackPageCollection> _workingMergeCollections;
        private readonly IWorker _worker;

        public AsyncThreadStreamProcessor(ICallstackCollection callstacks, IFunctionCollection functions, IProcessor processor, ISessionStorage sessionStorage)
            : base(callstacks, functions, processor, sessionStorage)
        {
            _activeMergeCollections = new ConcurrentDictionary<uint, CallstackPageCollection>();
            _workingMergeCollections = new ConcurrentDictionary<Guid, CallstackPageCollection>();
            _worker = new Worker();
        }

        //private readonly ConcurrentDictionary<uint, uint> _dirs = new ConcurrentDictionary<uint, uint>();


        protected override void MergeInternal(ISourcePage sourcePage)
        {
            //uint callstackIndex;
            //if (!_dirs.TryGetValue(callstackPage.ThreadId, out callstackIndex))
            //{
            //    callstackIndex = 0;
            //    _dirs.Add(callstackPage.ThreadId, callstackIndex);
            //}
            //string dirPath = string.Format(@"C:\chronos_test\{0}\{1}\", callstackPage.ThreadId, callstackIndex);
            //if (!Directory.Exists(dirPath))
            //{
            //    Directory.CreateDirectory(dirPath);
            //}
            //string filePath = Path.Combine(dirPath, callstackPage.PageIndex.ToString("D4"));
            //using (FileStream fileInfo = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            //{
            //    using (callstackPage)
            //    {
            //        Stream source = callstackPage.OpenRead();
            //        source.CopyTo(fileInfo);
            //    }
            //}
            //switch ((CallstackPageState)callstackPage.Flag)
            //{
            //    case CallstackPageState.Close:
            //        _dirs[callstackPage.ThreadId] = _dirs[callstackPage.ThreadId] + 1;
            //        break;
            //}


            CallstackPageCollection collection;
            if (!_activeMergeCollections.TryGetValue(sourcePage.ThreadId, out collection))
            {
                collection = new CallstackPageCollection(sourcePage.ThreadId, SaveCallstack, _worker, Processor);
                _activeMergeCollections.Add(sourcePage.ThreadId, collection);
                _workingMergeCollections.Add(collection.Id, collection);
            }
            collection.Insert(sourcePage);
            switch ((PageState)sourcePage.Flag)
            {
                case PageState.Close:
                    _activeMergeCollections.Remove(collection.ThreadId);
                    break;
                case PageState.Break:
                    _activeMergeCollections.Remove(collection.ThreadId);
                    collection.Dispose();
                    break;
            }
        }

        private void SaveCallstack(Guid id, IConvertedPage callstack)
        {
            _workingMergeCollections.Remove(id);
            MergedCallstacks.Enqueue(callstack);
        }
    }
}
