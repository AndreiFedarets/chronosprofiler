using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using ChronosBuildTool.Model;

namespace ChronosBuildTool
{
    public class MainViewModel : NotifyPropertyChanged
    {
        private readonly Output _output;
        private readonly SolutionCollection _solutions;
        private readonly Dispatcher _dispatcher;
        private Configuration _currentConfiguration;
        private string _currentPlatformToolset;
        private bool _isConsoleVisible;
        private volatile bool _requestStopOperation;

        public MainViewModel()
        {
            _output = new Output();
            _dispatcher = Dispatcher.CurrentDispatcher;
            _solutions = new SolutionCollection();
            _solutions.LoadFrom(AppDomain.CurrentDomain.BaseDirectory);
            _solutions.Sort();
            BuildCommand = new Command(Build);
            RebuildCommand = new Command(Rebuild);
            CleanCommand = new Command(Clean);
            StopCommand = new Command(Stop);
            StopCommand.Disable();
            ShowHideConsoleCommand = new Command(ShowHideConsole);
            CurrentConfiguration = Configuration.Debug;
            Configurations = new[] {Configuration.Debug, Configuration.Release};
            PlatformToolsets = new[] {"v110", "v120"};
            CurrentPlatformToolset = Solution.GetCurrentPlatformToolset();
            if (string.IsNullOrEmpty(CurrentPlatformToolset))
            {
                CurrentPlatformToolset = PlatformToolsets.First();
            }
            _requestStopOperation = false;
        }

        public IEnumerable<Configuration> Configurations { get; private set; }

        public Configuration CurrentConfiguration
        {
            get { return _currentConfiguration; }
            set
            {
                _currentConfiguration = value;
                OnPropertyChanged(() => CurrentConfiguration);
            }
        }

        public IEnumerable<string> PlatformToolsets { get; private set; }

        public string CurrentPlatformToolset
        {
            get { return _currentPlatformToolset; }
            set
            {
                _currentPlatformToolset = value;
                OnPropertyChanged(() => CurrentPlatformToolset);
            }
        }

        public SolutionCollection Solutions
        {
            get { return _solutions; }
        }

        public bool IsConsoleVisible
        {
            get { return _isConsoleVisible; }
            set
            {
                _isConsoleVisible = value;
                OnPropertyChanged(() => IsConsoleVisible); }
        }

        public Command BuildCommand { get; private set; }
        public Command RebuildCommand { get; private set; }
        public Command CleanCommand { get; private set; }
        public Command StopCommand { get; private set; }
        public Command ShowHideConsoleCommand { get; private set; }

        public IOutput Output
        {
            get { return _output; }
        }

        private void ShowHideConsole(object parameter)
        {
            IsConsoleVisible = !IsConsoleVisible;
        }

        private void Stop(object parameter)
        {
            _requestStopOperation = true;
            StopCommand.Disable();
        }

        private void Clean(object parameter)
        {
            DisableCommands();
            _output.Clear();
            Task task = new Task(CleanInternal);
            task.Start();
        }

        private void CleanInternal()
        {
            try
            {
                foreach (Solution solution in Solutions.GetDependencySortedSolutions())
                {
                    if (_requestStopOperation)
                    {
                        break;
                    }
                    if (solution.IsChecked)
                    {
                        solution.CleanSolution();
                    }
                }
            }
            catch (Exception exception)
            {
                ExceptionReporter.ShowError(exception);
            }
            finally
            {
                Invoke(EnableCommands);
            }
        }

        private void Rebuild(object parameter)
        {
            DisableCommands();
            _output.Clear();
            Task task = new Task(() => BuildInternal(true));
            task.Start();
        }

        private void Build(object parameter)
        {
            DisableCommands();
            _output.Clear();
            Task task = new Task(() => BuildInternal(false));
            task.Start();
        }

        private void DisableCommands()
        {
            BuildCommand.Disable();
            RebuildCommand.Disable();
            CleanCommand.Disable();
            StopCommand.Enable();
        }

        private void EnableCommands()
        {
            _requestStopOperation = false;
            BuildCommand.Enable();
            RebuildCommand.Enable();
            CleanCommand.Enable();
            StopCommand.Disable();
        }

        private void BuildInternal(bool rebuild)
        {
            try
            {
                Invoke(ResetBuildResult);
                foreach (Solution solution in Solutions.GetDependencySortedSolutions())
                {
                    if (_requestStopOperation)
                    {
                        break;
                    }
                    if (solution.IsChecked)
                    {
                        solution.BuildSolution(_output, CurrentConfiguration, CurrentPlatformToolset, rebuild);
                    }
                }
            }
            catch (Exception exception)
            {
                ExceptionReporter.ShowError(exception);
            }
            finally
            {
                Invoke(EnableCommands);
            }
        }

        private void ResetBuildResult()
        {
            foreach (Solution solution in Solutions.GetDependencySortedSolutions())
            {
                if (solution.IsChecked)
                {
                    solution.ResetBuildResult();
                }
            }
        }

        private void Invoke(Action action)
        {
            _dispatcher.Invoke(action);
        }
    }
}
