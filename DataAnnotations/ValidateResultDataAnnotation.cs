using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MFramework.Common.Validation.DataAnnotations
{
    class ValidateResultDataAnnotation : IValidateResult
    {
        #region member variable

        private ValidationResult _result;
        private bool _success;
        #endregion
        #region Constructor
        internal ValidateResultDataAnnotation(bool success, ValidationResult result)
        {
            _result = result;
            _success = success;
        }
        #endregion
        public IList<IValidateFailure> Errors
        {
            get { return(ValidateFailureDataAnnotation.FailureResults(_result )); }
        }

        public bool IsValid
        {
            get { return _success; }
        }
    }
}
