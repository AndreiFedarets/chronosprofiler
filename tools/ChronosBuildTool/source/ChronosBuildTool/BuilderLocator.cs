using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;

namespace ChronosBuildTool
{
    internal static class BuilderLocator
    {
        public static List<string> GetAvailableBuilders()
        {
            List<string> collection = new List<string>();
            collection.Add(GetLatestBuilder());
            DirectoryInfo programFiles = new DirectoryInfo(Environment.ExpandEnvironmentVariables("%ProgramFiles%\\Microsoft Visual Studio\\2017"));
            if (programFiles.Exists)
            {
                FileInfo[] msbuilds = programFiles.GetFiles("MSBuild.exe", SearchOption.AllDirectories);
                foreach (FileInfo msbuild in msbuilds)
                {
                    if (!collection.Contains(msbuild.FullName))
                    {
                        collection.Add(msbuild.FullName);
                    }
                }
            }
            return collection;
        }

        public static string GetLatestBuilder()
        {
            RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\MSBuild\ToolsVersions");
            string latestVersionPath = ""; //default
            Version latestVersion = new Version();
            foreach (string subKeyName in key.GetSubKeyNames())
            {
                Version version;
                if (Version.TryParse(subKeyName, out version) && version > latestVersion)
                {
                    RegistryKey tempkey = key.OpenSubKey(subKeyName);
                    string path = (string)tempkey.GetValue("MSBuildToolsPath");
                    if (!string.IsNullOrEmpty(path))
                    {
                        latestVersion = version;
                        latestVersionPath = Path.Combine(path, "msbuild.exe");
                    }
                }
            }
            return latestVersionPath;
        }
    }
}
