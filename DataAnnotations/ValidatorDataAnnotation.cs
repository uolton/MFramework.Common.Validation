using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MFramework.Common.Validation.Composite;

namespace MFramework.Common.Validation.DataAnnotations
{
    
    public class ValidatorDataAnnotation<TValidatable> : ValidatorBase<TValidatable> where TValidatable : class,IValidatable
    {

        
        public override IValidateResult DoValidate(TValidatable v)
        {
            
            //var context = new ValidationContext(v, serviceProvider: null, items: null);
            var context = new DataAnnotationValidationContext(v);
            context.SetValidationActive();
            var results = new List<ValidationResult>();
            bool success = Validator.TryValidateObject(v, context.Context, results,true);
            return (new ValidateResultComposite(results.Select(r => new ValidateResultDataAnnotation(success, r))));
        }

    }
}
