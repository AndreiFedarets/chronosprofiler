using System.Collections;
using System.Windows;
using System.Windows.Controls;
using Rhiannon.Resources;

namespace Rhiannon.Windows.Presentation.ApplyResourceBehavior
{
	internal abstract class BehaviorBase
	{
		protected abstract void ApplyElement(DependencyObject element, IResourceProvider provider);

		public void ApplyRecursively(DependencyObject element, IResourceProvider provider)
		{
			Apply(element, provider);
			ApplyChildern(element, provider);
		}

		public void Apply(DependencyObject element, IResourceProvider provider)
		{
			ApplyContentControl(element, provider);
			ApplyElement(element, provider);
			ApplyContextMenu(element, provider);
		}

		private void ApplyContentControl(DependencyObject element, IResourceProvider provider)
		{
			Control control = element as Control;
			if (control != null)
			{
				var visualTree = control.Template;
				if (visualTree != null)
				{

				}
			}
		}

		public void ApplyChildern(DependencyObject element, IResourceProvider provider)
		{
			if (element == null)
			{
				return;
			}
			IEnumerable children = LogicalTreeHelper.GetChildren(element);
			foreach (object child in children)
			{
				DependencyObject childElement = child as DependencyObject;
				if (childElement != null)
				{
					BehaviorBase behavior = ResourceBehaviorCollection.Resolve(childElement.GetType());
					behavior.ApplyRecursively(childElement, provider);
				}
			}
		}

		public void ApplyContextMenu(DependencyObject element, IResourceProvider provider)
		{
			FrameworkElement frameworkElement = element as FrameworkElement;
			if (frameworkElement != null)
			{
				BehaviorBase behavior = ResourceBehaviorCollection.Resolve<ContextMenu>();
				behavior.ApplyRecursively(frameworkElement.ContextMenu, provider);
			}
		}
	}
}
