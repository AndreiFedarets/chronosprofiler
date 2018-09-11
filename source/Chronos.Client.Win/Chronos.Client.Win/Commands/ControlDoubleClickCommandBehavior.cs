using System.Windows.Controls;
using System.Windows.Input;

namespace Chronos.Client.Win.Commands
{
    public class ControlDoubleClickCommandBehavior : ControlEventCommandBehavior
    {
        public ControlDoubleClickCommandBehavior(Control control)
            : base(control)
        {

        }

        public override string EventName
        {
            get { return "MouseDoubleClick"; }
        }

        protected override string HandlerMethodName
        {
            get { return "OnMouseDoubleClick"; }
        }

        public void OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ExecuteCommand();
        }
    }
}
