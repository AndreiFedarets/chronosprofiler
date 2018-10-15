using System;
using System.Diagnostics;
using System.Reflection;
using Caliburn.Micro;
using Chronos.Client.Win.ViewModels;
using Chronos.Messaging;
using Chronos.Settings;
using Chronos.Win32;

namespace Chronos.Client.Win
{
    internal abstract class ApplicationBase : Client.ApplicationBase, IApplicationBase
    {
        private readonly Bootstrapper _bootstrapper;
        private readonly IContainer _container;
        private readonly Guid _uid;

        protected ApplicationBase(Guid uid)
            : this(uid, true)
        {
        }

        protected ApplicationBase(Guid uid, bool processOwner)
            : base(processOwner)
        {
            _uid = uid;
            _container = new Container();
            _bootstrapper = new Bootstrapper(_container);
            DispatcherHolder.Initialize(new WindowsDispatcher());
        }

        public override Guid Uid
        {
            get { return _uid; }
        }

        protected override string ApplicationCode
        {
            get { return Constants.ApplicationCodeName.WinClient; }
        }

        public PageViewModel MainViewModel { get; private set; }

        public IViewModelManager ViewModelManager { get; private set; }

        public IMessageBus MessageBus { get; private set; }

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
            ConfigureContainer(_container);
            _bootstrapper.Initialize();
            AssemblyResolver.AssemblyLoaded += OnExtensionAssemblyLoaded;
        }

        protected override void OnEndInitialize()
        {
            base.OnEndInitialize();
            ShowMainWindow();
        }

        protected abstract PageViewModel BuildMainViewModel();

        private void ShowMainWindow()
        {
            MainViewModel = BuildMainViewModel();
            ViewModelManager.ShowWindow(MainViewModel);
        }

        protected virtual void ConfigureContainer(IContainer container)
        {
            container.RegisterInstance<IApplicationBase>(this);
            container.RegisterInstance<IApplicationSettings>(ApplicationSettings);
            container.RegisterType<IViewModelManager, ViewModelManager>(true);
            container.RegisterType<IWindowManager, CustomWindowManager>(true);
            container.RegisterInstance(ClientMessageBus.Current);
            ViewModelManager = container.Resolve<IViewModelManager>();
            MessageBus = ClientMessageBus.Current;
        }

        private void OnExtensionAssemblyLoaded(object sender, AssemblyLoadEventArgs e)
        {
            Assembly assembly = e.LoadedAssembly;
            if (assembly.IsWPFAssembly())
            {
                AssemblySource.Instance.Add(e.LoadedAssembly);
            }
        }

        public override void Dispose()
        {
            AssemblyResolver.AssemblyLoaded -= OnExtensionAssemblyLoaded;
            MainViewModel.Dispose();
            Properties.Settings.Default.Save();
            base.Dispose();
        }
    }
}