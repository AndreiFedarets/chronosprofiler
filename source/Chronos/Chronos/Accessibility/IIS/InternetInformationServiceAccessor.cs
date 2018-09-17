using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.ServiceProcess;
using Chronos.Accessibility.WS;
using Microsoft.Win32;
using ServiceController = System.ServiceProcess.ServiceController;

namespace Chronos.Accessibility.IIS
{
    //https://www.codeproject.com/Articles/18301/Using-Managed-Code-to-Detect-if-IIS-is-Installed-a
    public sealed class InternetInformationServiceAccessor : RemoteBaseObject, IInternetInformationServiceAccessor
    {
        private readonly IInternetInformationService _internetInformationService;

        public InternetInformationServiceAccessor(IInternetInformationService internetInformationService)
        {
            _internetInformationService = internetInformationService;
        }

        public bool IsAvailable
        {
            get { return _internetInformationService.IsAvailable; }
        }

        public List<string> GetApplicationPools()
        {
            return _internetInformationService.GetApplicationPools();
        }
    }
}
