using System.Collections.Generic;
using System.Linq;

namespace Chronos.Core.Internal
{
	internal class ReferencesAnalyzer : IReferencesAnalyzer
	{
		private readonly IProcessShadow _processShadow;

		public ReferencesAnalyzer(IProcessShadow processShadow)
		{
			_processShadow = processShadow;
		}

		public Reference<ProcessInfo, ThreadInfo, ClassInfo, FunctionInfo, IEvent> GetAssemblyReferences(AssemblyInfo assemblyInfo)
		{
			var processReference = new Reference<ProcessInfo, ThreadInfo, ClassInfo, FunctionInfo, IEvent>(_processShadow.ProcessInfo);
			Dictionary<uint, FunctionInfo> functions = assemblyInfo.Functions.ToDictionary(x => x.Id, x=> x);
			foreach (IThreadTrace threadTrace in _processShadow.ThreadTraces)
			{
				Reference<ThreadInfo, ClassInfo, FunctionInfo, IEvent> threadReference = processReference[threadTrace.ThreadInfo];
				FindAssemblyReferences(threadTrace, functions, threadReference);
			}
			return processReference;
		}

		private void FindAssemblyReferences(IEvent current, Dictionary<uint, FunctionInfo> targets, Reference<ThreadInfo, ClassInfo, FunctionInfo, IEvent> reference)
		{
			if (current.EventType == EventType.FunctionCall)
			{
				FunctionInfo functionInfo;
				if (targets.TryGetValue(current.Unit, out functionInfo))
				{
					reference.Add(functionInfo.Class, functionInfo, current);
				}
			}
			foreach (IEvent @event in current.Children)
			{
				FindAssemblyReferences(@event, targets, reference);
			}
		}

        public Reference<ProcessInfo, ThreadInfo, FunctionInfo, IEvent> GetClassReferences(ClassInfo classInfo)
        {
            var processReference = new Reference<ProcessInfo, ThreadInfo, FunctionInfo, IEvent>(_processShadow.ProcessInfo);
            Dictionary<uint, FunctionInfo> functions = classInfo.Functions.ToDictionary(x => x.Id, x => x);
            foreach (IThreadTrace threadTrace in _processShadow.ThreadTraces)
            {
                Reference<ThreadInfo, FunctionInfo, IEvent> threadReference = processReference[threadTrace.ThreadInfo];
                FindClassReferences(threadTrace, functions, threadReference);
            }
            return processReference;
        }

        private void FindClassReferences(IEvent current, Dictionary<uint, FunctionInfo> targets, Reference<ThreadInfo, FunctionInfo, IEvent> reference)
        {
            if (current.EventType == EventType.FunctionCall)
            {
                FunctionInfo functionInfo;
                if (targets.TryGetValue(current.Unit, out functionInfo))
                {
                    reference.Add(functionInfo, current);
                }
            }
            foreach (IEvent @event in current.Children)
            {
                FindClassReferences(@event, targets, reference);
            }
        }

        public Reference<ProcessInfo, ThreadInfo, IEvent> GetFunctionReferences(FunctionInfo functionInfo)
        {
            var processReference = new Reference<ProcessInfo, ThreadInfo, IEvent>(_processShadow.ProcessInfo);
            foreach (IThreadTrace threadTrace in _processShadow.ThreadTraces)
            {
                Reference<ThreadInfo, IEvent> threadReference = processReference[threadTrace.ThreadInfo];
                FindFunctionReferences(threadTrace, functionInfo, threadReference);
            }
            return processReference;
        }

        private void FindFunctionReferences(IEvent current, FunctionInfo target, Reference<ThreadInfo, IEvent> reference)
        {
            if (current.EventType == EventType.FunctionCall)
            {
                if (current.Unit == target.Id)
                {
                    reference.Add(current);
                }
            }
            foreach (IEvent @event in current.Children)
            {
                FindFunctionReferences(@event, target, reference);
            }
        }

    }
}
