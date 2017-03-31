using System;

namespace Rhiannon.Extensions
{
	public class FileSystemSize
	{
		public FileSystemSize(long bytes)
		{
			Bytes = bytes;
		}

		public long Bytes { get; private set; }

		public double KiloBytes
		{
			get { return Math.Round((double) Bytes/1024, 2); }
		}

		public double MegaBytes
		{
			get { return Math.Round(KiloBytes / 1024, 2); }
		}
	}
}
