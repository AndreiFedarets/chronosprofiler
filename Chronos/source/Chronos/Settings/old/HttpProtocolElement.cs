namespace Chronos.Config
{
    public class HttpProtocolElement : RemotingProtocolElement
    {
        public const string ElementName = "http";

        public override string Key
        {
            get { return ElementName; }
        }

    }
}
