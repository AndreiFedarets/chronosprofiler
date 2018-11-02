namespace Adenium.Layouting
{
    public class MenuControlHandlerBase : IMenuControlHandler
    {
        private IMenuControl _control;
        private IViewModel _ownerViewModel;

        protected IMenuControl Control
        {
            get { return _control; }
        }

        protected IViewModel OwnerViewModel
        {
            get { return _ownerViewModel; }
        }

        public virtual void OnControlAttached(IMenuControl control)
        {
            _control = control;
        }

        public virtual void OnViewModelAttached(IViewModel ownerViewModel)
        {
            _ownerViewModel = ownerViewModel;
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

        public virtual string GetText()
        {
            return string.Empty;
        }

        public virtual void Dispose()
        {

        }
    }
}
