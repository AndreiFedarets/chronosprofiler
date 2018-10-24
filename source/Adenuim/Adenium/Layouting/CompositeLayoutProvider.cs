using System.Collections.Generic;

namespace Adenium.Layouting
{
    public static class CompositeLayoutProvider
    {
        private static readonly List<ILayoutProvider> Providers;
        private static readonly LayoutReader Reader;

        static CompositeLayoutProvider()
        {
            Providers = new List<ILayoutProvider>();
            Reader = new LayoutReader();
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

        internal static List<ViewModelLayout> GetLayouts(IViewModel viewModel)
        {
            List<ViewModelLayout> layouts = new List<ViewModelLayout>();
            lock (Providers)
            {
                foreach (ILayoutProvider provider in Providers)
                {
                    string layoutString = provider.GetLayout(viewModel);
                    if (!string.IsNullOrWhiteSpace(layoutString))
                    {
                        ViewModelLayout layout = Reader.Read(layoutString, provider);
                        layouts.Add(layout);
                    }
                }
            }
            return layouts;
        }

        internal static ViewModelLayout GetLayout(IViewModel viewModel)
        {
            Dictionary<ILayoutProvider, string> rawLayouts GetLayouts(viewModel)
        }
    }
}
