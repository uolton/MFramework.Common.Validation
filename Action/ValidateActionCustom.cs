using System;

namespace MFramework.Common.Validation.Action
{
    public class ValidateActionCustom<TValidatable> : IValidateAction<TValidatable> where TValidatable : class,IValidatable
    {
        #region Attributes

        private Action<TValidatable, IValidateFailure> _action;
        #endregion
        #region Constructor
        public ValidateActionCustom(Action<TValidatable, IValidateFailure> a)
        {
            _action = a;
        }
        #endregion
        #region IEntityValidationAction Interface
        public void FailureOnValidation(TValidatable entity, IValidateFailure f)
        {
            _action(entity, f);
        }
        #endregion
        
    }
}
