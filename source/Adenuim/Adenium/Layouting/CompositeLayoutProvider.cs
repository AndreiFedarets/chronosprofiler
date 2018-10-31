using System.Collections.Generic;

namespace Adenium.Layouting
{
    public static class CompositeLayoutProvider
    {
        private static readonly List<ILayoutProvider> Providers;
        private static readonly LayoutReader Reader;
        private static readonly LayoutBuilder Builder;
        private static IContainer _mainContainer;

        static CompositeLayoutProvider()
        {
            Providers = new List<ILayoutProvider>();
            Reader = new LayoutReader();
            Builder = new LayoutBuilder();
        }

        internal static IContainer MainContainer
        {
            get { return _mainContainer ?? (_mainContainer = new Container()); }
        }

        public static void Initialize(IContainer container)
        {
            if (_mainContainer == null)
            {
                _mainContainer = container;
            }
        }

        public static void Register(ILayoutProvider provider)
        {
            lock (Providers)
            {
                if (!Providers.Contains(provider))
                {
                    Providers.Add(provider);
                }
            }
        }

        internal static List<ViewModelLayout> GetLayouts(IContainerViewModel viewModel)
        {
            List<ViewModelLayout> layouts = new List<ViewModelLayout>();
            lock (Providers)
            {
                foreach (ILayoutProvider provider in Providers)
                {
                    string layoutString = provider.GetLayout(viewModel);
                    if (!string.IsNullOrWhiteSpace(layoutString))
                    {
                        IContainer container = MainContainer.CreateChildContainer();
                        provider.ConfigureContainer(container);
                        ViewModelLayout layout = Reader.Read(layoutString, viewModel, container);
                        layouts.Add(layout);
                    }
                }
            }
            return layouts;
        }

        internal static ViewModelLayout GetLayout(IContainerViewModel viewModel)
        {
            List<ViewModelLayout> layouts = GetLayouts(viewModel);
            ViewModelLayout layout = Builder.BuildLayout(layouts);
            return layout;
        }

        internal static ViewModelLayout GetLayout(IViewModel viewModel)
        {
            IContainerViewModel containerViewModel = viewModel as IContainerViewModel;
            if (containerViewModel != null)
            {
                return GetLayout(containerViewModel);
            }
            return new ViewModelLayout(new List<ViewModelReference>(), new MenuCollection());
        }
    }
}
