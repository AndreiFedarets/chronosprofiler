namespace Chronos.Core
{
	public interface IReferencesAnalyzer
	{
		Reference<ProcessInfo, ThreadInfo, ClassInfo, FunctionInfo, IEvent> GetAssemblyReferences(AssemblyInfo assemblyInfo);

        Reference<ProcessInfo, ThreadInfo, FunctionInfo, IEvent> GetClassReferences(ClassInfo classInfo);

        Reference<ProcessInfo, ThreadInfo, IEvent> GetFunctionReferences(FunctionInfo functionInfo);
    }
}
