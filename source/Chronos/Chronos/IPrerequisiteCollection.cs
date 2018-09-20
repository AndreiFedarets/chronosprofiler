using System.Collections.Generic;

namespace Chronos
{
    public interface IPrerequisiteCollection : IEnumerable<IPrerequisite>
    {
        List<PrerequisiteValidationResult> Validate(bool failedOnly);
    }
}
