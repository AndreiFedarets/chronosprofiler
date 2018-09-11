using System;

namespace Chronos
{
    internal static class ErrorMessageFormatter
    {
        public static string SessionAlreadyRemovedFromCollection(Guid sessionUid)
        {
            return string.Format("Session '{0}' already removed from the collection", sessionUid);
        }

        public static string HostApplicationIsAlreadyLaunched()
        {
            return "Host Application is already launched";
        }

        public static string UnableToConnectToHostApplication()
        {
            return "Unable to connect to Host Application";
        }
    }
}
