using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Remoting;
using System.Threading;
using Chronos.Communication;
using Chronos.Communication.Managed;
using System.Collections.Generic;

namespace Chronos.Daemon
{
    public static class ApplicationManager
    {
        private static readonly List<IInplaceApplicationManager> InplaceApplications;

        static ApplicationManager()
        {
            InplaceApplications = new List<IInplaceApplicationManager>();
            //AppDomain.CurrentDomain.ProcessExit += OnProcessExit;
        }

        //private static void OnProcessExit(object sender, EventArgs e)
        //{
        //    AppDomain.CurrentDomain.ProcessExit -= OnProcessExit;
        //    foreach (IInplaceApplicationManager manager in InplaceApplications)
        //    {
        //        manager.Dispose();
        //    }
        //    InplaceApplications.Clear();
        //}

        private static string CurrentRuntype
        {
            get { return Runtype.Application; }
        }

        public static void Run(Guid sessionUid)
        {
            ConnectionSettings connectionSettings = CreateLocalConnectionSettings(sessionUid);
            if (CheckConnection(connectionSettings))
            {
                throw new ApplicationStartupException("Daemon Application is already launched");
            }
            switch (CurrentRuntype)
            {
                case Runtype.Inplace:
                    RunInplace(false, sessionUid);
                    break;
                case Runtype.Application:
                    RunApplication(connectionSettings, sessionUid);
                    break;
                default:
                    RunInplace(false, sessionUid);
                    break;
            }
        }

        public static IApplication RunInplace(bool processOnwer, Guid sessionUid)
        {
            IApplication application;
            if (processOnwer)
            {
                application = new Application(true, sessionUid);
                application.Run();
            }
            else
            {
                AppDomain appDomain = AppDomain.CurrentDomain.Clone(typeof(Application).ToString(), typeof(ApplicationManager).GetAssemblyPath());
                ChronosApplicationLauncher<Application> activator = RemoteActivator.CreateInstance<ChronosApplicationLauncher<Application>>(appDomain);
                activator.Run(false, sessionUid);
                application = ConnectLocal(sessionUid);
            }
            return application;
        }

        public static void RunApplication(ConnectionSettings connectionSettings, Guid sessionUid)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            string fullName = Path.Combine(path, Constants.DaemonApplicationExecutableName);
            Process process = new Process();
            process.StartInfo = new ProcessStartInfo(fullName);
            //TODO: use argument instead of env variable
            process.StartInfo.EnvironmentVariables.Add(Constants.SessionUidEnvironmentVariableName, sessionUid.ToString());
            process.StartInfo.UseShellExecute = false;
            process.Start();
            WaitForStartup(connectionSettings);
        }

        public static bool CheckConnection(ConnectionSettings connectionSettings)
        {
            try
            {
                const string message = "test";
                IApplication application = Connector.Managed.Connect<Application>(connectionSettings);
                string ping = application.Ping(message);
                return string.Equals(ping, message, StringComparison.InvariantCultureIgnoreCase);
            }
            catch (RemotingException)
            {
            }
            catch (Exception exception)
            {
                LoggingProvider.Current.Log(TraceEventType.Error, exception);
            }
            return false;
        }

        public static IApplication ConnectLocal(Guid sessionUid)
        {
            ConnectionSettings connectionSettings = CreateLocalConnectionSettings(sessionUid);
            return Connect(connectionSettings);
        }

        public static IApplication Connect(ConnectionSettings connectionSettings)
        {
            if (!CheckConnection(connectionSettings))
            {
                //TODO: add connectionsettings to exception message
                throw new ApplicationStartupException("Unable to connect to Host Application");
            }
            IApplication application = ConnectInternal(connectionSettings);
            return application;
        }

        public static bool TryConnect(ConnectionSettings connectionSettings, out IApplication application)
        {
            try
            {
                const string message = "test";
                application = ConnectInternal(connectionSettings);
                return string.Equals(application.Ping(message), message, StringComparison.InvariantCultureIgnoreCase);
            }
            catch (RemotingException)
            {
            }
            catch (Exception exception)
            {
                LoggingProvider.Current.Log(TraceEventType.Error, exception);
            }
            application = null;
            return false;
        }

        private static IApplication ConnectInternal(ConnectionSettings connectionSettings)
        {
            IApplication application = Connector.Managed.Connect<Application>(connectionSettings);
            application = new Proxy.Daemon.Application(application);
            return application;
        }

        internal static ConnectionSettings CreateLocalConnectionSettings(Guid sessionUid)
        {
            ChannelSettings channelSettings = new IpcChannelSettings(sessionUid);
            ConnectionSettings connectionSettings = new ConnectionSettings(channelSettings, "localhost", sessionUid);
            return connectionSettings;
        }

        private static void WaitForStartup(ConnectionSettings connectionSettings)
        {
            const int checkPeriod = 100;
            const int iterations = 50;
            for (int i = 0; i < iterations; i++)
            {
                if (CheckConnection(connectionSettings))
                {
                    return;
                }
                Thread.Sleep(checkPeriod);
            }
            throw new ApplicationStartupException("Unable to connect to Daemon Application");
        }

        public static void Shutdown()
        {
            foreach (IInplaceApplicationManager manager in InplaceApplications)
            {
                manager.Dispose();
            }
            InplaceApplications.Clear();
        }
    }
}
