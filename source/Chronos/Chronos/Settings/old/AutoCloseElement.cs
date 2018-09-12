using System.Configuration;

namespace Chronos.Config
{
    public class AutoCloseElement : ConfigurationElement
    {
        private const string TimeoutPropertyName = "timeout";
        private const string EnabledPropertyName = "enabled";
        public static readonly AutoCloseElement Default;

        static AutoCloseElement()
        {
            Default = new AutoCloseElement();
        }

        [ConfigurationProperty(TimeoutPropertyName, IsRequired = false, DefaultValue = double.MaxValue)]
        public double Timeout
        {
            get { return (double)this[TimeoutPropertyName]; }
            set { this[TimeoutPropertyName] = value; }
        }

        [ConfigurationProperty(EnabledPropertyName, IsRequired = false, DefaultValue = false)]
        public bool Enabled
        {
            get { return (bool)this[EnabledPropertyName]; }
            set { this[EnabledPropertyName] = value; }
        }
    }
}
