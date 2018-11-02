using System;

namespace Adenium.Layouting
{
    internal abstract class ViewModelFactoryBase : IViewModelFactory
    {
        private readonly IViewModel _targetViewModel;
        private readonly string _typeName;

        protected ViewModelFactoryBase(IViewModel targetViewModel, string typeName)
        {
            _targetViewModel = targetViewModel;
            _typeName = typeName;
        }

        public abstract IViewModel CreateViewModel<T1, T2, T3>(T1 dependency1, T2 dependency2, T3 dependency3);

        protected IViewModel CreateViewModelInternal<T1, T2, T3>(T1 dependency1, T2 dependency2, T3 dependency3)
        {
            Type viewModelType = Type.GetType(_typeName);
            IContainerViewModel parentViewModel = _targetViewModel as IContainerViewModel;
            if (parentViewModel == null)
            {
                parentViewModel = _targetViewModel.Parent;
            }
            IViewModel viewModel = ViewModelManager.Instance.CreateViewModel(viewModelType, parentViewModel, dependency1, dependency2, dependency3);
            return viewModel;
        }
    }
}
