using System.Linq;

namespace Chronos.Client.Win.Menu
{
    internal sealed class CompositeMenuItem : CompositeControlCollection<IMenuItem>, IMenuItem
    {
        public CompositeMenuItem(IMenuItem control)
            : base(control)
        {
            
        }

        public string Text
        {
            get { return UndrelyingControls.First().Text; }
        }

        public void OnAction()
        {
            foreach (IMenuItem child in UndrelyingControls)
            {
                child.OnAction();
            }
        }
    }
}
