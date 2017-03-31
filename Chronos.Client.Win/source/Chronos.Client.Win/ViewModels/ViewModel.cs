using System;
using Caliburn.Micro;

namespace Chronos.Client.Win.ViewModels
{
    public abstract class ViewModel : Screen, IDisposable
    {
        protected ViewModel()
        {
            TypeId = GetTypeId(this);
            InstanceId = Guid.NewGuid();
            Deactivated += OnDeactivated;
        }

        private void OnDeactivated(object sender, DeactivationEventArgs e)
        {
            if (e.WasClosed)
            {
                Dispose();
            }
        }

        public virtual Guid TypeId { get; private set; }

        public virtual Guid InstanceId { get; private set; }

        public virtual PageViewModel Page { get; internal set; }

        public virtual void Dispose()
        {
            Deactivated -= OnDeactivated;
        }

        public static Guid GetTypeId(ViewModel viewModel)
        {
            return viewModel.GetType().GUID;
        }

        protected internal virtual void OnAttached()
        {

        }

        public void Attach(ViewModel viewModel)
        {
            if (Page == null)
            {
                throw new TempException();
            }
            if (!(Page is GridViewModel))
            {
                throw new TempException();
            }
            Page.Add(viewModel);
        }

        protected internal virtual void OnDeattached()
        {

        }

        public void Deattach(Guid instanceId)
        {
            if (Page == null)
            {
                throw new TempException();
            }
            if (!(Page is GridViewModel))
            {
                throw new TempException();
            }
            Page.Remove(instanceId);
        }
    }
}
