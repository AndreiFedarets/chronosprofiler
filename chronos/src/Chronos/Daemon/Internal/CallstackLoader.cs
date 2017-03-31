using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Chronos.Core;
using Chronos.Storage;
using Rhiannon.Extensions;
using System;

namespace Chronos.Daemon.Internal
{
	internal class CallstackLoader : MarshalByRefObject, ICallstackLoader
	{
		private readonly IProcessor _processor;
		private readonly ISessionStorage _sessionStorage;

		public CallstackLoader(ISessionStorage sessionStorage, IProcessor processor)
		{
			_sessionStorage = sessionStorage;
			_processor = processor;
		}

		public byte[] LoadCallstacks(IList<CallstackInfo> callstackInfos)
		{
            using (MemoryStream target = new MemoryStream())
            {
                foreach (CallstackInfo callstack in callstackInfos)
                {
                    using (Stream source = _sessionStorage.ReadCallstack(callstack.Id))
                    {
                        source.CopyTo(target);
                    }
                }
                using (IConvertedPage callstack = new ConvertedPage((int)target.Length))
                {
                    Marshal.Copy(target.ToArray(), 0, callstack.Data, callstack.DataSize);
                    using (IConvertedPage mergedCallstack = _processor.MergePage(callstack))
                    {
                        using (IConvertedPage sortedCallstack = _processor.SortPage(mergedCallstack))
                        {
                            if (sortedCallstack.IsEmpty)
                            {
                                return new byte[0];
                            }
                            byte[] result = new byte[sortedCallstack.DataSize];
                            Marshal.Copy(sortedCallstack.Data, result, 0, result.Length);
                            return result;
                        }
                    }
                }
            }
		}

		public override object InitializeLifetimeService()
		{
			return null;
		}
	}
}
