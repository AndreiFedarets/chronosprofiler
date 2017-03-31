using System.IO;
using Rhiannon.Serialization;
using Rhiannon.Unity;

namespace Rhiannon.CompositeService
{
	public class CompositeServiceXmlBuilder : ICompositeServiceBuilder
	{
		private readonly ServiceConfiguration _serviceConfiguration;
		private readonly IContainer _container;

		public CompositeServiceXmlBuilder(IContainer container, ServiceConfiguration serviceConfiguration)
		{
			_serviceConfiguration = serviceConfiguration;
			_container = container;
		}

		public CompositeServiceXmlBuilder(Stream stream, ISerializerFactory serializerFactory, IContainer container)
		{
			_container = container;
			ISerializer serializer = serializerFactory.CreateXml<ServiceConfiguration>();
			_serviceConfiguration = serializer.Deserialize<ServiceConfiguration>(stream);
		}

		public CompositeServiceXmlBuilder(string fileFullName, ISerializerFactory serializerFactory, IContainer container)
			: this(new FileStream(fileFullName, FileMode.Open), serializerFactory, container)
		{
		}

		public ICompositeService Build()
		{
			ICompositeService service = (ICompositeService) _container.Resolve(_serviceConfiguration.ServiceType);
			foreach (ServiceOperationConfiguration operationConfiguration in _serviceConfiguration.Operations)
			{
				IServiceOperation operation = (IServiceOperation) _container.Resolve(operationConfiguration.OperationType);
				service.RegisterOperation(operation, operationConfiguration.OperationOrder);
			}
			return service;
		}
	}
}
