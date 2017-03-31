using Chronos.Core;

namespace Chronos.Client.Win.Views.Units
{
	public class UnitEventArgs : System.EventArgs
	{
		public UnitEventArgs(UnitBase unit)
		{
			Unit = unit;
		}

		public UnitBase Unit { get; private set; }
	}
}
