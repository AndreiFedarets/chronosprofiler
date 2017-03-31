namespace Chronos.Client.Win.Menu
{
    public class MenuItem : ControlCollection, IMenuItem
    {
        private string _text;

        public MenuItem()
        {
            _text = string.Empty;
        }

        public virtual string Text
        {
            get { return _text; }
            protected set
            {
                _text = value;
                NotifyOfPropertyChange(() => Text);
            }
        }

        public virtual void OnAction()
        {
            
        }
    }
}
