using Chronos.Settings;
using System;
using System.Collections.Generic;

namespace Chronos.Host
{
    public class ConnectionManager : IConnectionManager
    {
        public void RestoreConnections(IApplicationCollection applications, IConnectionSettingsCollection settings)
        {
            if (settings.RunLocal)
            {
                applications.ConnectLocal(true);
            }
            List<Exception> exceptions = null;
            foreach (IConnectionSettings hostSettings in settings)
            {
                Communication.Managed.ConnectionSettings connectionSettings = hostSettings.GetConnectionSettings();
                try
                {
                    applications.Connect(connectionSettings);
                }
                catch (Exception exception)
                {
                    if (exceptions == null)
                    {
                        exceptions = new List<Exception>();
                    }
                    exceptions.Add(exception);
                }
            }
            if (exceptions != null)
            {
                throw new AggregateException(exceptions);
            }
        }

        public void SaveConnections(IApplicationCollection applications, IConnectionSettingsCollection settings)
        {

        }

    }
}
