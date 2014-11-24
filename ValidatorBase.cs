using System.Linq;

namespace MFramework.Common.Validation
{
    /// <summary>
    /// ValidatorBase<TValidatable> 
    /// </summary>
    /// <typeparam name="TValidatable"></typeparam>
    public abstract class ValidatorBase<TValidatable> : IValidator<TValidatable> where TValidatable : class,IValidatable
    {
        protected bool _enabled = true;
        private IValidateResult noError = new NoErrorValidationResult();
        public abstract IValidateResult DoValidate(TValidatable v);
        
        public IValidateResult Validate(TValidatable v)
        {
            if (IsEnabled) return DoValidate(v);
            return noError;
        }

        
        public virtual IValidateResult Validate(TValidatable t, IValidateAction<TValidatable> a)
        {
            
            IValidateResult result = Validate(t);
            result.Errors.ToList().ForEach(f => a.FailureOnValidation(t, f));
            return (result);
        }

        public void Disable()
        {
            _enabled=false;
        }

        public bool IsEnabled
        {
            get { return _enabled; }
        }

        public void Enable()
        {
            _enabled=true;
        }
    }
}
