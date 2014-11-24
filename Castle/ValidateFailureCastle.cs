namespace MFramework.Common.Validation.Castle
{
    public class ValidateFailureCastle:IValidateFailure
    {
        #region Attributes

        private string _errorMessage;
        private string _propertyName;

        #endregion
        #region Constructor
        internal ValidateFailureCastle(string error,string propertyName)
            {
                _errorMessage=error;
                _propertyName = propertyName;

            }
        #endregion
        #region Propertyes
            public object AttemptedValue { get { return null; } }
            public object CustomState { get { return null; } }
            public string  ErrorMessage { get { return _errorMessage; } }
            public string  PropertyName { get { return _propertyName; } }
        #endregion
            
    }
}
