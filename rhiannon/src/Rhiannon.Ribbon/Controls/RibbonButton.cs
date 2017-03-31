using System.Windows;
using System.Windows.Controls;
using System;

namespace Rhiannon.Ribbon.Controls
{
	[TemplatePart(Name = ButtonPartName, Type = typeof(Button))]
	public class RibbonButton : RibbonControl
	{
		private const string ButtonPartName = "BUTTON_PART";

		private Button _button;

		static RibbonButton()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(RibbonButton), new FrameworkPropertyMetadata(typeof(RibbonButton)));
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			if (_button != null)
			{
				_button.Click -= OnButtonClick;
			}
			_button = GetTemplateChild(ButtonPartName) as Button;
			if (_button != null)
			{
				_button.Click += OnButtonClick;
			}
		}

		private void OnButtonClick(object sender, EventArgs e)
		{
			IButton button = (IButton)ControlObject;
			button.OnAction();
		}
	}
}
