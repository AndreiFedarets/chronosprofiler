using System.Xml.Serialization;

namespace Chronos.Configuration
{
	[XmlRoot("daemon")]
	public class DaemonConfiguration
	{
		[XmlElement("entryPoint")]
		public string EntryPoint { get; set; }

		[XmlElement("binaryPath")]
		public string BinaryPath { get; set; }

        [XmlElement("threadStreamsCount")]
        public uint ThreadStreamsCount { get; set; }

        [XmlElement("threadMergersCount")]
        public uint ThreadMergersCount { get; set; }

        [XmlElement("threadMergersPool")]
        public uint ThreadStreamsPool { get; set; }

		[XmlElement("exitTimeout")]
		public int ExitTimeout { get; set; }

		[XmlElement("autoExitTimeout")]
		public int AutoExitTimeout { get; set; }

		[XmlElement("callPageSize")]
		public uint CallPageSize { get; set; }

		[XmlElement("performanceSamplingPeriod")]
		public int PerformanceSamplingPeriod { get; set; }

		[XmlElement("runtype")]
		public string Runtype { get; set; }

	}
}
