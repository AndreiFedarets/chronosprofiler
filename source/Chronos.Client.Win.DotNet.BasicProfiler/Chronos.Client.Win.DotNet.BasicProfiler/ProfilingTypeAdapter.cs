﻿using System.Collections.Generic;
using Chronos.Client.Win.DotNet.BasicProfiler.Properties;
using Chronos.Client.Win.Menu;
using Chronos.Client.Win.ViewModels.Profiling;
using Chronos.Messaging;

namespace Chronos.Client.Win.DotNet.BasicProfiler
{
    public class ProfilingTypeAdapter : IProfilingTypeAdapter, IMessageBusHandler, IInitializable
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
        internal void BuildProfilingViewMenu(ProfilingViewModel viewModel, List<IMenu> menus)
        {
            ResolutionDependencies dependencies = new ResolutionDependencies();
            dependencies.Register(viewModel);
            IMenu menu = MenuReader.ReadMenu(Resources.Menu, dependencies);
            menus.Add(menu);
        }
    }
}
