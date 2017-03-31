using System;
using System.Collections.Generic;
using Chronos.Core;

namespace Chronos.Daemon.Internal
{
	internal class UnitStreamPool
	{
		private readonly Guid _daemonToken;
		private readonly IDictionary<uint, IDisposable> _streams;
		private readonly IProcessShadow _processShadow;

		public UnitStreamPool(Guid daemonToken, IProcessShadow processShadow)
		{
			_daemonToken = daemonToken;
			_processShadow = processShadow;
			_streams = new Dictionary<uint, IDisposable>();
			Initialize();
		}

		private void Initialize()
		{
            _streams.Add((uint)UnitType.AppDomain, new UnitStream<AppDomainInfo>(_daemonToken, _processShadow.AppDomains));
            _streams.Add((uint)UnitType.Assembly, new UnitStream<AssemblyInfo>(_daemonToken, _processShadow.Assemblies));
            _streams.Add((uint)UnitType.Module, new UnitStream<ModuleInfo>(_daemonToken, _processShadow.Modules));
            _streams.Add((uint)UnitType.Class, new UnitStream<ClassInfo>(_daemonToken, _processShadow.Classes));
            _streams.Add((uint)UnitType.Function, new UnitStream<FunctionInfo>(_daemonToken, _processShadow.Functions));
            _streams.Add((uint)UnitType.Thread, new UnitStream<ThreadInfo>(_daemonToken, _processShadow.Threads));
            _streams.Add((uint)UnitType.Exception, new UnitStream<ExceptionInfo>(_daemonToken, _processShadow.Exceptions));
            _streams.Add((uint)UnitType.SqlRequest, new UnitStream<SqlRequestInfo>(_daemonToken, _processShadow.SqlRequests));
		}


		public void Dispose()
		{
			foreach (IDisposable stream in _streams.Values)
			{
				stream.Dispose();
			}
			_streams.Clear();
		}
	}
}
