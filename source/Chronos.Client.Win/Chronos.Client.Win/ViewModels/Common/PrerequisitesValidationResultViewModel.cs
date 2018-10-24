using System.Collections.Generic;
using Adenium;

namespace Chronos.Client.Win.ViewModels.Common
{
    public class PrerequisitesValidationResultViewModel : ViewModel
    {
        public PrerequisitesValidationResultViewModel(IEnumerable<PrerequisiteValidationResult> validationResults)
        {
            ValidationResults = new List<PrerequisiteValidationResult>(validationResults);
        }

        public IEnumerable<PrerequisiteValidationResult> ValidationResults { get; private set; }
    }
}
