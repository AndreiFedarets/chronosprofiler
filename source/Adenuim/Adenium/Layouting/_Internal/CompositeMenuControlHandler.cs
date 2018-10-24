using System.Collections.Generic;
using System.Linq;

namespace Adenium.Layouting
{
    internal class CompositeMenuControlHandler : IMenuControlHandler
    {
        private readonly List<IMenuControlHandler> _handlers;

        public CompositeMenuControlHandler()
        {
            _handlers = new List<IMenuControlHandler>();
        }

        public void OnControlAttached(IMenuControl control)
        {
            _handlers.ForEach(x => x.OnControlAttached(control));
        }

        public virtual void OnAction()
        {
            _handlers.ForEach(x => x.OnAction());
        }

        public virtual bool GetEnabled()
        {
            return _handlers.Any(x => x.GetEnabled());
        }

        public virtual bool GetVisible()
        {
            return _handlers.Any(x => x.GetVisible());
        }

        public string GetText()
        {
            foreach (IMenuControlHandler handler in _handlers)
            {
                string text = handler.GetText();
                if (!string.IsNullOrWhiteSpace(text))
                {
                    return text;
                }
            }
            return string.Empty;
        }

        internal void AttachHandler(IMenuControlHandler handler)
        {
            _handlers.Add(handler);
        }

        public virtual void Dispose()
        {
            _handlers.ForEach(x => x.Dispose());
        }
    }
}
