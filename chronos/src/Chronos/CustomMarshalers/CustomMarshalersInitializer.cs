using Rhiannon.Serialization.Marshaling;

namespace Chronos.CustomMarshalers
{
	public static class CustomMarshalersInitializer
	{
		public static void Initialize()
		{
			//MarshalingManager.RegisterMarshaler(new CounterPointMarshaller());
			MarshalingManager.RegisterMarshaler(new AppDomainInfoMarshaller());
			MarshalingManager.RegisterMarshaler(new AssemblyInfoMarshaller());
			MarshalingManager.RegisterMarshaler(new ModuleInfoMarshaller());
			MarshalingManager.RegisterMarshaler(new ClassInfoMarshaller());
			MarshalingManager.RegisterMarshaler(new FunctionInfoMarshaller());
			MarshalingManager.RegisterMarshaler(new ExceptionInfoMarshaller());
			MarshalingManager.RegisterMarshaler(new ThreadInfoMarshaller());
			MarshalingManager.RegisterMarshaler(new ProcessInfoMarshaller());

			MarshalingManager.RegisterMarshaler(new AgentSettingsMarshaller());
			MarshalingManager.RegisterMarshaler(new ConfigurationSettingsMarshaller());
		}
	}
}
