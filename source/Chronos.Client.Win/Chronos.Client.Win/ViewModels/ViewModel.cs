using System;
using Caliburn.Micro;
using Chronos.Client.Win.Menu;

namespace Chronos.Client.Win.ViewModels
{
    public abstract class ViewModel : Screen, IDisposable
    {
        protected ViewModel()
        {
            InstanceId = Guid.NewGuid();
            Deactivated += OnDeactivated;
            ContextMenu = new Menu.Menu();
        }

        public virtual Guid TypeId
        {
            get { return GetType().GUID; }
        }

        public IMenu ContextMenu { get; private set; }

        public virtual Guid InstanceId { get; private set; }

        public virtual PageViewModel Page { get; internal set; }

        public virtual void Dispose()
        {
            Deactivated -= OnDeactivated;
            ContextMenu.TryDispose();
        }

        protected virtual void BuildContextMenu()
        {
            
        }

        private void OnDeactivated(object sender, DeactivationEventArgs e)
        {
            if (e.WasClosed)
            {
                Dispose();
            }
        }

        //protected internal virtual void OnAttached()
        //{

        //}

        //public void Attach(ViewModel viewModel)
        //{
        //    if (Page == null)
        //    {
        //        throw new TempException();
        //    }
        //    if (!(Page is GridViewModel))
        //    {
        //        throw new TempException();
        //    }
        //    Page.Add(viewModel);
        //}

        //protected internal virtual void OnDeattached()
        //{

        //}

        //public void Deattach(Guid instanceId)
        //{
        //    if (Page == null)
        //    {
        //        throw new TempException();
        //    }
        //    if (!(Page is GridViewModel))
        //    {
        //        throw new TempException();
        //    }
        //    Page.Remove(instanceId);
        //}
    }
}
