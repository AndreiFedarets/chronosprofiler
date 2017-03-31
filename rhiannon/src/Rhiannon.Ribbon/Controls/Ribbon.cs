using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Rhiannon.Ribbon.Controls
{
	[TemplatePart(Name = TabControlPartName, Type = typeof(TabControl))]
	public class Ribbon : Control
	{
		private const string TabControlPartName = "TabControl";

		public static readonly DependencyProperty RibbonObjectProperty;

		private TabControl _tabControl;

		static Ribbon()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(Ribbon), new FrameworkPropertyMetadata(typeof(Ribbon)));
			RibbonObjectProperty = DependencyProperty.Register("RibbonObject", typeof(IRibbon), typeof(Ribbon), new PropertyMetadata(null, OnRibbonObjectPropertyChanged));
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			_tabControl = GetTemplateChild(TabControlPartName) as TabControl;
			Rebuild();
		}

		public IRibbon RibbonObject
		{
			get { return (IRibbon)GetValue(RibbonObjectProperty); }
			set { SetValue(RibbonObjectProperty, value); }
		}

		private void Rebuild()
		{
			if (RibbonObject == null)
			{
				return;
			}
			if (_tabControl == null)
			{
				return;
			}
			ObservableCollection<RibbonTab> tabs = new ObservableCollection<RibbonTab>();
			foreach (ITab tabObject in RibbonObject.Tabs)
			{
				RibbonTab tab = RibbonControlFactory.CreateTab(tabObject);
				tabs.Add(tab);
			}
			_tabControl.ItemsSource = tabs;
		}

		private static void OnRibbonObjectPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			Ribbon ribbon = (Ribbon) sender;
			if (ribbon != null)
			{
				ribbon.Rebuild();
			}
		}
	}
}
