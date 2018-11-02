namespace Adenium.Layouting
{
    internal abstract class MenuControl : PropertyChangedBaseEx, IMenuControl
    {
        protected readonly CompositeMenuControlHandler Handler;
        private bool _isEnabled;
        private bool _isVisible;

        protected MenuControl(string id)
        {
            Id = id;
            Handler = new CompositeMenuControlHandler();
        }

        public string Id { get; private set; }

        public bool IsVisible
        {
            get { return _isVisible; }
            private set
            {
                _isVisible = value;
                NotifyOfPropertyChange(() => IsVisible);
            }
        }

        public bool IsEnabled
        {
            get { return _isEnabled; }
            private set
            {
                _isEnabled = value;
                NotifyOfPropertyChange(() => IsEnabled);
            }
        }

        public virtual void Invalidate()
        {
            SmartDispatcher.Main.Invoke(() =>
            {
                IsVisible = Handler.GetVisible();
                if (IsVisible)
                {
                    IsEnabled = Handler.GetEnabled();
                }
            });
        }

        internal void AttachHandler(IMenuControlHandler handler)
        {
            Handler.AttachHandler(handler);
        }

        internal virtual void Initialize(IViewModel ownerViewModel)
        {
            Handler.OnControlAttached(this);
            Handler.OnViewModelAttached(ownerViewModel);
            Invalidate();
        }

        internal virtual void Merge(MenuControl control)
        {
            Handler.AttachHandler(control.Handler);
        }
    }
}
