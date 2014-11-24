using Castle.Components.Validator;
using MFramework.Common.Validation.Extending.Castle;

namespace MFramework.Common.Validation.Castle
{
    public   class ValidatorCastle<TValidatable> :ValidatorBase<TValidatable> where TValidatable : class,IValidatable
    {
        #region Attributes

        private ValidatorRunner _validator;
        #endregion
        #region Constructor

        public  ValidatorCastle()
        {
            //
            _validator = new ValidatorRunner(new MFrameworkCachedMetadataValidationRegistry());
            
        }
        #endregion
        public override IValidateResult DoValidate(TValidatable t)
        {
            // deve essere richieamata prima di ottenere l'oggetto  Error Summary
            
            bool isValid = _validator.IsValid(t);
            return (new ValidateResultCastle(_validator.GetErrorSummary(t)));
        
        }
        
    }
}
