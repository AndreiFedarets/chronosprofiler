using Rhiannon.Logging;
using Rhiannon.Ribbon.Client;
using Rhiannon.Serialization.Xml;
using Rhiannon.Windows.Presentation;

namespace Rhiannon.Ribbon.Internal
{
	internal abstract class ControlBase : PropertyChangedNotifier, IControl, IHavePresenter
	{
		protected readonly IControlCallback ControlCallback;
		protected readonly INode Node;
		protected readonly IControlFactory Factory;
		protected readonly INodeLocator NodeLocator;
		protected readonly IControlCallbackCollection Callbacks;
		protected IControlPresenter ControlPresenter;
		private bool _enabled;
		private bool _visible;
		private string _label;

		protected ControlBase(INode node, IControlCallbackCollection callbacks, IControlFactory factory, INodeLocator nodeLocator)
		{
			Node = node;
			Factory = factory;
			NodeLocator = nodeLocator;
			Callbacks = callbacks;
			ControlCallback = callbacks[Id];
			LoggingProvider.Current.LogIf(ControlCallback == null, string.Format("callbacks for control with Id='{0}' is null", Id), Constants.Functionality, Policy.Core, LogEntryType.Warning);
			if (ControlCallback != null)
			{
				ControlCallback.ControlLoaded(this);
			}
		}

		public string Id
		{
			get { return Node.Attributes[Constants.Attributes.Id].Value; }
		}

		public bool Enabled
		{
			get { return _enabled; }
			private set
			{
				_enabled = value;
				if (ControlPresenter != null)
				{
					ControlPresenter.Enabled = value;
				}
				NotifyPropertyChanged(() => Enabled);
			}
		}

		public bool Visible
		{
			get { return _visible; }
			private set
			{
				_visible = value;
				if (ControlPresenter != null)
				{
					ControlPresenter.Visible = value;
				}
				NotifyPropertyChanged(() => Visible);
			}
		}

		public string Label
		{
			get { return _label; }
			private set
			{
				_label = value;
				if (ControlPresenter != null)
				{
					ControlPresenter.Label = value;
				}
				NotifyPropertyChanged(() => Label);
			}
		}

		public virtual void Invalidate()
		{
			if (ControlCallback != null)
			{
				Enabled = ControlCallback.GetEnabled();
				Visible = ControlCallback.GetVisible();
				Label = ControlCallback.GetLabel();
			}
		}

		public void AttachPresenter(IControlPresenter controlPresenter)
		{
			ControlPresenter = controlPresenter;
			Invalidate();
		}
	}
}
