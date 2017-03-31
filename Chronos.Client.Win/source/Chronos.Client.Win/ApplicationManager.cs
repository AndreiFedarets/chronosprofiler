using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Chronos.Client.Win
{
    public static class ApplicationManager
    {
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
                Action a = () =>
                {
                    AppDomain appDomain = AppDomain.CurrentDomain.Clone(typeof (ProfilingApplication).ToString(),
                        typeof (ApplicationManager).GetAssemblyPath());
                    appDomain.InvokeStaticMember(typeof (ApplicationManager), "Initialize",
                        BindingFlags.InvokeMethod | BindingFlags.Public, null);
                    ChronosApplicationLauncher<ProfilingApplication> activator =
                        RemoteActivator.CreateInstance<ChronosApplicationLauncher<ProfilingApplication>>(appDomain);
                    activator.Run(sessionUid, processOnwer);
                    application = (IApplicationBase) activator.GetApplication();
                };
                System.Windows.Application.Current.Dispatcher.Invoke(a);
            }
            return application;
        }

        public static void RunApplication(Guid sessionUid)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            string fullName = Path.Combine(path, Constants.WinClientApplicationExecutableName);
            Process process = new Process();
            process.StartInfo = new ProcessStartInfo(fullName);
            //TODO: use command line argument instead of env variable
            process.StartInfo.EnvironmentVariables.Add(Chronos.Constants.SessionUidEnvironmentVariableName, sessionUid.ToString());
            process.StartInfo.UseShellExecute = false;
            process.Start();
        }

        public static void Initialize()
        {
            ChronosCoreLocator coreLocator = new ChronosCoreLocator();
            coreLocator.Initialize();
        }
    }
}
