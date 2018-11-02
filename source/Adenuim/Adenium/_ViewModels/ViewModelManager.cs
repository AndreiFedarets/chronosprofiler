using System;
using System.Collections.Generic;
using System.Linq;
using Adenium.Layouting;
using Caliburn.Micro;

namespace Adenium
{
    public class ViewModelManager : IViewModelManager
    {
        private readonly ILayoutReader _layoutReader;
        private readonly List<ILayoutProvider> _layoutProviders;
        private readonly IWindowManager _windowManager;
        private readonly IContainer _mainContainer;

        public ViewModelManager(IWindowManager windowManager, IContainer container)
        {
            _windowManager = windowManager;
            _mainContainer = container;
            _layoutProviders = new List<ILayoutProvider>();
            _layoutReader = new SmartLayoutReader();
            Instance = this;
        }

        internal static IViewModelManager Instance { get; private set; }

        public IViewModel CreateViewModel(Type viewModelType)
        {
            return CreateViewModel<object, object, object>(viewModelType, null, null, null, null);
        }

        public IViewModel CreateViewModel(Type viewModelType, IContainerViewModel parentViewModel)
        {
            return CreateViewModel<object, object, object>(viewModelType, parentViewModel, null, null, null);
        }

        public IViewModel CreateViewModel<T1>(Type viewModelType, IContainerViewModel parentViewModel, T1 dependency1)
        {
            return CreateViewModel<T1, object, object>(viewModelType, parentViewModel, dependency1, null, null);
        }

        public IViewModel CreateViewModel<T1, T2>(Type viewModelType, IContainerViewModel parentViewModel, T1 dependency1, T2 dependency2)
        {
            return CreateViewModel<T1, T2, object>(viewModelType, parentViewModel, dependency1, dependency2, null);
        }

        public IViewModel CreateViewModel<T1, T2, T3>(Type viewModelType, IContainerViewModel parentViewModel, T1 dependency1, T2 dependency2, T3 dependency3)
        {
            IContainer scopeContainer = CreateScopeContainer(parentViewModel, dependency1, dependency2, dependency3);
            IViewModel viewModel = (IViewModel)scopeContainer.Resolve(viewModelType);
            IHaveScope haveScope = viewModel as IHaveScope;
            if (haveScope != null)
            {
                haveScope.AssignScopeContainer(scopeContainer);
            }
            return viewModel;
        }

        public void ShowWindow(IViewModel viewModel)
        {
            BuildViewModelLayout(viewModel);
            _windowManager.ShowWindow(viewModel);
        }

        public bool? ShowDialog(IViewModel viewModel)
        {
            BuildViewModelLayout(viewModel);
            return _windowManager.ShowDialog(viewModel);
        }

        public void ShowPopup(IViewModel viewModel)
        {
            BuildViewModelLayout(viewModel);
            _windowManager.ShowPopup(viewModel);
        }

        public void BuildViewModelLayout(IViewModel viewModel)
        {
            IHaveLayout haveLayout = viewModel as IHaveLayout;
            if (haveLayout == null || haveLayout.Layout != null)
            {
                return;
            }
            IContainer scopeContainer = GetScopeContainer(viewModel);
            ViewModelLayout layout = PrepareViewModelLayout(viewModel, scopeContainer);
            try
            {
                //First step - build menus
                MenuCollection menus = viewModel.Menus as MenuCollection;
                if (menus != null)
                {
                    foreach (Menu menu in layout.Menus.Cast<Menu>())
                    {
                        menus.Add(menu);
                    }
                    menus.Initialize(viewModel);
                }
                //Second step - process attached view models
                //If our view model is ContainerViewModel - take it
                IContainerViewModel containerViewModel = viewModel as IContainerViewModel;
                //If our view model is ViewModel - take it's parrent as target
                containerViewModel = containerViewModel ?? viewModel.Parent;
                if (containerViewModel != null)
                {
                    foreach (ViewModelAttachment attachment in layout.Attachments)
                    {
                        if (attachment.Activation == ViewModelActivation.OnStartup)
                        {
                            IViewModel childViewModel = attachment.CreateViewModel();
                            containerViewModel.ActivateItem(childViewModel);
                            if (!Equals(containerViewModel, viewModel) && childViewModel is IAttachmentViewModel)
                            {
                                ((IAttachmentViewModel)childViewModel).OnAttached(viewModel);
                            }
                        }
                    }
                }
            }
            finally
            {
                haveLayout.AssignLayout(layout);
            }
        }

