using System;
using System.Reflection;

namespace Adenium.Layouting
{
    internal abstract class ViewModelFactoryBase : IViewModelFactory
    {
        private readonly IContainerViewModel _parentViewModel;
        private readonly IContainer _container;
        private readonly string _typeName;

        protected ViewModelFactoryBase(IContainerViewModel parentViewModel, string typeName, IContainer container)
        {
            _parentViewModel = parentViewModel;
            _typeName = typeName;
            _container = container;
        }

        public abstract IViewModel CreateViewModel();

        protected IViewModel CreateViewModelInternal()
        {
            Type viewModelType = Type.GetType(_typeName);
            IContainer container = _container.CreateChildContainer();
            ConfigureContainer(container, _parentViewModel);
            return (IViewModel)container.Resolve(viewModelType);
        }

        private void ConfigureContainer(IContainer container, IContainerViewModel parentViewModel)
        {
            if (parentViewModel == null)
            {
                return;
            }
            Type parentViewModelType = parentViewModel.GetType();
            foreach (PropertyInfo propertyInfo in parentViewModelType.GetProperties())
            {
                object[] attributes = propertyInfo.GetCustomAttributes(typeof(ViewModelExportAttribute), true);
                if (attributes.Length > 0)
                {
                    Type exportType = propertyInfo.PropertyType;
                    object exportValue = propertyInfo.GetValue(parentViewModel, null);
                    container.RegisterInstance(exportType, exportValue);
                }
            }
            ConfigureContainer(container, parentViewModel.Parent);
        }
    }
}
