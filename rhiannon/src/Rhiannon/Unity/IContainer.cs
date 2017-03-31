using System;

namespace Rhiannon.Unity
{
	public interface IContainer
	{
		object Resolve(Type type);

		bool IsRegistered<T>();

		T Resolve<T>();

		T Resolve<T, T1>(T1 arg1);

		T Resolve<T, T1, T2>(T1 arg1, T2 arg2);

		T Resolve<T, T1, T2, T3>(T1 arg1, T2 arg2, T3 arg3);

		T Resolve<T, T1, T2, T3, T4>(T1 arg1, T2 arg2, T3 arg3, T4 arg4);

		void RegisterType<TInterface, TImplementaion>(bool singleton) where TImplementaion : TInterface;

		void RegisterType<TInterface, TImplementaion>() where TImplementaion : TInterface;

		void RegisterInstance<T>(T instance);

		void RegisterTypeIfNotRegistered<TInterface, TImplementaion>(bool singleton) where TImplementaion : TInterface;

		void RegisterTypeIfNotRegistered<TInterface, TImplementaion>() where TImplementaion : TInterface;

		void RegisterInstanceIfNotRegistered<T>(T instance);

		IContainer CreateChildContainer();
	}
}
