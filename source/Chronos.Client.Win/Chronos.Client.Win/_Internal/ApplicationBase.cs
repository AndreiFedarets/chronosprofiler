using Chronos.Win32;
using Layex;
using Layex.Layouts;
using System;
using System.Diagnostics;

namespace Chronos.Client.Win
{
    internal abstract class ApplicationBase : Client.ApplicationBase, IApplicationBase
    {
        private Bootstrapper _bootstrapper;
        private readonly Guid _uid;
        private string _mainViewModelName;

        protected ApplicationBase(Guid uid, bool processOwner, string mainViewModelName)
            : base(processOwner)
        {
            _uid = uid;
            _mainViewModelName = mainViewModelName;
        }

        public override Guid Uid
        {
            get { return _uid; }
        }

        protected override string ApplicationCode
        {
            get { return Constants.ApplicationCodeName.WinClient; }
        }

        protected abstract IDependencyContainer Container { get; }

        protected abstract ILayoutProvider LayoutProvider { get; }

        public void Activate()
        {
            using (Process process = Process.GetCurrentProcess())
            {
                User32.SetForegroundWindow(process.MainWindowHandle);
            }
        }

        protected override void OnEndInitialize()
        {
            base.OnEndInitialize();
            _bootstrapper = new Bootstrapper(Container, LayoutProvider, _mainViewModelName);
            _bootstrapper.Initialize();
        }

        public override void Dispose()
        {
            Properties.Settings.Default.Save();
            base.Dispose();
        }
    }
}