using System.Collections.Generic;

namespace MFramework.Common.Validation.Composite
{
    class ValidatorComposite<TValidatable> : ValidatorBase<TValidatable> where TValidatable : class,IValidatable
    {
        #region member variable

        private List<IValidator<TValidatable>> _validators;
        #endregion
        #region Constructor
        private ValidatorComposite(List<IValidator<TValidatable>> validators)
        {
            _validators = validators;
        }
        public  ValidatorComposite():this(new List<IValidator<TValidatable>>() ){}
        public ValidatorComposite(IValidator<TValidatable> validator) : this(new List<IValidator<TValidatable>> (new []{validator})) { }
        public ValidatorComposite(IEnumerable<IValidator<TValidatable>> validators) : this(new List<IValidator<TValidatable>>(validators)) { }

        #endregion

        

        public override IValidateResult DoValidate(TValidatable v)
        {
            ValidateResultComposite r = new ValidateResultComposite();
            _validators.ForEach(vldtr => r.Add(vldtr.Validate(v)));
            return r;
        }
    }
}
