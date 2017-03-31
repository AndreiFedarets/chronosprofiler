using Rhiannon.Windows.Presentation;

namespace Chronos.Client.Win
{
	public class ProfilingFilterItem : PropertyChangedNotifier
	{
		private bool _isChecked;

        public ProfilingFilterItem(AssemblyName assemblyName)
		{
            AssemblyName = assemblyName;
		}

		public ProfilingFilterItem(System.Reflection.AssemblyName assemblyName)
		{
            AssemblyName = new AssemblyName(assemblyName);
		}

        public ProfilingFilterItem(string assemblyName)
        {
            AssemblyName = new AssemblyName(assemblyName);
        }

		public bool IsChecked
		{
			get { return _isChecked; }
			set { SetPropertyAndNotifyChanged(() => IsChecked, ref _isChecked, value); }
		}

		public string Name
		{
			get { return AssemblyName.Name; }
		}

		public AssemblyName AssemblyName { get; private set; }
	}
}
