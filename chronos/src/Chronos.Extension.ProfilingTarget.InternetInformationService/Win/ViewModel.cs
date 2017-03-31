using System.Collections.Generic;
using System.Linq;
using Rhiannon.Windows.Presentation;

namespace Chronos.Extension.ProfilingTarget.InternetInformationService.Win
{
	public class ViewModel : ViewModelBase, IViewModel
	{
		private readonly IInternetInformationService _internetInformationService;
		private IApplicationPool _selectedApplicationPool;

		public ViewModel()
		{
			_internetInformationService = new InternetInformationService();
		}

        public IApplicationPool SelectedApplicationPool
        {
            get { return _selectedApplicationPool; }
            set { SetPropertyAndNotifyChanged(() => SelectedApplicationPool, ref _selectedApplicationPool, value); }
		}

		public IEnumerable<IApplicationPool> ApplicationPools
		{
			get { return _internetInformationService.ApplicationPools; }
		}

        protected override void InitializeInternal()
        {
            base.InitializeInternal();
            SelectedApplicationPool = ApplicationPools.FirstOrDefault();
        }
	}
}
