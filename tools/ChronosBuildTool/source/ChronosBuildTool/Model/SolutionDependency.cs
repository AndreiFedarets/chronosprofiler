using System.Collections.Generic;

namespace ChronosBuildTool.Model
{
    public class SolutionDependency
    {
        private readonly List<FileDependency> _files;

        public SolutionDependency(string name, List<FileDependency> files)
        {
            SolutionName = name;
            _files = files;
        }

        public string SolutionName { get; private set; }

        public IEnumerable<FileDependency> Files
        {
            get { return _files; }
        }
    }
}
