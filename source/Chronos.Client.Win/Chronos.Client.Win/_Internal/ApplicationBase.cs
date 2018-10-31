using System;
using System.Diagnostics;
using Adenium;
using Adenium.Layouting;
using Caliburn.Micro;
using Chronos.Settings;
using Chronos.Win32;

namespace Chronos.Client.Win
{
    internal abstract class ApplicationBase : Client.ApplicationBase, IApplicationBase
    {
        protected readonly IContainer Container;
        private readonly Bootstrapper _bootstrapper;
        private readonly Guid _uid;

        protected ApplicationBase(Guid uid)
            : this(uid, true)
        {
        }

        protected ApplicationBase(Guid uid, bool processOwner)
            : base(processOwner)
        {
            _uid = uid;
            Container = new Container();
            _bootstrapper = new Bootstrapper(Container);
        }

        public override Guid Uid
        {
            get { return _uid; }
        }

        protected override string ApplicationCode
        {
            get { return Constants.ApplicationCodeName.WinClient; }
        }

        public IContainerViewModel MainViewModel { get; private set; }

        public IWindowsManager WindowsManager { get; private set; }

        public void Activate()
        {
            using (Process process = Process.GetCurrentProcess())
            {
                User32.SetForegroundWindow(process.MainWindowHandle);
            }
        }

        protected override void RunInternal()
        {
            base.RunInternal();
            ConfigureContainer(Container);
            _bootstrapper.Initialize();
            MainViewModel = BuildMainViewModel();
        }

        protected override void OnEndInitialize()
        {
            base.OnEndInitialize();
            ShowMainWindow();
        }

        protected abstract IContainerViewModel BuildMainViewModel();

        private void ShowMainWindow()
        {
            WindowsManager.ShowWindow(MainViewModel);
        }

        protected virtual void ConfigureContainer(IContainer container)
        {
            container.RegisterInstance<IApplicationBase>(this);
            container.RegisterInstance<IApplicationSettings>(ApplicationSettings);
            WindowsManager = container.Resolve<CustomWindowManager>();
            container.RegisterInstance<IWindowsManager>(WindowsManager);
            container.RegisterInstance<IWindowManager>((IWindowManager)WindowsManager);
            ILayoutProvider layoutProvider = container.Resolve<ClientLayoutProvider>();
            CompositeLayoutProvider.Initialize(container);
            CompositeLayoutProvider.Register(layoutProvider);
        }

        public override void Dispose()
        {
            MainViewModel.Dispose();
            Properties.Settings.Default.Save();
            base.Dispose();
        }
    }
}