using System.Collections.Generic;
using System.Linq;

namespace Chronos.Core
{
	public class Reference<T1>
	{
		public Reference(T1 item)
		{
			Item = item;
		}

        public virtual long Count
	    {
	        get { return 1; }
	    }

		public T1 Item { get; private set; }
	}

	public class Reference<T1, T2> : Reference<T1>
	{
		private readonly Dictionary<T2, Reference<T2>> _collection;

		public Reference(T1 item)
			: base(item)
		{
			_collection = new Dictionary<T2, Reference<T2>>();
		}

        public override long Count
        {
            get { return Collection.Sum(x => x.Count); }
        }

		public IEnumerable<Reference<T2>> Collection
		{
			get { return _collection.Values; }
		}

		public Reference<T2> this[T2 item2]
		{
			get
			{
				Reference<T2> reference;
				if (!_collection.TryGetValue(item2, out reference))
				{
					reference = new Reference<T2>(item2);
					_collection.Add(item2, reference);
				}
				return reference;
			}
		}

		public void Add(T2 item2)
        {
            Reference<T2> reference;
            if (_collection.TryGetValue(item2, out reference))
            {

            }
            else
			{
				_collection.Add(item2, new Reference<T2>(item2));
            }
        }
	}

	public class Reference<T1, T2, T3> : Reference<T1>
	{
		private readonly Dictionary<T2, Reference<T2, T3>> _collection;

		public Reference(T1 item)
			: base(item)
		{
			_collection = new Dictionary<T2, Reference<T2, T3>>();
		}

        public override long Count
        {
            get { return Collection.Sum(x => x.Count); }
        }

		public IEnumerable<Reference<T2, T3>> Collection
		{
			get { return _collection.Values; }
		}

		public Reference<T2, T3> this[T2 item2]
		{
			get
			{
				Reference<T2, T3> reference;
				if (!_collection.TryGetValue(item2, out reference))
				{
					reference = new Reference<T2, T3>(item2);
					_collection.Add(item2, reference);
				}
				return reference;
			}
		}

		public void Add(T2 item2, T3 item3)
		{
			Reference<T2, T3> reference;
			if (_collection.TryGetValue(item2, out reference))
			{
				reference.Add(item3);
			}
			else
			{
				reference = new Reference<T2, T3>(item2);
				reference.Add(item3);
				_collection.Add(item2, reference);
			}
		}
	}

	public class Reference<T1, T2, T3, T4> : Reference<T1>
	{
		private readonly Dictionary<T2, Reference<T2, T3, T4>> _collection;

		public Reference(T1 item)
			: base(item)
		{
			_collection = new Dictionary<T2, Reference<T2, T3, T4>>();
		}

        public override long Count
        {
            get { return Collection.Sum(x => x.Count); }
        }

		public IEnumerable<Reference<T2, T3, T4>> Collection
		{
			get { return _collection.Values; }
		}

		public Reference<T2, T3, T4> this[T2 item2]
		{
			get
			{
				Reference<T2, T3, T4> reference;
				if (!_collection.TryGetValue(item2, out reference))
				{
					reference = new Reference<T2, T3, T4>(item2);
					_collection.Add(item2, reference);
				}
				return reference;
			}
		}

		public void Add(T2 item2, T3 item3, T4 item4)
		{
			Reference<T2, T3, T4> reference;
			if (_collection.TryGetValue(item2, out reference))
			{
				reference.Add(item3, item4);
			}
			else
			{
				reference = new Reference<T2, T3, T4>(item2);
				reference.Add(item3, item4);
				_collection.Add(item2, reference);
			}
		}
	}

	public class Reference<T1, T2, T3, T4, T5> : Reference<T1>
	{
		private readonly Dictionary<T2, Reference<T2, T3, T4, T5>> _collection;

		public Reference(T1 item)
			: base(item)
		{
			_collection = new Dictionary<T2, Reference<T2, T3, T4, T5>>();
		}

        public override long Count
        {
            get { return Collection.Sum(x => x.Count); }
        }

		public IEnumerable<Reference<T2, T3, T4, T5>> Collection
		{
			get { return _collection.Values; }
		}

		public Reference<T2, T3, T4, T5> this[T2 item2]
		{
			get
			{ 
				Reference<T2, T3, T4, T5> reference;
				if (!_collection.TryGetValue(item2, out reference))
				{
					reference = new Reference<T2, T3, T4, T5>(item2);
					_collection.Add(item2, reference);
				}
				return reference;
			}
		}

		public void Add(T2 item2, T3 item3, T4 item4, T5 item5)
		{
			Reference<T2, T3, T4, T5> reference;
			if (_collection.TryGetValue(item2, out reference))
			{
				reference.Add(item3, item4, item5);
			}
			else
			{
				reference = new Reference<T2, T3, T4, T5>(item2);
				reference.Add(item3, item4, item5);
				_collection.Add(item2, reference);
			}
		}
	}







    //public class Reference<T1, T2, T3, T4> : Reference<T1, Reference<T2, Reference<T3, T4>>>
    //{
    //    public Reference(T1 item)
    //        : base(item)
    //    {
    //    }

    //    public Reference<T2, T3, T4> Add(T2 item)
    //    {
    //        return new Reference<T2, T3, T4>(item);
    //    }
    //}

    //public class Reference<T1, T2, T3, T4, T5> : Reference<T1, Reference<T2, Reference<T3, Reference<T4, T5>>>>
    //{
    //    public Reference(T1 item)
    //        : base(item)
    //    {
    //    }

    //    public Reference<T2, Reference<T3, Reference<T4, T5>>> Add(T2 item)
    //    {
    //        Reference<T2, Reference<T3, Reference<T4, T5>>> reference = new Reference<T2, Reference<T3, Reference<T4, T5>>>(item);
    //        _references.Add(reference);
    //        return reference;
    //    }

    //    public Reference<T2, Reference<T3, Reference<T4, T5>>> Get(T2 item)
    //    {
    //        Reference<T2, Reference<T3, Reference<T4, T5>>> reference = _references.FirstOrDefault(x => (object)x.Item == (object)item);
    //        return reference;
    //    }
    //}

    //public class ProcessThreadClassFunctionEventReference : Reference<ProcessInfo, ThreadClassFunctionEventReference>
    //{
    //    public ProcessThreadClassFunctionEventReference(ProcessInfo item)
    //        : base(item)
    //    {
    //    }
    //}

    //public class ThreadClassFunctionEventReference : Reference<ThreadInfo, ClassFunctionEventReference>
    //{
    //    public ThreadClassFunctionEventReference(ThreadInfo item)
    //        : base(item)
    //    {
    //    }
    //}

    //public class ClassFunctionEventReference : Reference<ClassInfo, FunctionEventReference>
    //{
    //    public ClassFunctionEventReference(ClassInfo item)
    //        : base(item)
    //    {
    //    }
    //}

    //public class FunctionEventReference : Reference<FunctionInfo, EventReference>
    //{
    //    public FunctionEventReference(FunctionInfo item)
    //        : base(item)
    //    {
    //    }
    //}

    //public class EventReference : Reference<IEvent>
    //{
    //    public EventReference(IEvent item)
    //        : base(item)
    //    {
    //    }
    //}

	

}
