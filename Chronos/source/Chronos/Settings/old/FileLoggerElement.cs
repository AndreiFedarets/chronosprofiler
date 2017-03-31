using System.Configuration;

namespace Chronos.Config
{
    public class FileLoggerElement : LoggerElement
    {
        private const string PathProperty = "path";

        [ConfigurationProperty(PathProperty, IsRequired = true)]
        public string Path
        {
            get { return (string)this[PathProperty]; }
            set { this[PathProperty] = value; }
        }
    }
}
