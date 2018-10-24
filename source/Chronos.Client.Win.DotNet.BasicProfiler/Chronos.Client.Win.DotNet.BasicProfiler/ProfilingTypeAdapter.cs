using System.Collections.Generic;
using Adenium;
using Adenium.Menu;
using Chronos.Client.Win.DotNet.BasicProfiler.Properties;
using Chronos.Client.Win.Menu;
using Chronos.Client.Win.ViewModels;
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
        internal void BuildProfilingViewMenu(IContainerViewModel viewModel, List<IMenu> menus)
        {
            Container container = new Container();
            container.RegisterInstance(_application);
            MenuReader reader = new MenuReader();
            IMenu menu = reader.ReadMenu(Resources.Menu, container);
            menus.Add(menu);
        }
    }
}
