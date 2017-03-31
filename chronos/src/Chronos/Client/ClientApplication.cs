using System;
using System.Collections.Generic;
using System.Linq;
using Chronos.Communication.Remoting;
using Chronos.Configuration;
using Chronos.Extensibility;
using Chronos.Host;
using Rhiannon.Unity;

namespace Chronos.Client
{
	public class ClientApplication : IClientApplication
	{
		private static IChannelFactory _channelFactory;
		private static ClientApplication _current;
		private static readonly object Lock;
		private IContainer _container;

		static ClientApplication()
		{
			Lock = new object();
		}

		private ClientApplication()
		{
			Token = Guid.NewGuid();
		}

		private void Initialize(IContainer container)
		{
			_container = ConfigureContainer(container);
			Host = _container.Resolve<IHostApplication>();
			ProfilingTargets = _container.Resolve<IProfilingTargetCollection>();
			ProfilingStrategies = _container.Resolve<IProfilingStrategyCollection>();
			LoadExtensions();
		}

		public Guid Token { get; private set; }

		public IProfilingTargetCollection ProfilingTargets { get; private set; }

		public IProfilingStrategyCollection ProfilingStrategies { get; private set; }

		public IHostApplication Host { get; private set; }

		public static IClientApplication Current
		{
			get
			{
				lock (Lock)
				{
					return _current;
				}
			}
		}

		private IContainer ConfigureContainer(IContainer container)
		{
			container.RegisterInstance<IClientApplication>(this);
			ICommonConfiguration configuration = ConfigurationProvider.Load();
			container.RegisterInstance(configuration);

			IContainer localContainer = container.CreateChildContainer();
			localContainer.RegisterTypeIfNotRegistered<IProfilingTargetCollection, ProfilingTargetCollection>(true);
			localContainer.RegisterTypeIfNotRegistered<IProfilingStrategyCollection, ProfilingStrategyCollection>(true);
			localContainer.RegisterTypeIfNotRegistered<IRemotingExecutor, RemotingExecutor>(true);
			if (!HostProvider.CheckConnection())
			{
				HostProvider.Run(configuration);
			}
			IHostApplication hostApplication = HostProvider.Connect(new RemotingExecutor());
			localContainer.RegisterInstance(hostApplication);
			return localContainer;
		}

		public static IClientApplication Run()
		{
			return Run(new Container());
		}

		public static IClientApplication Run(IContainer container)
		{
			lock (Lock)
			{
				if (_current == null)
				{
					_channelFactory = new IpcChannelFactory();
					RemotingFactory.Current.Initialize(_channelFactory);
					_current = new ClientApplication();
					_current.Initialize(container);

				}
				return _current;
			}
		}

		private void LoadExtensions()
		{
			ExtensionsLoader extensionsLoader = new ExtensionsLoader(_container);
			ICommonConfiguration commonConfiguration = _container.Resolve<ICommonConfiguration>();
			extensionsLoader.LoadAndInitialize(commonConfiguration.WinClient.Extensions);
		}

		public static void Shutdown()
		{
			lock (Lock)
			{
				if (_current != null)
				{
					IList<ISession> sessions = _current.Host.Sessions.ToList();
					foreach (ISession session in sessions)
					{
						session.Close();
					}
					_current.Host.Quit();
				}
				_channelFactory.Dispose();
			}
		}

		public void Dispose()
		{
			Shutdown();
		}
	}
}
