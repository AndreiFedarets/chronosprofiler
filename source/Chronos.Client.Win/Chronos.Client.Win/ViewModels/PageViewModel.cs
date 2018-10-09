using Chronos.Client.Win.Contracts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace Chronos.Client.Win.ViewModels
{
    public abstract class PageViewModel : ViewModel, IEnumerable<ViewModel>
    {
        internal readonly ObservableCollection<ViewModel> Items;
        protected readonly ContractCollection Contracts;
        private IContainer _container;

        protected PageViewModel()
        {
            Items = new ObservableCollection<ViewModel>();
            Items.CollectionChanged += OnCollectionChanged;
            Contracts = new ContractCollection();
        }

        protected IContainer Container
        {
            get
            {
                if (_container == null)
                {
                    _container = new Container();
                    ConfigureContainer();
                }
                return _container;
            }
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public bool Contains(ViewModel viewModel)
        {
            return viewModel != null && Items.Contains(viewModel);
        }

        public bool Contains(Guid instanceId)
        {
            ViewModel viewModel = FindByInstanceId(instanceId);
            return Contains(viewModel);
        }

        public bool Add(ViewModel viewModel)
        {
            if (viewModel == null || Items.Contains(viewModel))
            {
                return false;
            }
            Items.Add(viewModel);
            Contracts.RegisterItem(viewModel);
            viewModel.Page = this;
            //viewModel.OnAttached();
            ViewModelManager.Current.OnViewAttached(this, viewModel);
            return true;
        }

        public bool Remove(Guid instanceId)
        {
            ViewModel viewModel = FindByInstanceId(instanceId);
            return Remove(viewModel);
        }

        public bool Remove(ViewModel viewModel)
        {
            if (viewModel == null || !Items.Contains(viewModel))
            {
                return false;
            }
            Contracts.UnregisterItem(viewModel);
            Items.Remove(viewModel);
            //viewModel.OnDeattached();
            ViewModelManager.Current.OnViewDeattached(this, viewModel);
            return true;
        }

        public void Clear()
        {
            List<ViewModel> viewModels = new List<ViewModel>(Items);
            foreach (ViewModel viewModel in viewModels)
            {
                Remove(viewModel);
            }
        }

        public ViewModel FindByInstanceId(Guid instanceId)
        {
            return Items.FirstOrDefault(x => x.InstanceId == instanceId);
        }

        public IEnumerator<ViewModel> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        protected virtual void ConfigureContainer()
        {
        }

        protected virtual void BuildLayout()
        {

        }

        protected virtual void RegisterContracts()
        {
            RegisterDeclaredContracts();
        }

        protected void RegisterDeclaredContracts()
        {
            IEnumerable<EnableContractAttribute> attributes = EnableContractAttribute.GetContractAttributes(GetType());
            foreach (EnableContractAttribute attribute in attributes)
            {
                Type contractType = attribute.ContractType;
                object contractObject = Activator.CreateInstance(contractType);
                IContract contract = (IContract) contractObject;
                Contracts.RegisterContract(contract);
            }
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            NotifyCollectionChangedEventHandler handler = CollectionChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            RegisterContracts();
            Contracts.RegisterItem(this);
            BuildLayout();
        }

        public override void Dispose()
        {
            foreach (ViewModel viewModel in Items)
            {
                viewModel.Dispose();
            }
            Items.Clear();
            base.Dispose();
        }
    }
}
