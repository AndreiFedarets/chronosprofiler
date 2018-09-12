using System;

namespace Chronos
{
    public sealed class ChronosApplicationLauncher<T> : RemoteBaseObject, IDisposable where T : ChronosApplication
    {
        private ChronosApplication _application;

        public void Run(params object[] args)
        {
            _application = (ChronosApplication)Activator.CreateInstance(typeof(T), args);
            _application.Run();
        }

        public ChronosApplication GetApplication()
        {
            return _application;
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }

        public override void Dispose()
        {
            base.Dispose();
            _application.Close();
        }
    }
}
