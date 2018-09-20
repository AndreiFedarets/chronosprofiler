namespace Chronos.Prerequisites
{
    public sealed class AdministratorPermissionsPrerequisite : IPrerequisiteAdapter
    {
        public PrerequisiteValidationResult Validate()
        {
            string message = string.Empty;
            bool result = SecurityExtensions.HasAdministratorPermissions();
            if (!result)
            {
                message = Properties.Resources.AdministratorPermissionsPrerequisiteErrorMessage;
            }
            return new PrerequisiteValidationResult(result, message);
        }
    }
}
