using System;

namespace Chronos
{
    internal sealed class InplaceApplicationManager<T> : BaseObject, IInplaceApplicationManager where T : ChronosApplication
    {
        private AppDomain _appDomain;
        private ChronosApplicationLauncher<T> _activator;

        public void Run(params object[] args)
        {
            _appDomain = AppDomain.CurrentDomain.Clone(typeof(T).ToString(), GetType().GetAssemblyPath());
            _activator = RemoteActivator.CreateInstance<ChronosApplicationLauncher<T>>(_appDomain);
            _activator.Run(args);
        }

        public override void Dispose()
        {
            _activator.Dispose();
            try
            {
                AppDomain.Unload(_appDomain);
            }
            catch (Exception exception)
            {
                LoggingProvider.Current.Log(System.Diagnostics.TraceEventType.Warning, exception);
            }
            base.Dispose();
        }
    }
}
