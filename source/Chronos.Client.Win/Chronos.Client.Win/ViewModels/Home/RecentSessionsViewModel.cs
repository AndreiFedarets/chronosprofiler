namespace Chronos.Client.Win.ViewModels.Home
{
    public class RecentSessionsViewModel : ViewModel
    {
        private readonly IMainApplication _application;

        public RecentSessionsViewModel(IMainApplication application)
        {
            _application = application;
        }

        public override string DisplayName
        {
            get { return "Recent Sessions"; }
        }
    }
}
