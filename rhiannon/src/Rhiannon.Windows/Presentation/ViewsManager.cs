using System;
using System.Collections.Generic;
using Microsoft.Practices.Composite.Regions;
using Rhiannon.Extensions;
using Rhiannon.Threading;
using Rhiannon.Unity;

namespace Rhiannon.Windows.Presentation
{
	public class ViewsManager : IViewsManager
	{
		private readonly IContainer _container;
		private readonly IRegionManager _regionManager;
		private readonly IThreadFactory _threadFactory;
		private readonly IDictionary<Type, IViewActivatorBase> _activatorsTypes;
		private readonly IDictionary<string, IViewActivatorBase> _activatorsNames;

		public ViewsManager(IContainer container, IRegionManager regionManager, IThreadFactory threadFactory)
			: this(container, regionManager, threadFactory, new Dictionary<Type, IViewActivatorBase>(), new Dictionary<string, IViewActivatorBase>())
		{
		}

		private ViewsManager(IContainer container, IRegionManager regionManager, IThreadFactory threadFactory,
			IDictionary<Type, IViewActivatorBase> activatorsTypes, IDictionary<string, IViewActivatorBase> activatorsNames)
		{
			_container = container;
			_regionManager = regionManager;
			_threadFactory = threadFactory;
			_activatorsTypes = activatorsTypes;
			_activatorsNames = activatorsNames;
		}

		public IWindow ResolveAndWrap<T>(params object[] args) where T : IViewBase
		{
			Func<IWindow> func = () =>
			{
				IViewActivatorBase activator = _activatorsTypes[typeof(T)];
				return activator.ActivateAndWrap(_container, args);
			};
			if (ThreadExtensions.IsStaThread)
			{
				return func();
			}
			return _threadFactory.Invoke(func);
		}

		public IWindow ResolveAndWrap(string viewName, params object[] args)
		{
			Func<IWindow> func = () =>
			{
				IViewActivatorBase activator = _activatorsNames[viewName];
				return activator.ActivateAndWrap(_container, args);
			};
			if (ThreadExtensions.IsStaThread)
			{
				return func();
			}
			return _threadFactory.Invoke(func);
		}

		public T Resolve<T>(params object[] args) where T : IViewBase
		{
			Func<IViewBase> func = () =>
			{
				IViewActivatorBase activator = _activatorsTypes[typeof(T)];
				return activator.Activate(_container, args);
			};
			if (ThreadExtensions.IsStaThread)
			{
				return (T)func();
			}
			return (T)_threadFactory.Invoke(func);
		}

		public IViewBase Resolve(string viewName, params object[] args)
		{
			Func<IViewBase> func = () =>
			{
				IViewActivatorBase activator = _activatorsNames[viewName];
				return activator.Activate(_container, args);
			};
			if (ThreadExtensions.IsStaThread)
			{
				return func();
			}
			return _threadFactory.Invoke(func);
		}

		public void Register<T>() where T : IViewActivatorBase
		{
			T activator = _container.Resolve<T>();
			_regionManager.RegisterViewWithRegion(activator.ViewName, () => Resolve(activator.ViewName));
			_activatorsTypes.Add(activator.ViewType, activator);
			_activatorsNames.Add(activator.ViewName, activator);
		}

		public IViewsManager CreateChildViewsManager(IContainer container)
		{
			IRegionManager regionManager = container.Resolve<IRegionManager>();
			IThreadFactory threadFactory = container.Resolve<IThreadFactory>();
			ViewsManager viewsManager = new ViewsManager(container, regionManager, threadFactory, _activatorsTypes, _activatorsNames);
			return viewsManager;
		}
	}
}
