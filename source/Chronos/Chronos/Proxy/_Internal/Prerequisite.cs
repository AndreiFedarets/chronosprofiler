namespace Chronos.Proxy
{
    internal sealed class Prerequisite : ProxyBaseObject<IPrerequisite>, IPrerequisite
    {
        public Prerequisite(IPrerequisite remoteObject)
            : base(remoteObject)
        {
        }

        public PrerequisiteValidationResult Validate()
        {
            return Execute(() => RemoteObject.Validate());
        }
    }
}
