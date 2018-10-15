using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.Remoting;
using Chronos.Communication;
using Chronos.Communication.Managed;

namespace Chronos.Client.Win
{
    public static class ApplicationManager
    {
        public static void Initialize()
        {
            ChronosCoreLocator coreLocator = new ChronosCoreLocator();
            coreLocator.Initialize();
        }

        public static class Main
        {
            public static Process RunApplication()
            {
                string path = Assembly.GetCallingAssembly().GetAssemblyPath();
                string fullName = Path.Combine(path, Constants.CoreExecutableName.Client);
                Process process = new Process();
                process.StartInfo = new ProcessStartInfo(fullName);
                //TODO: use command line argument instead of env variable
                process.StartInfo.UseShellExecute = false;
                process.Start();
                return process;
            }

            public static IApplicationBase RunInplace(bool processOnwer)
            {
                IApplicationBase application;
                if (processOnwer)
                {
                    application = new MainApplication(processOnwer);
                    application.Run();
                }
                else
                {
                    AppDomain appDomain = AppDomain.CurrentDomain.Clone(typeof(MainApplication).ToString(), typeof(ApplicationManager).GetAssemblyPath());
                    ChronosApplicationLauncher<MainApplication> activator = RemoteActivator.CreateInstance<ChronosApplicationLauncher<MainApplication>>(appDomain);
                    activator.Run(processOnwer);
                    application = (IApplicationBase)activator.GetApplication();
                }
                return application;
            }

        }

        public static class Profiling
        {
            public static Process RunApplication(Guid sessionUid)
            {
                string path = Assembly.GetCallingAssembly().GetAssemblyPath();
                string fullName = Path.Combine(path, Constants.CoreExecutableName.Client);
                Process process = new Process();
                process.StartInfo = new ProcessStartInfo(fullName);
                //TODO: use command line argument instead of env variable
                process.StartInfo.EnvironmentVariables[Chronos.Constants.SessionUidEnvironmentVariableName] = sessionUid.ToString();
                process.StartInfo.UseShellExecute = false;
                process.Start();
                return process;
            }

            public static void RunOrActivateApplication(Guid sessionUid)
            {
                Guid applicationUid = ProfilingApplication.GenerateApplicationUid(sessionUid);
                ConnectionSettings connectionSettings = CreateLocalConnectionSettings(applicationUid);
                if (CheckConnection(connectionSettings))
                {
                    ProfilingApplication application = Connect(connectionSettings);
                    application.Activate();
                }
                else
                {
                    RunApplication(sessionUid);
                }
            }

            public static IApplicationBase RunInplace(Guid sessionUid, bool processOnwer)
            {
                IApplicationBase application = null;
                if (processOnwer)
                {
                    application = new ProfilingApplication(sessionUid, processOnwer);
                    application.Run();
                }
                else
                {
                    AppDomain appDomain = AppDomain.CurrentDomain.Clone(typeof(ProfilingApplication).ToString(), typeof(ApplicationManager).GetAssemblyPath());
                    appDomain.InvokeStaticMember(typeof(ApplicationManager), "Initialize", BindingFlags.InvokeMethod | BindingFlags.Public, null);
                    ChronosApplicationLauncher<ProfilingApplication> activator = RemoteActivator.CreateInstance<ChronosApplicationLauncher<ProfilingApplication>>(appDomain);
                    activator.Run(sessionUid, processOnwer);
                    application = (IApplicationBase)activator.GetApplication();
                }
                return application;
            }

            private static ProfilingApplication Connect(ConnectionSettings connectionSettings)
            {
                if (!CheckConnection(connectionSettings))
                {
                    //TODO: add connectionsettings to exception message
                    throw new ApplicationStartupException("Unable to connect to Profiling Application");
                }
                ProfilingApplication application = ConnectInternal(connectionSettings);
                return application;
            }

            private static ProfilingApplication ConnectInternal(ConnectionSettings connectionSettings)
            {
                ProfilingApplication application = Connector.Managed.Connect<ProfilingApplication>(connectionSettings);
                return application;
            }

            private static ConnectionSettings CreateLocalConnectionSettings(Guid applicationUid)
            {
                ChannelSettings channelSettings = new IpcChannelSettings(applicationUid);
                ConnectionSettings connectionSettings = new ConnectionSettings(channelSettings, "localhost", applicationUid);
                return connectionSettings;
            }

            private static bool CheckConnection(ConnectionSettings connectionSettings)
            {
                try
                {
                    const string message = "test";
                    ProfilingApplication application = Connector.Managed.Connect<ProfilingApplication>(connectionSettings);
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

        }
    }
}
