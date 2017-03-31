using System.Windows.Input;
using Chronos.Core;
using Rhiannon.Windows.Presentation;
using Rhiannon.Windows.Presentation.Commands;

namespace Chronos.Client.Win.Views.Units.Classes
{
	public class ViewModel : ViewModel<ClassInfo>, IViewModel
	{
        private readonly IProcessShadow _processShadow;
        private readonly IViewsManager _viewsManager;

        public ViewModel(IProcessShadow processShadow, IViewsManager viewsManager)
			: base(processShadow.Classes, processShadow)
		{
			_processShadow = processShadow;
            _viewsManager = viewsManager;
			processShadow.Classes.UnitsUpdated += OnUnitsUpdated;
		}

        public ICommand FindAllReferencesCommand { get; private set; }

        protected override void InitializeInternal()
        {
            base.InitializeInternal();
            FindAllReferencesCommand = new SyncCommand<ClassInfo>(FindAllReferences);
        }

        private void FindAllReferences(ClassInfo unit)
        {
            IWindow window = _viewsManager.ResolveAndWrap(ViewNames.References.Class, unit);
            window.Open();
        }

		public override void Dispose()
		{
			base.Dispose();
			_processShadow.Classes.UnitsUpdated -= OnUnitsUpdated;
		}
	}
}
