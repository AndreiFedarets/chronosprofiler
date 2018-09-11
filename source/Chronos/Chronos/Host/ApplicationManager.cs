using Chronos.Communication;
using Chronos.Communication.Managed;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.Remoting;
using System.ServiceProcess;
using System.Threading;

namespace Chronos.Host
{
    public static class ApplicationManager
    {
        private static IInplaceApplicationManager InplaceApplication;

        private static string CurrentRuntype
        {
            get { return Runtype.Inplace; }
        }

        public static IApplication Run()
        {
            ConnectionSettings connectionSettings = CreateLocalConnectionSettings();
            if (CheckConnection(connectionSettings))
            {
                throw new ApplicationStartupException(ErrorMessageFormatter.HostApplicationIsAlreadyLaunched());
            }
            IApplication application = null;
            switch (CurrentRuntype)
            {
                case Runtype.Inplace:
                    application = RunInplace(false);
                    break;
                case Runtype.Application:
                    application = RunApplication();
                    break;
                case Runtype.Service:
                    application = RunService();
                    break;
                default:
                    application = RunInplace(false);
                    break;
            }
            return application;
        }

        public static IApplication RunInplace(bool processOnwer)
        {
            IApplication application;
            if (processOnwer)
            {
                application = new Application(true);
                application.Run();
            }
            else
            {
                InplaceApplication = new InplaceApplicationManager<Application>();
                InplaceApplication.Run(false);
                application = ConnectLocal();
            }
            return application;
        }

        public static IApplication RunApplication()
        {
            ConnectionSettings connectionSettings = CreateLocalConnectionSettings();
            string fullName = GetApplicationFullName();
            Process process = new Process();
            process.StartInfo = new ProcessStartInfo(fullName);
            process.StartInfo.UseShellExecute = false;
            process.Start();
            WaitForStartup(connectionSettings);
            return Connect(connectionSettings);
        }

        public static IApplication RunService()
        {
            ConnectionSettings connectionSettings = CreateLocalConnectionSettings();
            ServiceController controller = new ServiceController(Constants.HostApplicationServiceName);
            controller.Start();
            WaitForStartup(connectionSettings);
            return Connect(connectionSettings);
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

        public static IApplication ConnectLocal()
        {
            ConnectionSettings connectionSettings = CreateLocalConnectionSettings();
            return Connect(connectionSettings);
        }

        public static IApplication Connect(ConnectionSettings connectionSettings)
        {
            VerifyManagedConnector();
            if (!CheckConnection(connectionSettings))
            {
                throw new ApplicationStartupException(ErrorMessageFormatter.UnableToConnectToHostApplication());
            }
            IApplication application = ConnectInternal(connectionSettings);
            return application;
        }

        public static bool TryConnect(ConnectionSettings connectionSettings, out IApplication application)
        {
            VerifyManagedConnector();
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
            application = new Proxy.Host.Application(application);
            return application;
        }

        public static ConnectionSettings CreateLocalConnectionSettings()
        {
            ChannelSettings channelSettings = new IpcChannelSettings(Application.ApplicationUid);
            ConnectionSettings connectionSettings = new ConnectionSettings(channelSettings, "localhost", Application.ApplicationUid);
            return connectionSettings;
        }

        private static void WaitForStartup(ConnectionSettings connectionSettings)
        {
            // checkPeriod (ms) * iterations = totally wait time (ms)
            // 200 (ms) * 20 = 4000 (ms) = 4 (sec)
            const int checkPeriod = 200;
            const int iterations = 20;
            for (int i = 0; i < iterations; i++)
            {
                if (CheckConnection(connectionSettings))
                {
                    return;
                }
                Thread.Sleep(checkPeriod);
            }
            throw new ApplicationStartupException(ErrorMessageFormatter.UnableToConnectToHostApplication());
        }

        private static void VerifyManagedConnector()
        {
            if (Connector.Managed == null)
            {
                throw new TempException();
            }
        }

        public static void Shutdown()
        {
            if (InplaceApplication != null)
            {
                InplaceApplication.Dispose();
            }
        }

        private static string GetApplicationFullName()
        {
            string fullName = Assembly.GetCallingAssembly().GetAssemblyPath();
            fullName = Path.Combine(fullName, Constants.CoreProcessName.Host);
            return fullName;
        }
    }
}