        public void ResetViewModelLayout(IViewModel viewModel)
        {
            IHaveLayout haveLayout = viewModel as IHaveLayout;
            if (haveLayout != null && haveLayout.Layout != null)
            {
                haveLayout.AssignLayout(null);
            }
            IContainerViewModel containerViewModel = viewModel as IContainerViewModel;
            if (containerViewModel != null)
            {
                containerViewModel.RemoveItems();
            }
        }

        private IContainer GetScopeContainer(IViewModel viewModel)
        {
            IContainer scopeContainer = null;
            IHaveScope haveScope = viewModel as IHaveScope;
            if (haveScope != null)
            {
                scopeContainer = haveScope.ScopeContainer;
            }
            if (scopeContainer == null)
            {
                scopeContainer = _mainContainer;
            }
            return scopeContainer;
        }

        private IContainer CreateScopeContainer<T1, T2, T3>(IContainerViewModel parentViewModel, T1 dependency1, T2 dependency2, T3 dependency3)
        {
            IContainer parentContainer = GetScopeContainer(parentViewModel);
            IContainer scopeContainer = parentContainer.CreateChildContainer();
            if (dependency1 != null)
            {
                scopeContainer.RegisterInstance(dependency1);
            }
            if (dependency2 != null)
            {
                scopeContainer.RegisterInstance(dependency2);
            }
            if (dependency3 != null)
            {
                scopeContainer.RegisterInstance(dependency3);
            }
            return scopeContainer;
        }

        public void RegisterLayoutProvider(ILayoutProvider layoutProvider)
        {
            lock (_layoutProviders)
            {
                if (!_layoutProviders.Contains(layoutProvider))
                {
                    _layoutProviders.Add(layoutProvider);
                }
            }
        }

        public IViewModel ActivateItem(IContainerViewModel containerViewModel, string childViewModelId)
        {
            return ActivateItem<object, object, object>(containerViewModel, childViewModelId, null, null, null);
        }

        public IViewModel ActivateItem<T1>(IContainerViewModel containerViewModel, string childViewModelId, T1 dependency1)
        {
            return ActivateItem<T1, object, object>(containerViewModel, childViewModelId, dependency1, null, null);
        }

        public IViewModel ActivateItem<T1, T2>(IContainerViewModel containerViewModel, string childViewModelId, T1 dependency1, T2 dependency2)
        {
            return ActivateItem<T1, T2, object>(containerViewModel, childViewModelId, dependency1, dependency2, null);
        }

        public IViewModel ActivateItem<T1, T2, T3>(IContainerViewModel containerViewModel, string childViewModelId, T1 dependency1, T2 dependency2, T3 dependency3)
        {
            IHaveLayout haveLayout = containerViewModel as IHaveLayout;
            if (haveLayout == null || haveLayout.Layout == null)
            {
                return null;
            }
            ViewModelLayout layout = haveLayout.Layout;
            ViewModelAttachment attachment = layout.FindAttachment(childViewModelId);
            if (attachment == null)
            {
                return null;
            }
            IViewModel viewModel = attachment.CreateViewModel(dependency1, dependency2, dependency3);
            containerViewModel.ActivateItem(viewModel);
            return viewModel;
        }

        private ViewModelLayout PrepareViewModelLayout(IViewModel viewModel, IContainer scopeContainer)
        {
            List<ViewModelLayout> layouts = CollectViewModelLayouts(viewModel, scopeContainer);
            ViewModelLayout layout = CombineViewModelLayouts(layouts);
            return layout;
        }

        private ViewModelLayout CombineViewModelLayouts(List<ViewModelLayout> layouts)
        {
            List<ViewModelAttachment> viewModels = layouts.SelectMany(x => x.Attachments).OrderBy(x => x.Order).ToList();
            MenuCollection menus = new MenuCollection();
            foreach (ViewModelLayout layout in layouts)
            {
                IEnumerable<Menu> localMenus = layout.Menus.Cast<Menu>();
                foreach (Menu localMenu in localMenus)
                {
                    menus.Add(localMenu);
                }
            }
            return new ViewModelLayout(viewModels, menus);
        }

        private List<ViewModelLayout> CollectViewModelLayouts(IViewModel viewModel, IContainer scopeContainer)
        {
            List<ViewModelLayout> layouts = new List<ViewModelLayout>();
            lock (_layoutProviders)
            {
                foreach (ILayoutProvider provider in _layoutProviders)
                {
                    string layoutString = provider.GetLayout(viewModel);
                    if (!string.IsNullOrWhiteSpace(layoutString))
                    {
                        provider.ConfigureContainer(viewModel, scopeContainer);
                        ViewModelLayout layout = _layoutReader.Read(layoutString, viewModel, scopeContainer);
                        layouts.Add(layout);
                    }
                }
            }
            return layouts;
        }
    }
}
