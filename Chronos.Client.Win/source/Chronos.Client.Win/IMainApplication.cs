namespace Chronos.Client.Win
{
    public interface IMainApplication : IApplicationBase
    {
        Host.IApplicationCollection HostApplications { get; }

        ISessionCollection Sessions { get; }
    }
}
