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
            ContextMenu.TryDispose();
        }

        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);
            if (close)
            {
                Dispose();
            }
        }
        
        protected internal virtual void OnAttached()
        {
            ClientMessageBus.Current.SendMessage(this, Constants.Message.ViewModelActivated, null);
        }

        protected internal virtual void OnDeattached()
        {
            ClientMessageBus.Current.SendMessage(this, Constants.Message.ViewModelDeactivated, null);
        }

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
