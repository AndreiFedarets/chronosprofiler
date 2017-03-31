using System;

namespace Chronos.Daemon.Internal
{
    internal class CallstackPageCollection : IDisposable
    {
        public const uint LevelDefaultSize = 512;
        private readonly uint _treeLevel;
        private readonly object _nextLevelLock;
        private readonly IProcessor _processor;
        private readonly IWorker _worker;
        private IConvertedPage[] _pages;
        private volatile CallstackPageCollection _nextLevel;
        private readonly uint _levelOriginalSize;
        private readonly Action<Guid, IConvertedPage> _onMergeDone;

        public CallstackPageCollection(uint threadId, uint treeLevel, uint levelSize, Guid id, Action<Guid, IConvertedPage> onMergeDone, IWorker worker, IProcessor processor)
        {
            _treeLevel = treeLevel;
            _nextLevelLock = new object();
            _levelOriginalSize = levelSize;
            _pages = new IConvertedPage[levelSize];
            _worker = worker;
            _onMergeDone = onMergeDone;
            _processor = processor;
            ThreadId = threadId;
            Id = id;
        }

        public CallstackPageCollection(uint threadId, Action<Guid, IConvertedPage> onMergeDone, IWorker worker, IProcessor processor)
            : this(threadId, 0, LevelDefaultSize, Guid.NewGuid(), onMergeDone, worker, processor)
        {
        }

        public Guid Id { get; private set; }

        public uint ThreadId { get; private set; }

        private CallstackPageCollection NextLevel
        {
            get
            {
                if (_nextLevel == null)
                {
                    lock (_nextLevelLock)
                    {
                        if (_nextLevel == null)
                        {
                            uint levelSize = (uint) _pages.Length/2;
                            _nextLevel = new CallstackPageCollection(ThreadId, _treeLevel + 1, levelSize, Id, _onMergeDone, _worker, _processor);
                        }
                    }
                }
                return _nextLevel;
            }
        }

        private void InsertInternal(IConvertedPage page)
        {
            if (GetPageIsRoot(page))
            {
                OnMergeDone(page);
                return;
            }
            lock (_pages)
            {
                uint requiredLength = page.PageIndex + 1;
                if (_pages.Length <= requiredLength)
                {
                    ResizeLevel(requiredLength);
                }
            }
            //Node is left-hand node
            if (GetPageHand(page) == 0)
            {
                if ((PageState)page.Flag == PageState.Close)
                {
                    Task task = new Task(() => Merge(page));
                    _worker.Enqueue(task);
                }
                else
                {
                    uint rightHandPageIndex = page.PageIndex + 1;
                    IConvertedPage rightHandPage = _pages[rightHandPageIndex];
                    //right-hand node is not exist, we cannot start merging - save node to _nodes
                    if (rightHandPage == null)
                    {
                        _pages[page.PageIndex] = page;
                    }
                    //right-hand node exists, we can start merging
                    else
                    {
                        _pages[rightHandPageIndex] = null;
                        Task task = new Task(() => Merge(page, rightHandPage));
                        _worker.Enqueue(task);
                    }
                }
            }
            //Node is right-hand node
            else
            {
                uint leftHandPageIndex = page.PageIndex - 1;
                IConvertedPage leftHandPage = _pages[leftHandPageIndex];
                //left-hand node is not exist, we cannot start merging - save node to _nodes
                if (leftHandPage == null)
                {
                    _pages[page.PageIndex] = page;
                }
                //left-hand node exists, we can start merging
                else
                {
                    _pages[leftHandPageIndex] = null;
                    Task task = new Task(() => Merge(leftHandPage, page));
                    _worker.Enqueue(task);
                }
            }
        }

        public void Insert(ISourcePage page)
        {
            Task task = new Task(() => Convert(page));
            _worker.Enqueue(task);
        }

        private void OnMergeDone(IConvertedPage page)
        {
            _onMergeDone(Id, page);
        }

        private bool GetPageIsRoot(IConvertedPage page)
        {
            return page.Flag == PageState.Close && page.PageIndex == 0;
        }

        private uint GetPageHand(IConvertedPage page)
        {
            return page.PageIndex%2;
        }

        private void ResizeLevel(uint requiredSize)
        {
            uint newSize = (uint)_pages.Length + _levelOriginalSize;
            if (requiredSize <= newSize)
            {
                newSize = requiredSize + 1;
            }
            IConvertedPage[] pages = new IConvertedPage[newSize];
            for (int i = 0; i < _pages.Length; i++)
            {
                pages[i] = _pages[i];
            }
            _pages = pages;
        }

        private void Convert(ISourcePage sourcePage)
        {
            IConvertedPage mergedPage;
            using (sourcePage)
            {
                IConvertedPage convertedPage;
                convertedPage = _processor.ConvertPage(sourcePage);
                using (convertedPage)
                {
                    mergedPage = _processor.MergePage(convertedPage);
                }
            }
            InsertInternal(mergedPage);
        }

        private void Merge(IConvertedPage leftHandPage, IConvertedPage rightHandPage)
        {
            IConvertedPage mergedPage;
            using (leftHandPage)
            {
                using (rightHandPage)
                {
                    mergedPage = _processor.MergePages(leftHandPage, rightHandPage);
                }
            }
            mergedPage.PageIndex = ReindexForNextLevel(leftHandPage.PageIndex);
            NextLevel.InsertInternal(mergedPage);
        }

        private void Merge(IConvertedPage page)
        {
            if (page.IsEmpty)
            {
                return;
            }
            IConvertedPage mergedPage;
            using (page)
            {
                mergedPage = _processor.MergePage(page);
            }
            mergedPage.PageIndex = ReindexForNextLevel(page.PageIndex);
            NextLevel.InsertInternal(mergedPage);
        }

        private uint ReindexForNextLevel(uint thisLevelIndex)
        {
            return thisLevelIndex / 2;
        }

        public void Dispose()
        {
            //we should stop all merge tasks
        }
    }
}
