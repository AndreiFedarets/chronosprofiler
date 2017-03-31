using System.Windows;

namespace Rhiannon.Windows.Presentation
{
	public class SimpleNotificationService : INotificationService
	{
		public void Show(string message)
		{
			MessageBox.Show(message);
		}
	}
}
