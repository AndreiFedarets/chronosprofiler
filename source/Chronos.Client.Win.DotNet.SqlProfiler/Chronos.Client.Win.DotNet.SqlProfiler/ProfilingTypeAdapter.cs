﻿using System.Collections.Generic;
using Chronos.Client.Win.DotNet.SqlProfiler.Properties;
using Chronos.Client.Win.Menu;
using Chronos.Client.Win.ViewModels;
using Chronos.Messaging;

namespace Chronos.Client.Win.DotNet.SqlProfiler
{
    public sealed class ProfilingTypeAdapter : IProfilingTypeAdapter, IInitializable, IMessageBusHandler
    {
        private IProfilingApplication _application;

        public object CreateSettingsPresentation(ProfilingTypeSettings profilingTypeSettings)
        {
            return null;
        }

        void IInitializable.Initialize(IChronosApplication applicationObject)
        {
            _application = applicationObject as IProfilingApplication;
            if (_application != null)
            {
                _application.MessageBus.Subscribe(this);
            }
        }

        [MessageHandler(Win.Constants.Message.BuildProfilingViewMenu)]
        internal void BuildProfilingViewMenu(IContainerViewModel viewModel, List<IMenu> menus)
        {
            ResolutionDependencies dependencies = new ResolutionDependencies();
            dependencies.Register(_application);
            IMenu menu = MenuReader.ReadMenu(Resources.Menu, dependencies);
            menus.Add(menu);
        }
    }
}
