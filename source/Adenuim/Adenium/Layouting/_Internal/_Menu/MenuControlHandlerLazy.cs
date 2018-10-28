using System;

namespace Adenium.Layouting
{
    internal sealed class MenuControlHandlerLazy : MenuControlHandlerBase
    {
        private readonly Lazy<IMenuControlHandler> _handler;
        private readonly Type _controlHandlerType;
        private readonly IActivator _activator;

        public MenuControlHandlerLazy(Type controlHandlerType, IActivator activator)
        {
            _controlHandlerType = controlHandlerType;
            _activator = activator;
            _handler = new Lazy<IMenuControlHandler>(CreateControlHandler);
        }

        public override void OnControlAttached(IMenuControl control)
        {
            _handler.Value.OnControlAttached(control);
        }

        public override bool GetVisible()
        {
            return _handler.Value.GetVisible();
        }

        public override bool GetEnabled()
        {
            return _handler.Value.GetEnabled();
        }

        public override void OnAction()
        {
            _handler.Value.OnAction();
        }

        public override void Dispose()
        {
            base.Dispose();
            if (_handler.IsValueCreated)
            {
                _handler.Value.Dispose();
            }
        }

        private IMenuControlHandler CreateControlHandler()
        {
            return (IMenuControlHandler) _activator.Resolve(_controlHandlerType);
        }
    }
}
