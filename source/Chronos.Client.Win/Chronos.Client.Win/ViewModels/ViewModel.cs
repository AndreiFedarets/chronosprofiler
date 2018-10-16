using System;
using Caliburn.Micro;
using Chronos.Client.Win.Menu;

namespace Chronos.Client.Win.ViewModels
{
    public abstract class ViewModel : Screen, IViewModel, IDisposable
    {
        private readonly ViewModelContext _context;

        protected ViewModel()
        {
            _context = new ViewModelContext(this);
        }

        public virtual Guid TypeId
        {
            get { return _context.TypeId; }
        }

        //TODO: I don't like it here
        public IMenu ContextMenu
        {
            get { return _context.ContextMenu; }
        }

        public virtual Guid InstanceId
        {
            get { return _context.InstanceId; }
        }

        public new IContainerViewModel Parent
        {
            get { return base.Parent as IContainerViewModel; }
        }

        public virtual void Dispose()
        {
            _context.TryDispose();
        }

        //protected override void OnDeactivate(bool close)
        //{
        //    base.OnDeactivate(close);
        //    if (close)
        //    {
        //        Dispose();
        //    }
        //}
        
        //protected internal virtual void OnAttached()
        //{
        //    ClientMessageBus.Current.SendMessage(this, Constants.Message.ViewModelActivated, null);
        //}

        //protected internal virtual void OnDeattached()
        //{
        //    ClientMessageBus.Current.SendMessage(this, Constants.Message.ViewModelDeactivated, null);
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
