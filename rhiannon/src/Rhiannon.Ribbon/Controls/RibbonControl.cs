using System.Collections;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Rhiannon.Ribbon.Controls
{
	[TemplatePart(Name = ItemsControlPartName, Type = typeof(ItemsControl))]
	public class RibbonControl : Control, IControlPresenter
	{
		private const string ItemsControlPartName = "ItemsControl";

		public static readonly DependencyProperty ControlObjectProperty;
		public static readonly DependencyProperty LabelProperty;

		private ItemsControl _itemsControl;

		static RibbonControl()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(RibbonControl), new FrameworkPropertyMetadata(typeof(RibbonControl)));
			ControlObjectProperty = DependencyProperty.Register("ControlObject", typeof(IControl), typeof(RibbonControl), new PropertyMetadata(null, OnControlObjectPropertyChanged));
			LabelProperty = DependencyProperty.Register("Label", typeof(string), typeof(RibbonControl), new PropertyMetadata(string.Empty));
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			_itemsControl = GetTemplateChild(ItemsControlPartName) as ItemsControl;
			Rebuild();
		}

		public IControl ControlObject
		{
			get { return (IControl)GetValue(ControlObjectProperty); }
			set { SetValue(ControlObjectProperty, value); }
		}

		public string Label
		{
			get { return (string)GetValue(LabelProperty); }
			set { SetValue(LabelProperty, value); }
		}

		public bool Visible
		{
			get { return Visibility == Visibility.Visible; }
			set { Visibility = value ? Visibility.Visible : Visibility.Collapsed; }
		}

		public bool Enabled
		{
			get { return IsEnabled; }
			set { IsEnabled = value; }
		}

		private void Rebuild()
		{
			if (ControlObject == null)
			{
				return;
			}
			//Label = ControlObject.Label;
			IHavePresenter havePresenter = ControlObject as IHavePresenter;
			if (havePresenter != null)
			{
				havePresenter.AttachPresenter(this);
			}
			ObservableCollection<RibbonControl> controls = new ObservableCollection<RibbonControl>();
			IEnumerable enumerable = ControlObject as IEnumerable;
			if (enumerable == null || _itemsControl == null)
			{
				return;
			}
			foreach (object @object in enumerable)
			{
				IControl controlObject = @object as IControl;
				RibbonControl control = RibbonControlFactory.CreateControl(controlObject);
				controls.Add(control);
			}
			_itemsControl.ItemsSource = controls;
		}

		private static void OnControlObjectPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			RibbonControl ribbonControl = sender as RibbonControl;
			if (ribbonControl != null)
			{
				ribbonControl.Rebuild();
			}
		}
	}
}
