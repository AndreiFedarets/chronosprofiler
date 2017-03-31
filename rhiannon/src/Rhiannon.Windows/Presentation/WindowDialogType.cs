using System;

namespace Rhiannon.Windows.Presentation
{
	[Flags]
	public enum WindowDialogType
	{
		None = 0x00,
		Ok = 0x01,
		Cancel = 0x02,
		OkCancel = Ok | Cancel
	}
}
