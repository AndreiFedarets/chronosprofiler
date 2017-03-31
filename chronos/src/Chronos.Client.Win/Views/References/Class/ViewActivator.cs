using Chronos.Core;
using Rhiannon.Unity;
using Rhiannon.Windows.Presentation;

namespace Chronos.Client.Win.Views.References.Class
{
    public class ViewActivator : ViewActivatorBase<IView, View, IViewModel, ViewModel>
    {
        public ViewActivator()
            : base(ViewNames.References.Class)
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
            ClassInfo classInfo = (ClassInfo)args[0];
            IContainer childContainer = container.CreateChildContainer();
            childContainer.RegisterInstance(classInfo);
            IViewBase view = base.Activate(childContainer, args);
            return view;
        }
    }
}
