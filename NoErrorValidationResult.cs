using System.Collections.Generic;

namespace MFramework.Common.Validation
{
    class NoErrorValidationResult:IValidateResult
    {
        private readonly IList<IValidateFailure> emptyErrorList = new List<IValidateFailure>();
        public IList<IValidateFailure> Errors
        {
            get { return emptyErrorList; }
        }

        public bool IsValid
        {
            get { return true; }
        }
    }
}
