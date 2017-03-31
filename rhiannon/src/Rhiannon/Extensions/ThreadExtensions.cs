using System.Threading;

namespace Rhiannon.Extensions
{
	public static class ThreadExtensions
	{
		public static bool IsMainThread
		{
			get { return Thread.CurrentThread.ManagedThreadId == 1; }
		}

		public static bool IsStaThread
		{
			get { return Thread.CurrentThread.GetApartmentState() == ApartmentState.STA; }
		}
	}
}
