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
            _solutions.Sort((x, y) => string.Compare(x.ChronosSolutionName, y.ChronosSolutionName));
        }

        public IList<Solution> GetDependencySortedSolutions()
        {
            List<Solution> sortedSolutions = new List<Solution>();
            List<Solution> originalSolutions = new List<Solution>(_solutions);
            while (originalSolutions.Count > 0)
            {
                foreach (Solution solution in originalSolutions)
                {
                    List<SolutionDependency> dependencies = solution.Dependencies.ToList();
                    if (dependencies.All(x => sortedSolutions.Any(y => string.Equals(x.SolutionName, y.ChronosSolutionName))))
                    {
                        sortedSolutions.Add(solution);
                        originalSolutions.Remove(solution);
                        break;
                    }
                }
            }
            return sortedSolutions;
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
