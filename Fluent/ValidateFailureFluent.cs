using FluentValidation.Results;

namespace MFramework.Common.Validation.Fluent
{
    public class ValidateFailureFluent:IValidateFailure
    {
        #region Attributes

        private ValidationFailure _failure;

        #endregion
        #region Constructor
            internal ValidateFailureFluent(ValidationFailure f)
            {
                _failure = f;
            }
        #endregion
        #region Propertyes
            public object AttemptedValue { get { return _failure.AttemptedValue; } }
            public object CustomState { get { return _failure.CustomState; } }
            public string  ErrorMessage { get { return _failure.ErrorMessage; } }
            public string  PropertyName { get { return _failure.PropertyName; } }
        #endregion
            
    }
}
