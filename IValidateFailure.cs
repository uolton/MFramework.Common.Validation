namespace MFramework.Common.Validation
{
    public interface IValidateFailure{
        #region Propertyes
        object AttemptedValue { get; }
        object CustomState { get; }
        string ErrorMessage { get; }
        string PropertyName { get; }
        #endregion
    }
}
