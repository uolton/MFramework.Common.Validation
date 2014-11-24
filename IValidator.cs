namespace MFramework.Common.Validation
{
    public interface IValidator<TValidatable> where TValidatable:class,IValidatable
    {
        IValidateResult Validate(TValidatable v);
        IValidateResult Validate(TValidatable t, IValidateAction<TValidatable> a);
        void Disable();
        bool IsEnabled { get; }
        void  Enable();
    }
}
