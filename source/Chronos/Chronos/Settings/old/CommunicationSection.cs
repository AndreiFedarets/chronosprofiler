using System.Configuration;

namespace Chronos.Config
{
    public class CommunicationSection : ConfigurationSection
    {
        private const string RemotingPropertyName = "remoting";
        //private const string LocalPropertyName = "local";

        [ConfigurationProperty(RemotingPropertyName, IsRequired = true)]
        public RemotingCommunicationElement Remoting
        {
            get { return (RemotingCommunicationElement)this[RemotingPropertyName]; }
            set { this[RemotingPropertyName] = value; }
        }

        //[ConfigurationProperty(LocalPropertyName, IsRequired = true)]
        //public RemotingCommunicationElement Local
        //{
        //    get { return (RemotingCommunicationElement)this[LocalPropertyName]; }
        //    set { this[LocalPropertyName] = value; }
        //}
    }
}
