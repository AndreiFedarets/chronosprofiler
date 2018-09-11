using Chronos.Storage;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Chronos.Common.EventsTree
{
    internal sealed class EventTreeCollection : RemoteBaseObject, IEventTreeCollection
    {
        private readonly Dictionary<ulong, ISingleEventTree> _dictionaryByUid;
        private List<ISingleEventTree> _collector;

        public EventTreeCollection()
        {
            _collector = new List<ISingleEventTree>();
            _dictionaryByUid = new Dictionary<ulong, ISingleEventTree>();
        }

        public event EventHandler<EventTreeEventArgs> CollectionUpdated;

        internal void SetDependencies(IDataStorage storage)
        {
            ReadOnly = true;
        }

        internal bool ReadOnly { get; set; }

        public IEnumerator<ISingleEventTree> GetEnumerator()
        {
            List<ISingleEventTree> items;
            lock (_dictionaryByUid)
            {
                items = new List<ISingleEventTree>(_dictionaryByUid.Values);
            }
            return items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        internal void Add(EventPageHeader pageHeader, byte[] pageData)
        {
            SingleEventTree eventTree = new SingleEventTree(pageHeader.EventsTreeGlobalId, pageHeader.ThreadUid, pageHeader.ThreadOsId,
                                                pageHeader.BeginLifetime, pageHeader.EndLifetime, pageData);
            lock (_collector)
            {
                _collector.Add(eventTree);
            }
        }

        internal void FlushData()
        {
            List<ISingleEventTree> collector;
            lock (_collector)
            {
                collector = _collector;
                _collector = new List<ISingleEventTree>();
            }
            lock (_dictionaryByUid)
            {
                foreach (ISingleEventTree eventTree in collector)
                {
                    _dictionaryByUid.Add(eventTree.EventTreeUid, eventTree);
                }
            }
            EventHandler<EventTreeEventArgs> handler = CollectionUpdated;
            if (handler != null)
            {
                handler(this, new EventTreeEventArgs(collector));
            }
        }

        //EventTreeHeader header = new EventTreeHeader(pageHeader.ThreadId, pageHeader.EventsTreeGlobalId,
        //                                          pageHeader.BeginLifetime, pageHeader.EndLifetime,
        //                                          pageHeader.EventsDataSize);
        //EventTreeData data = new EventTreeData(header.EventsTreeId, pageData);
        //_headerTable.Add(header);
        //_dataTable.Add(data);
        //lock (_dictionary)
        //{
        //    _dictionary[header.EventsTreeId] = header;
        //}

        //public byte[] GetMergedTreesData(ulong[] treesIds)
        //{
        //    if (treesIds.Length == 0)
        //    {
        //        return new byte[0];
        //    }
        //    List<EventsTreeData> datas = _dataTable.Select(treesIds.Select(x => (object)x)).ToList();
        //    if (datas.Count == 1)
        //    {
        //        return datas.First().Data;
        //    }
        //    if (datas.Count == 0)
        //    {
        //        return new byte[0];
        //    }
        //    byte[] result;
        //    using (MemoryStream memoryStream = new MemoryStream())
        //    {
        //        foreach (EventsTreeData data in datas)
        //        {
        //            memoryStream.Write(data.Data, 0, data.Data.Length);
        //        }
        //        byte[] source = memoryStream.ToArray();
        //        //result = _agentLibrary.MergeEventsTrees(source);
        //        result = source;
        //    }
        //    return result;
        //}

        //public void Refresh()
        //{
        //    throw new System.NotImplementedException();
        //}
    }
}
