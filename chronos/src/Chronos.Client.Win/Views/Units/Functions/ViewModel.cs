using System.Windows.Input;
using Chronos.Core;
using Rhiannon.Windows.Presentation;
using Rhiannon.Windows.Presentation.Commands;

namespace Chronos.Client.Win.Views.Units.Functions
{
	public class ViewModel : ViewModel<FunctionInfo>, IViewModel
	{
        private readonly IProcessShadow _processShadow;
        private readonly IViewsManager _viewsManager;

        public ViewModel(IProcessShadow processShadow, IViewsManager viewsManager)
			: base(processShadow.Functions, processShadow)
		{
            _processShadow = processShadow;
            _viewsManager = viewsManager;
			processShadow.Functions.UnitsUpdated += OnUnitsUpdated;
		}

        public ICommand FindAllReferencesCommand { get; private set; }

        protected override void InitializeInternal()
        {
            base.InitializeInternal();
            FindAllReferencesCommand = new SyncCommand<FunctionInfo>(FindAllReferences);
        }

        private void FindAllReferences(FunctionInfo unit)
        {
            IWindow window = _viewsManager.ResolveAndWrap(ViewNames.References.Function, unit);
            window.Open();
        }

		public override void Dispose()
		{
			base.Dispose();
			_processShadow.Functions.UnitsUpdated -= OnUnitsUpdated;
		}
	}
}
