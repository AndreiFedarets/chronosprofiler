using System;
using Chronos.Configuration;
using Chronos.Core;
using Chronos.CustomMarshalers;
using Chronos.Extensibility;
using Rhiannon.Extensions;
using Rhiannon.Serialization;
using Rhiannon.Unity;

namespace Chronos.Host.Internal
{
	internal class HostApplication : MarshalByRefObject, IHostApplication
	{
		public static readonly Guid Token;
		private readonly IContainer _container;
		private readonly DisposableScope _disposable;
		private readonly Action _close;

		static HostApplication()
		{
			Token = new Guid("{1B6E1937-B1A8-4E54-B2E0-D4DC1CD7642A}");
		}

		public HostApplication(Action close)
        {
            CustomMarshalersInitializer.Initialize();
			_close = close;
			_container = new Container();
			_disposable = new DisposableScope();
			ConfigureContainer();
			SessionActivatorProvider = _container.Resolve<ISessionActivatorProvider>();
			Configurations = _container.Resolve<IConfigurationCollection>();
			Sessions = _container.Resolve<ISessionCollection>();
			_disposable.Add(Configurations);
			_disposable.Add(Sessions);
			_disposable.Add(_container.Resolve<IHostRequestServer>());
			LoadExtensions();
		}

		public override object InitializeLifetimeService()
		{
			return null;
		}

		private void LoadExtensions()
		{
			ExtensionsLoader extensionsLoader = new ExtensionsLoader(_container);
			ICommonConfiguration commonConfiguration = _container.Resolve<ICommonConfiguration>();
			extensionsLoader.LoadAndInitialize(commonConfiguration.Host.Extensions);
		}

		public IConfigurationCollection Configurations { get; private set; }

		public ISessionCollection Sessions { get; private set; }

		public ISessionActivatorProvider SessionActivatorProvider { get; private set; }

		public string MachineName
		{
			get { return Environment.MachineName; }
		}

		private void ConfigureContainer()
		{
			_container.RegisterInstance<IHostApplication>(this);
			ICommonConfiguration configuration = ConfigurationProvider.Load();
			_container.RegisterInstance(configuration);
			_container.RegisterTypeIfNotRegistered<IConfigurationCollection, ConfigurationCollection>(true);
			_container.RegisterTypeIfNotRegistered<ISerializerFactory, SerializerFactory>(true);
			_container.RegisterTypeIfNotRegistered<ISessionCollection, SessionCollection>(true);
			_container.RegisterTypeIfNotRegistered<ISessionActivatorProvider, SessionActivatorProvider>(true);
			_container.RegisterTypeIfNotRegistered<IHostRequestServer, HostRequestServer>(true);
		}

		public void Quit()
		{
			_close.SafeInvoke();
		}

		public string Ping(string message)
		{
			return message;
		}

		public void Dispose()
		{
			_disposable.Dispose();
		}
	}
}
