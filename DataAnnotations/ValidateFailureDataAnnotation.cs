using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MFramework.Common.Generics;

namespace MFramework.Common.Validation.DataAnnotations
{
    public class ValidateFailureDataAnnotation:IValidateFailure
    {
        #region Attributes

        private  string _errorMessage;
        private string _propertyName;
        #endregion
        #region Constructor
        private ValidateFailureDataAnnotation(string errorMessage,string propertyName)
        {
            _errorMessage = errorMessage;
            _propertyName = propertyName;
        }
        #endregion
        #region Propertyes
            public object AttemptedValue { get { return null; } }
            public object CustomState { get { return null; } }
            public string  ErrorMessage { get { return _errorMessage; } }
            public string PropertyName { get { return _propertyName; } }
        #endregion
        #region Static Members
            internal static  IList<IValidateFailure> FailureResults(ValidationResult result) 
            {
                return (result.MemberNames.Select(n => new ValidateFailureDataAnnotation(result.ErrorMessage, n)).ToList().ToCovariant<ValidateFailureDataAnnotation, IValidateFailure>());
            }
        #endregion
    }
}
