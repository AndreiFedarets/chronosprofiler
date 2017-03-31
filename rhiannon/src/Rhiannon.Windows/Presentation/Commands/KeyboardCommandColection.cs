using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace Rhiannon.Windows.Presentation.Commands
{
	public class KeyboardCommandColection : ItemsControl
	{
		private readonly Control _hostControl;
		private readonly ObservableCollection<KeyboardCommand> _commands;

		public KeyboardCommandColection(Control hostControl)
		{
			_hostControl = hostControl;
			_hostControl.KeyDown += OnControlKeyDown;
			_commands = new ObservableCollection<KeyboardCommand>();
		}

		private void OnControlKeyDown(object sender, KeyEventArgs e)
		{
			foreach (KeyboardCommand keyboardCommand in _commands)
			{
				if (keyboardCommand.CanExecute(e.Key, Keyboard.Modifiers))
				{
				    keyboardCommand.Execute();
				}
			}
		}

		public void Register(Key key, ModifierKeys modifiers, ICommand command, Func<object> getParameterFunc)
		{
			KeyboardCommand keyboardCommand = new KeyboardCommand();
			keyboardCommand.Command = command;
			keyboardCommand.Key = key;
			keyboardCommand.Modifiers = modifiers;
			keyboardCommand.GetParameterFunc = getParameterFunc;
			_commands.Add(keyboardCommand);
		}

		public void Register(Key key, ModifierKeys modifiers, ICommand command)
		{
			Register(key, modifiers, command, null);
		}
	}
}
