using System.Configuration;

namespace Chronos.Config
{
    public class DaemonSection : ConfigurationSection
    {
        private const string AutoClosePropertyName = "autoClose";

        [ConfigurationProperty(AutoClosePropertyName, IsRequired = true)]
        public AutoCloseElement AutoClose
        {
            get { return (AutoCloseElement)this[AutoClosePropertyName]; }
            set { this[AutoClosePropertyName] = value; }
        }
    }
}
