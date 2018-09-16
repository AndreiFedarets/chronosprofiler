using System;
using System.Management;
using System.Text;

namespace Chronos.Win32
{
    public static class SystemManagement
    {
        public static string GetProcessCommandLine(int processId)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                using (var searcher = new ManagementObjectSearcher("SELECT CommandLine FROM Win32_Process WHERE ProcessId=" + processId))
                {
                    bool firstIteration = true;
                    foreach (var @object in searcher.Get())
                    {
                        if (firstIteration)
                        {
                            firstIteration = false;
                        }
                        else
                        {
                            builder.Append(" ");
                        }
                        builder.Append(@object["CommandLine"]);
                    }
                }
                return builder.ToString();
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}
