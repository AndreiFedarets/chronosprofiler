namespace Adenium.Layouting
{
    internal sealed class MenuItem : MenuControlCollection
    {
        private string _text;

        public MenuItem(string id)
            : base(id)
        {
            _text = string.Empty;
        }

        public string Text
        {
            get { return _text; }
            private set
            {
                _text = value;
                NotifyOfPropertyChange(() => Text);
            }
        }

        public void OnAction()
        {
            Handler.OnAction();
        }

        public override void Invalidate()
        {
            base.Invalidate();
            if (IsVisible)
            {
                Text = Handler.GetText();
            }
        }
    }
}
