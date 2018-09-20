using System;

namespace Chronos.Prerequisites
{
    [Serializable]
    public sealed class PrerequisiteValidationResult
    {
        public PrerequisiteValidationResult(bool result, string message)
        {
            Result = result;
            Message = message;
        }

        public bool Result { get; private set; }

        public string Message { get; private set; }
    }
}
