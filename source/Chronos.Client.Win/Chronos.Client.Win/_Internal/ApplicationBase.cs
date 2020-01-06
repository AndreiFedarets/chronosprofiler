using Chronos.Settings;
using Chronos.Win32;
using Layex;
using Layex.ViewModels;
using System;
using System.Diagnostics;

namespace Chronos.Client.Win
{
    internal abstract class ApplicationBase : Client.ApplicationBase, IApplicationBase
    {
        private readonly Guid _uid;
        private Bootstrapper _bootstrapper;

        protected ApplicationBase(Guid uid, bool processOwner)
            : base(processOwner)
        {
            _uid = uid;
            _bootstrapper = new Bootstrapper();
            //Debugger.Launch();
        }

        public override Guid Uid
        {
            get { return _uid; }
        }

        protected override string ApplicationCode
        {
            get { return Constants.ApplicationCodeName.WinClient; }
        }

        public IDependencyContainer Container { get; private set; }

        public void Activate()
        {
            using (Process process = Process.GetCurrentProcess())
            {
                User32.SetForegroundWindow(process.MainWindowHandle);
            }
        }

        protected virtual IDependencyContainer GetDependencyContainer()
        {
            return _bootstrapper.DependencyContainer;
        }

        protected override void RunInternal()
        {
            base.RunInternal();
            _bootstrapper.Initialize();
            Container = GetDependencyContainer();
            ConfigureContainer(Container);
        }

        protected override void OnEndInitialize()
        {
            base.OnEndInitialize();
            ShowMainViewModel();
        }

        protected abstract void ShowMainViewModel();

        protected virtual void ConfigureContainer(IDependencyContainer container)
        {
            container.RegisterInstance<IApplicationBase>(this);
            container.RegisterInstance<IApplicationSettings>(ApplicationSettings);
        }

        public override void Dispose()
        {
            Properties.Settings.Default.Save();
            base.Dispose();
        }
    }
}