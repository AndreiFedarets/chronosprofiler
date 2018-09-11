namespace Chronos.Config
{
    public class TcpProtocolElement : RemotingProtocolElement
    {
        public const string ElementName = "tcp";

        public override string Key
        {
            get { return ElementName; }
        }
    }
}
