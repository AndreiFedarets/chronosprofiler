﻿using Chronos.Client.Win.DotNet.SqlProfiler.Properties;
using Chronos.Client.Win.Menu;
using Chronos.Client.Win.ViewModels;
using Chronos.Client.Win.ViewModels.Profiling;

namespace Chronos.Client.Win.DotNet.SqlProfiler
{
    public sealed class ProfilingTypeAdapter : IProfilingTypeAdapter, IMenuSource
    {
        public object CreateSettingsPresentation(ProfilingTypeSettings profilingTypeSettings)
        {
            return null;
        }

        public IMenu GetMenu(PageViewModel pageViewModel)
        {
            ProfilingViewModel profilingViewModel = pageViewModel as ProfilingViewModel;
            if (profilingViewModel != null)
            {
                IProfilingApplication application = profilingViewModel.Application;
                ResolutionDependencies dependencies = new ResolutionDependencies();
                dependencies.Register(application);
                dependencies.Register(profilingViewModel);
                return MenuReader.ReadMenu(Resources.Menu, dependencies);
            }
            return null;
        }
    }
}
