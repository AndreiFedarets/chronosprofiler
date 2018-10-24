namespace Adenium.Layouting
{
    public class MenuControlHandlerBase : IMenuControlHandler
    {
        private IMenuControl _control;

        protected IMenuControl Control
        {
            get { return _control; }
        }

        public void OnControlAttached(IMenuControl control)
        {
            _control = control;
        }

        public virtual void OnAction()
        {

        }

        public virtual bool GetEnabled()
        {
            return true;
        }

        public virtual bool GetVisible()
        {
            return true;
        }

        public string GetText()
        {
            return string.Empty;
        }

        public virtual void Dispose()
        {

        }
    }
}
