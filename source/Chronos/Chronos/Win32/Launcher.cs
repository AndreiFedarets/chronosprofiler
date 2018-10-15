using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Collections.Specialized;
using System.Text;

namespace Chronos.Win32
{
    public static class Launcher
    {
        public static Process Launch(int requiredSessionId, string fileName, string args, string workingDirectory, StringDictionary environmentVariables)
        {
            if (requiredSessionId > 0)
            {
                int currentSessionId = Kernel32.WTSGetActiveConsoleSessionId();
                if (currentSessionId != requiredSessionId)
                {
                    return LaunchInSpecialSession(requiredSessionId, fileName, args, workingDirectory, environmentVariables);
                }
            }
            return LaunchInCurrentSession(fileName, args, workingDirectory, environmentVariables);
        }

        public static Process LaunchInCurrentSession(string fileName, string args, string workingDirectory, StringDictionary environmentVariables)
        {
            Process process = new Process();
            ProcessStartInfo processStartInfo = new ProcessStartInfo(fileName);
            foreach (DictionaryEntry variable in environmentVariables)
            {
                string key = variable.Key.ToString();
                string value = variable.Value.ToString();
                processStartInfo.EnvironmentVariables[key] = value;
            }
            processStartInfo.UseShellExecute = false;
            processStartInfo.Arguments = args;
            processStartInfo.WorkingDirectory = workingDirectory;
            process.StartInfo = processStartInfo;
            process.Start();
            return process;
        }

        public static Process LaunchInSpecialSession(int sessionId, string fileName, string args, string workingDirectory, StringDictionary environmentVariables)
        {
            Process process;
            IntPtr userToken = IntPtr.Zero;
            IntPtr originalEnvironment = IntPtr.Zero;
            IntPtr customEnvironment = IntPtr.Zero;
            try
            {
                if (!NativeMethods.WTSQueryUserToken(sessionId, ref userToken))
                {
                    int error = Marshal.GetLastWin32Error();
                    throw new Win32Exception(error);
                }
                if (!NativeMethods.CreateEnvironmentBlock(ref originalEnvironment, userToken, false))
                {
                    int error = Marshal.GetLastWin32Error();
                    throw new Win32Exception(error);
                }
                UInt32 creationFlags = 0;
                if (originalEnvironment != IntPtr.Zero)
                {
                    creationFlags |= 0x400;
                }
                StringDictionary originalEnvironmentVariables = NativeMethods.ReadEnvironmentVariables(originalEnvironment);
                // Append custom environment variables to list.
                foreach (DictionaryEntry variable in environmentVariables)
                {
                    originalEnvironmentVariables.Add(variable.Key.ToString(), variable.Value.ToString());
                }
                StringBuilder newEnvironmentBlock = new StringBuilder();
                foreach (DictionaryEntry variable in originalEnvironmentVariables)
                {
                    newEnvironmentBlock.AppendFormat("{0}={1}\0", variable.Key, variable.Value);
                }
                newEnvironmentBlock.Append("\0");

                customEnvironment = Marshal.StringToHGlobalUni(newEnvironmentBlock.ToString());

                NativeMethods.StartupInfo startupInfo = new NativeMethods.StartupInfo();
                startupInfo.Size = Marshal.SizeOf(startupInfo);


                NativeMethods.ProcessInformation processInformation;
                if (Advapi32.CreateProcessAsUser(userToken, fileName, args, IntPtr.Zero, IntPtr.Zero, false, creationFlags, 
                    customEnvironment, workingDirectory, ref startupInfo, out processInformation))
                {
                    process = Process.GetProcessById((int) processInformation.ProcessId);
                    Kernel32.CloseHandle(processInformation.Process);
                    Kernel32.CloseHandle(processInformation.Thread);
                }
                else
                {
                    int error = Marshal.GetLastWin32Error();
                    throw new Win32Exception(error);
                }
            }
            finally
            {
                if (userToken != IntPtr.Zero)
                {
                    Kernel32.CloseHandle(userToken);
                }
                if (originalEnvironment != IntPtr.Zero)
                {
                    NativeMethods.DestroyEnvironmentBlock(originalEnvironment);
                }
                if (customEnvironment != IntPtr.Zero)
                {
                    NativeMethods.DestroyEnvironmentBlock(customEnvironment);
                }
            }
            return process;
        }
    }
}
