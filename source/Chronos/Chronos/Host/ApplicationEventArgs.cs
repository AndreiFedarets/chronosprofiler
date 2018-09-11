using System;

namespace Chronos.Host
{
    [Serializable]
    public sealed class ApplicationEventArgs : EventArgs
    {
        public ApplicationEventArgs(IApplication application)
        {
            Application = application;
        }

        public IApplication Application { get; private set; }

        internal static void RaiseEvent(EventHandler<ApplicationEventArgs> handler, object sender, IApplication application)
        {
            if (handler != null)
            {
                handler(sender, new ApplicationEventArgs(application));
            }
        }
    }
}
