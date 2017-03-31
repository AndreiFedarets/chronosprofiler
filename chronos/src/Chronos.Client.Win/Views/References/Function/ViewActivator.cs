using Chronos.Core;
using Rhiannon.Unity;
using Rhiannon.Windows.Presentation;

namespace Chronos.Client.Win.Views.References.Function
{
    public class ViewActivator : ViewActivatorBase<IView, View, IViewModel, ViewModel>
    {
        public ViewActivator()
            : base(ViewNames.References.Function)
        {
        }

        public override IWindow ActivateAndWrap(IContainer container, params object[] args)
        {
            IWindow window = ActivateAndWrapInternal(container, false, args);
            window.Height = 500;
            window.Width = 600;
            return window;
        }

        public override IViewBase Activate(IContainer container, params object[] args)
        {
            FunctionInfo functionInfo = (FunctionInfo)args[0];
            IContainer childContainer = container.CreateChildContainer();
            childContainer.RegisterInstance(functionInfo);
            IViewBase view = base.Activate(childContainer, args);
            return view;
        }
    }
}
