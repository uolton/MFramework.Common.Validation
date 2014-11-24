using MFramework.Common.Validation;

namespace MFramework.Common.Validation
{
     
    public   abstract class ValidatableObjectBase<TValidatable,TValidator> : IValidatable
        where TValidatable : ValidatableObjectBase<TValidatable,TValidator> 
        where TValidator : class,IValidator<TValidatable>,new()
    {
        #region Attributes
       protected IValidator<TValidatable> _validator;
        #endregion
        
        #region constructors
       protected ValidatableObjectBase()
       {
           _validator = new TValidator();
       }
       protected ValidatableObjectBase(IValidator<TValidatable> validator)
        {
            _validator = validator;
        }
    #endregion
       #region  IEntity Interface
       public bool IsValid() {return Validate().IsValid;} 
         
         public IValidateResult  Validate()
            {
                return (_validator.Validate(this as TValidatable)); 
            }
        #endregion
        
    }
}
