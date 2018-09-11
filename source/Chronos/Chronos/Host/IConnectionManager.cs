using Chronos.Settings;

namespace Chronos.Host
{
    public interface IConnectionManager
    {
        void RestoreConnections(IApplicationCollection applications, IConnectionSettingsCollection settings);

        void SaveConnections(IApplicationCollection applications, IConnectionSettingsCollection settings);
    }
}
