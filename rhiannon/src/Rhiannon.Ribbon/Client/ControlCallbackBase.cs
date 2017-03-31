namespace Rhiannon.Ribbon.Client
{
	public abstract class ControlCallbackBase : IControlCallback
	{
		protected IControl AssociatedControl;

		protected ControlCallbackBase(string controlId)
		{
			ControlId = controlId;
		}

		public string ControlId { get; private set; }

		public virtual bool GetVisible()
		{
			return true;
		}

		public virtual bool GetEnabled()
		{
			return true;
		}

		public virtual string GetLabel()
		{
			return null;
		}

		public virtual void ControlLoaded(IControl control)
		{
			AssociatedControl = control;
		}

		protected void Invalidate()
		{
			if (AssociatedControl != null)
			{
				AssociatedControl.Invalidate();
			}
		}
	}
}
