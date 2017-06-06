using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace ChronosBuildTool.Model
{
    public sealed class Solution : NotifyPropertyChanged
    {
        private readonly ExternalsFolder _externalsFolder;
        private readonly List<SolutionDependency> _dependencies;
        private readonly SolutionCollection _solutions;
        private readonly BuildFolder _buildFolder;
        private readonly PackageFolder _packageFolder;
        private readonly FileInfo _solutionFile;
        private readonly string _sourceFolder;
        private BuildResult _buildResult;
        private bool _isChecked;

        internal Solution(SolutionCollection solutions, FileInfo solutionFile, string name, string sourceFolder, BuildFolder buildFolder, 
            PackageFolder packageFolder, ExternalsFolder externalsFolder, List<SolutionDependency> dependencies)
        {
            _solutions = solutions;
            _solutionFile = solutionFile;
            _buildFolder = buildFolder;
            _packageFolder = packageFolder;
            _sourceFolder = sourceFolder;
            _externalsFolder = externalsFolder;
            _dependencies = dependencies;
            SolutionName = name;
        }

        public string ChronosSolutionName
        {
            get { return _solutionFile.Name; }
        }

        public string ChronosSolutionPath
        {
            get { return _solutionFile.Directory.FullName; }
        }
        
        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                _isChecked = value;
                OnPropertyChanged(() => IsChecked);
            }
        }

        public BuildResult BuildResult
        {
            get { return _buildResult; }
            private set
            {
                _buildResult = value;
                OnPropertyChanged(() => BuildResult);
            }
        }

        public string SolutionName { get; private set; }

        private string SolutionPath
        {
            get
            {
                if (_solutionFile.DirectoryName == null)
                {
                    return SolutionName;
                }
                string path = Path.Combine(_solutionFile.DirectoryName, _sourceFolder);
                return path;
            }
        }
        private string SolutionFullName
        {
            get
            {
                string fullName = Path.Combine(SolutionPath, SolutionName);
                return fullName;
            }
        }

        public IEnumerable<SolutionDependency> Dependencies
        {
            get { return _dependencies; }
        }

        public bool BuildSolution(IOutput output, Configuration configuration, bool rebuild)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            if (!UpdateExternals(output, configuration, false))
            {
                return false;
            }
            BuildResult buildResult = new BuildResult();
            Process process = new Process();
            process.StartInfo = new ProcessStartInfo(GetMsbuildPath());
            string targetAction = rebuild ? "Rebuild" : "Build";
            process.StartInfo.Arguments = string.Format("{0} /t:{1} /p:Configuration={2} /m", SolutionFullName, targetAction, configuration);
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardOutput = true;
            DataReceivedEventHandler eventHandler = (s, e) => { output.WriteLine(e.Data); buildResult.ProcessMessage(e.Data); };
            try
            {
                process.OutputDataReceived += eventHandler;
                process.Start();
                process.BeginOutputReadLine();
                process.WaitForExit();
            }
            finally
            {
                process.OutputDataReceived -= eventHandler;
                buildResult.SetTime(stopwatch.Elapsed);
                BuildResult = buildResult;
            }
            return process.ExitCode == 0;
        }

        public void CleanSolution()
        {
            DirectoryInfo solutionDirectory = new DirectoryInfo(SolutionPath);
            foreach (DirectoryInfo projectDirectory in solutionDirectory.GetDirectories())
            {
                CleanProject(projectDirectory);
            }
            DeleteDirectory(solutionDirectory, "ipch");
            DeleteDirectory(solutionDirectory, "_ReSharper.*");
            DeleteFile(solutionDirectory, "*.ReSharper.user");
            DeleteFile(solutionDirectory, "*.sln.DotSettings.user");
            DeleteFile(solutionDirectory, "*.suo");
            DeleteFile(solutionDirectory, "*.sdf");

            DirectoryInfo buildDebugDirectory = new DirectoryInfo(Path.Combine(ChronosSolutionPath, _buildFolder[Configuration.Debug]));
            if (buildDebugDirectory.Exists)
            {
                buildDebugDirectory.Delete(true);
            }
            DirectoryInfo buildReleaseDirectory = new DirectoryInfo(Path.Combine(ChronosSolutionPath, _buildFolder[Configuration.Release]));
            if (buildReleaseDirectory.Exists)
            {
                buildReleaseDirectory.Delete(true);
            }
        }

        private void CleanProject(DirectoryInfo projectDirectory)
        {
            DeleteDirectory(projectDirectory, "bin");
            DeleteDirectory(projectDirectory, "obj");
            DeleteFile(projectDirectory, "*.csproj.user");
            DeleteFile(projectDirectory, "*.csproj.DotSettings.user");
            DeleteFile(projectDirectory, "*.vcxproj.DotSettings.user");
        }

        private void DeleteDirectory(DirectoryInfo parentDirectory, string name)
        {
            DirectoryInfo[] directories = parentDirectory.GetDirectories(name);
            foreach (DirectoryInfo directory in directories)
            {
                try
                {
                    directory.Delete(true);
                }
                catch (Exception)
                {
                }
            }
        }

        private void DeleteFile(DirectoryInfo parentDirectory, string name)
        {
            FileInfo[] files = parentDirectory.GetFiles(name);
            foreach (FileInfo file in files)
            {
                try
                {
                    file.Delete();
                }
                catch (Exception)
                {
                }
            }
        }

        public bool UpdateExternals(IOutput output, Configuration configuration, bool force)
        {
            foreach (SolutionDependency solutionDependency in _dependencies)
            {
                Solution solution = _solutions.GetByChronosSolutioName(solutionDependency.SolutionName);
                foreach (FileDependency fileDependency in solutionDependency.Files)
                {
                    FileInfo sourceFile = solution.GetBuildFile(fileDependency.Name, configuration);
                    FileInfo targetFile = GetExternalsFile(fileDependency.Name, configuration);
                    if ((!targetFile.Exists && !sourceFile.Exists) || force)
                    {
                        if (!solution.BuildSolution(output, configuration, force))
                        {
                            return false;
                        }
                        return UpdateExternals(output, configuration, false);
                    }
                    //if target file doesn't exist or it's last write less that source file write time
                    //then we should copy source to target
                    if (!targetFile.Exists || sourceFile.LastWriteTimeUtc > targetFile.LastWriteTimeUtc)
                    {
                        if (!targetFile.Directory.Exists)
                        {
                            targetFile.Directory.Create();
                        }
                        sourceFile.CopyTo(targetFile.FullName, true);
                    }
                }
            }
            return true;
        }

        private FileInfo GetExternalsFile(string fileName, Configuration configuration)
        {
            string externalsPath = _externalsFolder[configuration];
            externalsPath = Path.Combine(ChronosSolutionPath, externalsPath);
            string fullName = Path.Combine(externalsPath, fileName);
            return new FileInfo(fullName);
        }

        private FileInfo GetBuildFile(string fileName, Configuration configuration)
        {
            string buildPath = _buildFolder[configuration];
            buildPath = Path.Combine(ChronosSolutionPath, buildPath);
            string fullName = Path.Combine(buildPath, fileName);
            if (!File.Exists(fullName))
            {
                buildPath = _packageFolder[configuration];
                buildPath = Path.Combine(ChronosSolutionPath, buildPath);
                fullName = Path.Combine(buildPath, fileName);
            }
            return new FileInfo(fullName);
        }

        private string GetMsbuildPath()
        {
            //RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\MSBuild\ToolsVersions");
            //string latestVersionString = ""; //default
            //double latestVersion = 0;
            //foreach (string subKeyName in key.GetSubKeyNames())
            //{
            //    double version;
            //    if (double.TryParse(subKeyName, out version) && version > latestVersion)
            //    {
            //        RegistryKey tempkey = key.OpenSubKey(subKeyName);
            //        if (tempkey.GetValue("MSBuildOverrideTasksPath") != null)
            //        {
            //            latestVersion = version;
            //            latestVersionString = subKeyName;
            //        }
            //    }
            //}
           
            //string msbuildPath = key.GetValue("MSBuildOverrideTasksPath") + @"\msbuild.exe";
            string msbuildPath = Environment.ExpandEnvironmentVariables(@"%ProgramFiles(x86)%\MSBuild\12.0\Bin\msbuild.exe");
            return msbuildPath;
        }

        public void ResetBuildResult()
        {
            BuildResult = null;
        }
    }
}
