using Chronos.Client.Win.ViewModels;
using Chronos.Model;
using System;
using System.Collections.Generic;

namespace Chronos.Client.Win.DotNet.FindReference
{
    internal class ContextMenuIntegrationCollection : IDisposable
    {
        private readonly Dictionary<Type, IContextMenuIntegration> _collection;
        private readonly IProfilingApplication _application;

        public ContextMenuIntegrationCollection(IProfilingApplication application)
        {
            _application = application;
            _collection = new Dictionary<Type, IContextMenuIntegration>();
            _application.ViewModelManager.ViewAttached += OnViewAttached;
        }

        public void Register<TU, TI>()
            where TU : UnitBase
            where TI : IContextMenuIntegration, new()
        {
            TI integration = new TI();
            _collection.Add(typeof(TU), integration);
        }

        public void Dispose()
        {
            _application.ViewModelManager.ViewAttached -= OnViewAttached;
        }

        private void OnViewAttached(object sender, ViewModelEventArgs e)
        {
            UnitsViewModel viewModel = e.ViewModel as UnitsViewModel;
            if (viewModel == null)
            {
                return;
            }
            Type unitType = viewModel.UnitType;
            IContextMenuIntegration integration;
            if (_collection.TryGetValue(unitType, out integration))
            {
                integration.Integrate(viewModel);
            }
        }
    }
}
