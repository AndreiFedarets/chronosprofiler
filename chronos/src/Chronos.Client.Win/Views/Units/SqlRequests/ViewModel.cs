using Chronos.Core;
using Rhiannon.Windows.Presentation;

namespace Chronos.Client.Win.Views.Units.SqlRequests
{
	public class ViewModel : ViewModel<SqlRequestInfo>, IViewModel
	{
        private readonly IProcessShadow _processShadow;
        private readonly IViewsManager _viewsManager;

        public ViewModel(IProcessShadow processShadow, IViewsManager viewsManager)
			: base(processShadow.SqlRequests, processShadow)
		{
            _processShadow = processShadow;
            _viewsManager = viewsManager;
			processShadow.SqlRequests.UnitsUpdated += OnUnitsUpdated;
		}

        //public ICommand FindAllReferencesCommand { get; private set; }

        //protected override void InitializeInternal()
        //{
        //    base.InitializeInternal();
        //    FindAllReferencesCommand = new SyncCommand<SqlRequestInfo>(FindAllReferences);
        //}

        //private void FindAllReferences(SqlRequestInfo unit)
        //{
        //    IWindow window = _viewsManager.ResolveAndWrap(ViewNames.References.Function, unit);
        //    window.Open();
        //}

		public override void Dispose()
		{
			base.Dispose();
			_processShadow.SqlRequests.UnitsUpdated -= OnUnitsUpdated;
		}
	}
}
