using System;
using Microsoft.Win32;

namespace Chronos.Prerequisites
{
    public sealed class InternetInformationServiceInstallationPrerequisite : IPrerequisiteAdapter
    {
        public PrerequisiteValidationResult Validate()
        {
            string message = string.Empty;
            bool? result = IsInternetInformationServiceKeyExists();
            if (!result.HasValue)
            {
                message = Properties.Resources.UndefinedInternetInformationServiceErrorMessage;
            }
            if (result.HasValue && !result.Value)
            {
                message = Properties.Resources.MissingInternetInformationServiceErrorMessage;
            }
            return new PrerequisiteValidationResult(result.HasValue && result.Value, message);
        }

        private bool? IsInternetInformationServiceKeyExists()
        {
            try
            {
                string serviceKeyPath = Constants.InternetInformationService.RegisteryPath;
                RegistryKey localMachine = Microsoft.Win32.Registry.LocalMachine;
                RegistryKey key = localMachine.OpenSubKey(serviceKeyPath, true);
                return key != null;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
