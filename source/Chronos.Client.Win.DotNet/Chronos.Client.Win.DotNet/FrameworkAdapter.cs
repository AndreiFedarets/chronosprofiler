using Adenium;
using Adenium.Layouting;

namespace Chronos.Client.Win.DotNet
{
    public class FrameworkAdapter : IFrameworkAdapter, IInitializable, ILayoutProvider
    {
        private readonly IContainer _container;

        public FrameworkAdapter()
        {
            _container = new Container();
        }

        void IInitializable.Initialize(IChronosApplication applicationObject)
        {
            IProfilingApplication application = applicationObject as IProfilingApplication;
            if (application != null)
            {
                _container.RegisterInstance(application);
            }
        }

        public IActivator Activator
        {
            get { return _container; }
        }

        public string GetLayout(IViewModel viewModel)
        {
            
        }
    }
}
