using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.ServiceProcess;

namespace Chronos.Accessibility.IIS
{
    public sealed class InternetInformationService : RemoteBaseObject, IInternetInformationService
    {
        //https://www.codeproject.com/Articles/18301/Using-Managed-Code-to-Detect-if-IIS-is-Installed-a
        public bool IsAvailable
        {
            get
            {
                try
                {
                    ServiceController service = new ServiceController("World Wide Web Publishing Service");
                    ServiceControllerStatus status = service.Status;
                    return true;
                }
                catch (InvalidOperationException)
                {
                    return false;
                }
            }
        }

        public bool IsRunning
        {
            get
            {
                if (!IsAvailable)
                {
                    return false;
                }
                ServiceController service = new ServiceController("World Wide Web Publishing Service");
                return service.Status.Equals(ServiceControllerStatus.Running);
            }
        }

        public List<string> GetApplicationPools()
        {
            List<string> collection = new List<string>();
            if (!IsAvailable)
            {
                return collection;
            }
            DirectoryEntries appPools = new DirectoryEntry("IIS://localhost/W3SVC/AppPools").Children;
            foreach (DirectoryEntry appPool in appPools)
            {
                collection.Add(appPool.Name);
            }
            return collection;
        }
    }
}
