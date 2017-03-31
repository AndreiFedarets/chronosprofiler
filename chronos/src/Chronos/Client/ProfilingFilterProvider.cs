using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Chronos.Core;
using Rhiannon.Serialization;

namespace Chronos.Client
{
	public class ProfilingFilterProvider : IProfilingFilterProvider
	{
        private const string DotNetFrameworkFilterName = ".Net Framework";
        private const string NothingFilterName = "Nothing";
		private readonly List<ProfilingFilter> _filters;
		private readonly ISerializerFactory _serializersFactory;
		private readonly string _fillName;

		public ProfilingFilterProvider(IProgramFilesSettings programFilesSettings, ISerializerFactory serializersFactory)
		{
			_fillName = programFilesSettings.ProfilingFiltersFile;
			_serializersFactory = serializersFactory;
			_filters = LoadInternal();
		}

		public void SetAsDefault(ProfilingFilter filter)
		{
			ProfilingFilter defaultConfiguration = _filters.First(x => x.IsDefault);
			defaultConfiguration.IsDefault = false;
			filter.IsDefault = true;
			SafeInternal(_filters);
		}

		public IEnumerable<ProfilingFilter> Load()
		{
			return _filters;
		}

        public ProfilingFilter Create(string filterName)
		{
            ProfilingFilter filter = new ProfilingFilter(filterName);
            _filters.Add(filter);
			SafeInternal(_filters);
            return filter;
		}

        public void Delete(ProfilingFilter filter)
		{
            _filters.Remove(filter);
			SafeInternal(_filters);
		}

        public void Save(ProfilingFilter filter)
		{
            _filters.Remove(filter);
            _filters.Add(filter);
			SafeInternal(_filters);
		}

		public ProfilingFilter GetDefault()
		{
			ProfilingFilter @default = _filters.First(x => x.IsDefault);
			return @default;
		}

		public ProfilingFilter Find(FilterType filterType, List<string> items)
		{
			foreach (ProfilingFilter configuration in _filters)
			{
				if (configuration.Type != filterType || configuration.Items.Count != items.Count)
				{
					continue;
				}
				bool equals = true;
				foreach (string item in items)
				{
					string name = item;
					if (!configuration.Items.Any(x => string.Equals(x.Name, name, StringComparison.CurrentCulture)))
					{
						equals = false;
					}
				}
				if (equals)
				{
					return configuration;
				}
			}
			return null;
		}

		private List<ProfilingFilter> LoadInternal()
		{
			FileInfo fileInfo = new FileInfo(_fillName);
			ISerializer serializer = _serializersFactory.Create<List<ProfilingFilter>>(SerializerType.Xml);
            List<ProfilingFilter> filters;
			if (!fileInfo.Exists)
			{
                filters = GetDefaults();
                SafeInternal(filters);
			}
			else
			{
                filters = serializer.Deserialize<List<ProfilingFilter>>(fileInfo);
			}
            return filters;
		}

		private void SafeInternal(List<ProfilingFilter> configurations)
		{
			FileInfo fileInfo = new FileInfo(_fillName);
			if (fileInfo.Exists)
			{
				fileInfo.Delete();
			}
			ISerializer serializer = _serializersFactory.Create<List<ProfilingFilter>>(SerializerType.Xml);
			serializer.Serialize(fileInfo, configurations);
		}

        public void Restore(ProfilingFilter filter)
        {
            ProfilingFilter restoredFilter;
            switch (filter.Name)
            {
                case DotNetFrameworkFilterName:
                    restoredFilter = GetDotNetFrameworkFilter();
                    break;
                case NothingFilterName:
                    restoredFilter = GetDotNetFrameworkFilter();
                    break;
                default:
                    restoredFilter = null;
                    break;
            }
            if (restoredFilter != null)
            {
                _filters.Remove(filter);
                _filters.Add(restoredFilter);
                SafeInternal(_filters);
            }
        }

		private List<ProfilingFilter> GetDefaults()
		{
            ProfilingFilter defaultFilter = GetDotNetFrameworkFilter();
		    ProfilingFilter emptyFilter = GetNothingFilter();
			return new List<ProfilingFilter> { defaultFilter, emptyFilter };
		}

        private ProfilingFilter GetDotNetFrameworkFilter()
        {
            ProfilingFilter filter = new ProfilingFilter(DotNetFrameworkFilterName, true);
            filter.Type = FilterType.Exclude;
            GlobalAssemblyCache gac = new GlobalAssemblyCache();
            filter.Items = gac.Select(x => new AssemblyName(x)).ToList();
            filter.Items.Sort((x, y) => string.Compare(x.Name, y.Name, StringComparison.InvariantCultureIgnoreCase));
            filter.IsEmbedded = true;
            return filter;
        }

        private ProfilingFilter GetNothingFilter()
        {
            ProfilingFilter filter = new ProfilingFilter(NothingFilterName, false);
            filter.Type = FilterType.Exclude;
            filter.Items = new List<AssemblyName>();
            filter.IsEmbedded = true;
            return filter;
        }
    }
}
