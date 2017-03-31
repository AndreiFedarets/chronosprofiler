using System;
using Microsoft.Practices.Unity;

namespace Rhiannon.Unity
{
	public class Container : IContainer
	{
		private readonly IUnityContainer _container;

		public Container()
			: this(new UnityContainer())
		{
		}

		public Container(IUnityContainer container)
		{
			_container = container;
			_container.RegisterInstance<IContainer>(this);
		}

		public bool IsRegistered<T>()
		{
			return _container.IsRegistered<T>();
		}

		public object Resolve(Type type)
		{
			return _container.Resolve(type);
		}

		public T Resolve<T>()
		{
			return _container.Resolve<T>();
		}

		public T Resolve<T, T1>(T1 arg1)
		{
			IUnityContainer container = _container.CreateChildContainer();
			container.RegisterInstance<T1>(arg1);
			return container.Resolve<T>();
		}

		public T Resolve<T, T1, T2>(T1 arg1, T2 arg2)
		{
			IUnityContainer container = _container.CreateChildContainer();
			container.RegisterInstance<T1>(arg1);
			container.RegisterInstance<T2>(arg2);
			return container.Resolve<T>();
		}

		public T Resolve<T, T1, T2, T3>(T1 arg1, T2 arg2, T3 arg3)
		{
			IUnityContainer container = _container.CreateChildContainer();
			container.RegisterInstance<T1>(arg1);
			container.RegisterInstance<T2>(arg2);
			container.RegisterInstance<T3>(arg3);
			return container.Resolve<T>();
		}

		public T Resolve<T, T1, T2, T3, T4>(T1 arg1, T2 arg2, T3 arg3, T4 arg4)
		{
			IUnityContainer container = _container.CreateChildContainer();
			container.RegisterInstance<T1>(arg1);
			container.RegisterInstance<T2>(arg2);
			container.RegisterInstance<T3>(arg3);
			container.RegisterInstance<T4>(arg4);
			return container.Resolve<T>();
		}

		public void RegisterType<TInterface, TImplementaion>(bool singleton) where TImplementaion : TInterface
		{
			if (singleton)
			{
				_container.RegisterType<TInterface, TImplementaion>(new ContainerControlledLifetimeManager());
			}
			else
			{
				_container.RegisterType<TInterface, TImplementaion>();
			}
		}

		public void RegisterInstance<T>(T instance)
		{
			_container.RegisterInstance(instance);
		}


		public void RegisterType<TInterface, TImplementaion>() where TImplementaion : TInterface
		{
			RegisterType<TInterface, TImplementaion>(false);
		}


		public void RegisterTypeIfNotRegistered<TInterface, TImplementaion>(bool singleton) where TImplementaion : TInterface
		{
			if (!IsRegistered<TInterface>())
			{
				RegisterType<TInterface, TImplementaion>(singleton);
			}
		}

		public void RegisterTypeIfNotRegistered<TInterface, TImplementaion>() where TImplementaion : TInterface
		{
			if (!IsRegistered<TInterface>())
			{
				RegisterType<TInterface, TImplementaion>();
			}
		}

		public void RegisterInstanceIfNotRegistered<T>(T instance)
		{
			if (!IsRegistered<T>())
			{
				RegisterInstance<T>(instance);
			}
		}

		public IContainer CreateChildContainer()
		{
			return new Container(_container.CreateChildContainer());
		}
	}
}
