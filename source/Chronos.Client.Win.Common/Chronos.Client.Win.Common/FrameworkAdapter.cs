using System.Collections.Generic;
using Adenium;
using Adenium.Menu;
using Chronos.Client.Win.Common.Properties;
using Chronos.Client.Win.Menu;
using Chronos.Client.Win.ViewModels;
using Chronos.Client.Win.ViewModels.Profiling;
using Chronos.Messaging;

namespace Chronos.Client.Win.Common
{
    public class FrameworkAdapter : IFrameworkAdapter, IInitializable, IMessageBusHandler
    {
        private IProfilingApplication _application;

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
            MenuReader reader = new MenuReader();
            IMenu menu = reader.ReadMenu(Resources.Menu, new Container());
            menus.Add(menu);
        }
    }
}
