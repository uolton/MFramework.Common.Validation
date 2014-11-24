using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MFramework.Common.Validation
{
    public interface IValidateResult
    {
        IList<IValidateFailure> Errors { get; }
        bool IsValid { get; }
    }
    public static class ValidateResultExtensionExtension
    {
        public static IEnumerable<ValidationResult> AsValidationResult(this IValidateResult @this)
        {
            return @this.Errors.Select(e => new ValidationResult(e.ErrorMessage,new [] {e.PropertyName} ));
        }
    }
}
