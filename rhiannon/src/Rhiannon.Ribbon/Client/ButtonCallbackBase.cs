namespace Rhiannon.Ribbon.Client
{
	public class ButtonCallbackBase : ControlCallbackBase, IButtonCallback
	{
		public ButtonCallbackBase(string controlId)
			: base(controlId)
		{
		}

		public virtual void OnAction()
		{

		}
	}
}
