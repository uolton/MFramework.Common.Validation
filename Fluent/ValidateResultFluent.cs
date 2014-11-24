using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;
using MFramework.Common.Generics;

namespace MFramework.Common.Validation.Fluent
{
    public class ValidateResultFluent:IValidateResult
    {
        #region Attributes
       
        private ValidationResult _result;
        #endregion
        #region Constructor
        internal ValidateResultFluent(ValidationResult r)
        {
            _result = r;
        }
        #endregion
        
        #region IEntityValidationResult Interface 

        public IList<IValidateFailure> Errors
        {
            get { return GetErrors(); }
        }

        public bool IsValid
        {
            get { return _result.IsValid; }
        }

        #endregion    
        #region Private Interface 
            private IList<IValidateFailure> GetErrors()
            {
                return( _result.Errors.Select(f => new ValidateFailureFluent(f)).ToList().ToCovariant<ValidateFailureFluent,IValidateFailure>());
            }
        #endregion
        }
        
    }
    

