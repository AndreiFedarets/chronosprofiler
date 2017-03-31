using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Rhiannon.Windows.Controls;

namespace Rhiannon.Windows.Presentation.ApplyResourceBehavior
{
	internal static class ResourceBehaviorCollection
	{
		private readonly static IDictionary<Type, BehaviorBase> Behaviors;

		static ResourceBehaviorCollection()
		{
			Behaviors = new Dictionary<Type, BehaviorBase>();
			Register<UserControlView>(new UserControlViewBehavior());
			Register<WindowView>(new WindowViewBehavior());
			Register<TextBlock>(new TextBlockBehavior());
			Register<HeaderedItemsControl>(new HeaderedItemsControlBehavior());
			Register<HeaderedContentControl>(new HeaderedContentControlBehavior());
			Register<ContentControl>(new ContentControlBehavior());
			Register<MenuItem>(new MenuItemBehavior());
			Register<Grid>(new GridBehavior());
			Register<ColumnDefinition>(new ColumnDefinitionBehavior());
			Register<RowDefinition>(new RowDefinitionBehavior());
			Register<ListView>(new ListViewBehavior());
			Register<ContextMenu>(new ContextMenuBehavior());
			Register<TextBox>(new TextBoxBehavior());
			Register<Button>(new ButtonBehavior());
			Register<ImageButton>(new ImageButtonBehavior());
		}

		public static BehaviorBase Resolve<T>() where T : DependencyObject
		{
			return Resolve(typeof (T));
		}

		public static BehaviorBase Resolve(Type type)
		{
			BehaviorBase behavior;
			if (!Behaviors.TryGetValue(type, out behavior))
			{
				behavior = new EmptyBehavior();
				Behaviors.Add(type, behavior);
			}
			return behavior;
		}

		public static void Register<T>(BehaviorBase behavior) where T : DependencyObject
		{
			Behaviors.Add(typeof(T), behavior);
		}
	}
}
