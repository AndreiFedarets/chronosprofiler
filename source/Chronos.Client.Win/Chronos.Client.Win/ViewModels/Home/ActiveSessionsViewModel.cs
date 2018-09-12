namespace Chronos.Client.Win.ViewModels.Home
{
    public class ActiveSessionsViewModel : ViewModel
    {
        private readonly IMainApplication _application;

        public ActiveSessionsViewModel(IMainApplication application)
        {
            _application = application;
        }

        public override string DisplayName
        {
            get { return "Active Sessions"; }
        }
    }
}
