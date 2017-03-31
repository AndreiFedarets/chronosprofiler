using System;
using System.Windows;
using System.Windows.Input;

namespace Rhiannon.Windows.Presentation.Commands
{
	public class KeyboardCommand : DependencyObject
	{
		public static readonly DependencyProperty KeyProperty;
		public static readonly DependencyProperty ModifiersProperty;
		public static readonly DependencyProperty CommandProperty;
		public static readonly DependencyProperty CommandParameterProperty;

		static KeyboardCommand()
		{
			KeyProperty = DependencyProperty.Register("Key", typeof(Key), typeof(KeyboardCommand), new PropertyMetadata(Key.None));
			ModifiersProperty = DependencyProperty.Register("Modifiers", typeof(ModifierKeys), typeof(KeyboardCommand), new PropertyMetadata(ModifierKeys.None));
			CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(KeyboardCommand), new PropertyMetadata(default(ICommand)));
			CommandParameterProperty = DependencyProperty.Register("CommandParameter", typeof(object), typeof(KeyboardCommand), new PropertyMetadata(default(object)));
		}

		public Key Key
		{
			get { return (Key) GetValue(KeyProperty); }
			set { SetValue(KeyProperty, value); }
		}

		public ModifierKeys Modifiers
		{
			get { return (ModifierKeys)GetValue(ModifiersProperty); }
			set { SetValue(ModifiersProperty, value); }
		}

		public ICommand Command
		{
			get { return (ICommand) GetValue(CommandProperty); }
			set { SetValue(CommandProperty, value); }
		}

		public object CommandParameter
		{
			get { return GetValue(CommandParameterProperty); }
			set { SetValue(CommandParameterProperty, value); }
		}

		public bool CanExecute(Key key, ModifierKeys modifiers)
		{
			return key == Key && modifiers == Modifiers && Command != null && Command.CanExecute(Parameter);
		}

		public void Execute()
		{
			Command.Execute(Parameter);
		}

		public Func<object> GetParameterFunc { get; set; }

		public object Parameter
		{
			get
			{
				object parameter = null;
				if (GetParameterFunc != null)
				{
					parameter = GetParameterFunc();
				}
				return parameter;
			}
		}
	}
}
