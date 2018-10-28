using Chronos.Messaging;

namespace Chronos.Client.Win.DotNet.ExceptionMonitor
{
    public class ProfilingTypeAdapter : IProfilingTypeAdapter, IInitializable, IMessageBusHandler
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

        //[MessageHandler(Win.Constants.Message.BuildProfilingViewMenu)]
        //internal void BuildProfilingViewMenu(IContainerViewModel viewModel, List<IMenu> menus)
        //{
        //    Container container = new Container();
        //    container.RegisterInstance(_application);
        //    MenuReader reader = new MenuReader();
        //    IMenu menu = reader.ReadMenu(Resources.Menu, container);
        //    menus.Add(menu);
        //}
    }
}
