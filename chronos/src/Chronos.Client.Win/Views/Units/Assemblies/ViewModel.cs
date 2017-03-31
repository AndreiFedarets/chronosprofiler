using System.Windows.Input;
using Chronos.Core;
using Rhiannon.Windows.Presentation;
using Rhiannon.Windows.Presentation.Commands;

namespace Chronos.Client.Win.Views.Units.Assemblies
{
    public class ViewModel : ViewModel<AssemblyInfo>, IViewModel
    {
        private readonly IProcessShadow _processShadow;
        private readonly IViewsManager _viewsManager;
        private readonly IProcessShadowNavigator _processShadowNavigator;

        public ViewModel(IProcessShadow processShadow, IViewsManager viewsManager, IProcessShadowNavigator processShadowNavigator)
            : base(processShadow.Assemblies, processShadow)
        {
            _processShadow = processShadow;
            _viewsManager = viewsManager;
            _processShadowNavigator = processShadowNavigator;
            processShadow.Assemblies.UnitsUpdated += OnUnitsUpdated;
        }

        public ICommand FindAllReferencesCommand { get; private set; }
        public ICommand GoToLoadedCommand { get; private set; }

        public override void Dispose()
        {
            base.Dispose();
            _processShadow.Assemblies.UnitsUpdated -= OnUnitsUpdated;
        }

        protected override void InitializeInternal()
        {
            base.InitializeInternal();
            FindAllReferencesCommand = new SyncCommand<AssemblyInfo>(FindAllReferences);
            GoToLoadedCommand = new SyncCommand<AssemblyInfo>(GoToLoaded);
        }

        private void FindAllReferences(AssemblyInfo unit)
        {
            IWindow window = _viewsManager.ResolveAndWrap(ViewNames.References.Assembly, unit);
            window.Open();
        }

        private void GoToLoaded(AssemblyInfo unit)
        {
            _processShadowNavigator.NavigateLoaded(unit);
        }
    }
}
