using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ChronosBuildTool.Model
{
    public class SolutionCollection : NotifyPropertyChanged, IEnumerable<Solution>
    {
        private readonly List<Solution> _solutions;

        public SolutionCollection()
        {
            _solutions = new List<Solution>();
        }

        public Solution GetByChronosSolutioName(string name)
        {
            return _solutions.FirstOrDefault(x => string.Equals(x.ChronosSolutionName, name, StringComparison.InvariantCultureIgnoreCase));
        }

        public bool IsChecked
        {
            get { return _solutions.All(x => x.IsChecked); }
            set
            {
                foreach (Solution solution in _solutions)
                {
                    solution.IsChecked = value;
                }
                OnPropertyChanged(() => IsChecked);
            }
        }

        public void LoadFrom(string path)
        {
            DirectoryInfo root = new DirectoryInfo(path);
            List<FileInfo> solutionFiles = new List<FileInfo>();
            foreach (DirectoryInfo solutionDirectory in root.GetDirectories())
            {
                FileInfo[] localSolutionFiles = solutionDirectory.GetFiles("*.chrsln", SearchOption.TopDirectoryOnly);
                solutionFiles.AddRange(localSolutionFiles);
            }
            SolutionXmlReader solutionXmlReader = new SolutionXmlReader();
            foreach (FileInfo solutionFile in solutionFiles)
            {
                Solution solution = solutionXmlReader.ReadSolution(this, solutionFile);
                _solutions.Add(solution);
            }
        }

        public void Sort()
        {
            List<Solution> sortedSolutions = new List<Solution>();
            while (_solutions.Count > 0)
            {
                foreach (Solution solution in _solutions)
                {
                    List<SolutionDependency> dependencies = solution.Dependencies.ToList();
                    if (dependencies.All(x => sortedSolutions.Any(y => string.Equals(x.SolutionName, y.ChronosSolutionName))))
                    {
                        sortedSolutions.Add(solution);
                        _solutions.Remove(solution);
                        break;
                    }
                }
            }
            _solutions.AddRange(sortedSolutions);
        }

        public IEnumerator<Solution> GetEnumerator()
        {
            return _solutions.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
