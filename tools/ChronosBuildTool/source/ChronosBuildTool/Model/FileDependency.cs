
namespace ChronosBuildTool.Model
{
    public class FileDependency
    {
        public FileDependency(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
    }
}
