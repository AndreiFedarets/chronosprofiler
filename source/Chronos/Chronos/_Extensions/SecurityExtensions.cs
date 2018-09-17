using System.Security.Principal;

namespace Chronos
{
    public static class SecurityExtensions
    {
        public static bool HasAdministratorPermissions()
        {
            using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
            {
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                bool isElevated = principal.IsInRole(WindowsBuiltInRole.Administrator);
                return isElevated;
            }
        }
    }
}
