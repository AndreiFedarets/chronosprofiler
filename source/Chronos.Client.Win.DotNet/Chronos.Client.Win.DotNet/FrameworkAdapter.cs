using System.Collections.Generic;
using Chronos.Client.Win.DotNet.Properties;
using Chronos.Client.Win.Menu;
using Chronos.Client.Win.ViewModels.Profiling;
using Chronos.Messaging;

namespace Chronos.Client.Win.DotNet
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
        internal void BuildProfilingViewMenu(ProfilingViewModel viewModel, List<IMenu> menus)
        {
            IMenu menu = MenuReader.ReadMenu(Resources.Menu);
            menus.Add(menu);
        }
    }
}
