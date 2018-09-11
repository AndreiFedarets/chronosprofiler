namespace Chronos.Host
{
    public sealed class Settings : ISettings
    {
        public string Runtype
        {
            get { return Host.Runtype.Inplace; }
        }
    }
}
