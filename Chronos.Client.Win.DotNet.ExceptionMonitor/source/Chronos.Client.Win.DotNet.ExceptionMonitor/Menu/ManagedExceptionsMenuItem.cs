using Chronos.Client.Win.DotNet.ExceptionMonitor.Properties;
using Chronos.Client.Win.Menu.Specialized;
using Chronos.Client.Win.Models;
using Chronos.Client.Win.Models.DotNet.ExceptionMonitor;
using Chronos.Client.Win.ViewModels.Profiling;
using Chronos.DotNet.ExceptionMonitor;

namespace Chronos.Client.Win.Menu.DotNet.ExceptionMonitor
{
    internal sealed class ManagedExceptionsMenuItem : UnitsMenuItemBase
    {
        public ManagedExceptionsMenuItem(ProfilingViewModel profilingViewModel)
            : base(profilingViewModel)
        {
        }

        public override string Text
        {
            get { return Resources.ManagedExceptionsMenuItem_Text; }
            protected set { }
        }

        protected override IUnitsModel GetModel()
        {
            IManagedExceptionCollection collection = ProfilingViewModel.Application.ServiceContainer.Resolve<IManagedExceptionCollection>();
            ManagedExceptionsModel model = new ManagedExceptionsModel(collection);
            return model;
        }
    }
}
