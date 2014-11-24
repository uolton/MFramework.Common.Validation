using System.Collections.Generic;
using System.Linq;
using Castle.Components.Validator;

namespace MFramework.Common.Validation.Castle
{
    public class ValidateResultCastle:IValidateResult
    {
        #region Attributes
       
        private ErrorSummary _result;
        #endregion
        #region Constructor
        internal ValidateResultCastle(ErrorSummary r)
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
            get { return ! _result.HasError; }
        }

        #endregion    
        #region Private Interface 
            private IList<IValidateFailure> GetErrors()
            {
                List<IValidateFailure> errors = new List<IValidateFailure>();

                (from p in _result.InvalidProperties
                 select new {property = p, errs = _result.GetErrorsForProperty(p)}).ToList()
                                                                                   .ForEach(
                                                                                       e =>
                                                                                       e.errs.ToList()
                                                                                        .ForEach(
                                                                                            m =>
                                                                                            errors.Add(
                                                                                                new ValidateFailureCastle
                                                                                                    (m, e.property))));
                return errors;

            }
        #endregion
        }
        
    }
    

