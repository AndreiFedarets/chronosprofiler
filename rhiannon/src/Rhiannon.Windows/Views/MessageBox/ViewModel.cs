using Rhiannon.Windows.Presentation;
using System.Windows;

namespace Rhiannon.Windows.Views.MessageBox
{
	public class ViewModel : ViewModelBase, IViewModel
	{
	    private MessageBoxImage _image;
	    private MessageBoxButton _button;
	    private string _message;

	    public MessageBoxImage Image
	    {
            get { return _image; }
            set { SetPropertyAndNotifyChanged(() => Image, ref _image, value); }
	    }

        public MessageBoxButton Button
        {
            get { return _button; }
            set { SetPropertyAndNotifyChanged(() => Button, ref _button, value); }
        }

	    public string Message
	    {
            get { return _message; }
            set { SetPropertyAndNotifyChanged(() => Message, ref _message, value); }
	    }
	}
}
