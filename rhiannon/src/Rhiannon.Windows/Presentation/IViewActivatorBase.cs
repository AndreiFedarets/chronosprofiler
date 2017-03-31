using System;
using Rhiannon.Unity;

namespace Rhiannon.Windows.Presentation
{
	public interface IViewActivatorBase
	{
		Type ViewType { get; }

		string ViewName { get; }

		IViewBase Activate(IContainer container, params object[] args);

        IWindow ActivateAndWrap(IContainer container, params object[] args);
	}
}
