using System;
using System.Collections.Generic;
using Chronos.Core;

namespace Chronos.Client
{
	[Serializable]
	public class ProfilingFilter
	{
		public ProfilingFilter(string name, bool isDefault)
		{
			Name = name;
			IsDefault = isDefault;
			Items = new List<AssemblyName>();
		}

		public ProfilingFilter(string name)
			: this(name, false)
		{
        }

		public ProfilingFilter()
			: this(string.Empty, false)
		{
		}

		public string Name { get; set; }

		public bool IsDefault { get; set; }

		public List<AssemblyName> Items { get; set; }

		public FilterType Type { get; set; }

        public bool IsEmbedded { get; set; }
	}
}
