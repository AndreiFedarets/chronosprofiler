namespace Chronos.Config
{
    public class IpcProtocolElement : RemotingProtocolElement
    {
        public const string ElementName = "ipc";

        public override string Key
        {
            get { return ElementName; }
        }

    }
}
