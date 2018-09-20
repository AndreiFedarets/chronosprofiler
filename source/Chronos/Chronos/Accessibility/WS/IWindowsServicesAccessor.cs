namespace Chronos.Accessibility.WS
{
    [PublicService(typeof(Proxy.Accessibility.WS.WindowsServicesAccessor))]
    public interface IWindowsServicesAccessor
    {
        bool HasPermissions { get; }

        WindowsServiceInfo[] GetServices();
    }
}
