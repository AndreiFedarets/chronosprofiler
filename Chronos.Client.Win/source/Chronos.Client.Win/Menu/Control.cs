namespace Chronos.Client.Win.Menu
{
    public abstract class Control : PropertyChangedBaseEx, IControl
    {
        private bool? _isEnabled;
        private bool? _isVisible;

        public virtual string Id
        {
            get { return GetType().FullName; }
        }

        public virtual bool? IsEnabled
        {
            get { return _isEnabled; }
            protected set
            {
                _isEnabled = value;
                NotifyOfPropertyChange(() => IsEnabled);
            }
        }

        public virtual bool? IsVisible
        {
            get { return _isVisible; }
            protected set
            {
                _isVisible = value;
                NotifyOfPropertyChange(() => IsVisible);
            }
        }

        public virtual void Invalidate()
        {
            NotifyOfPropertyChange(() => IsEnabled);
            NotifyOfPropertyChange(() => IsVisible);
        }
    }
}
