using System.ComponentModel.DataAnnotations;

namespace MFramework.Common.Validation.DataAnnotations
{
    public class DataAnnotationValidationContext
        {
            private ValidationContext _ctx;
            private const string ACTIVE_ID = "Active";
        public DataAnnotationValidationContext(object instanceToValidate)
            : this(new ValidationContext(instanceToValidate, serviceProvider: null, items: null))
            {
                _ctx.Items.Add(ACTIVE_ID, false);
            }
        public DataAnnotationValidationContext(ValidationContext ctx)
            {
                _ctx = ctx;

            }
            public ValidationContext Context { get { return _ctx; } }

            public DataAnnotationValidationContext SetValidationActive()
            {
                _ctx.Items[ACTIVE_ID]= true;
                return (this);
            }

            public bool IsValidationActive
            {
                get
                {
                    if (! _ctx.Items.ContainsKey(ACTIVE_ID)) return false;
                    return (bool) _ctx.Items[ACTIVE_ID];
                }
            }
        }
}
