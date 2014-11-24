using System.Collections.Generic;
using System.Linq;

namespace MFramework.Common.Validation.Composite
{
    class ValidateResultComposite:IValidateResult
    {
        #region Members variables

        private List<IValidateResult> _results;
        #endregion
        #region constructors
                public ValidateResultComposite()
                {
                    _results=new List<IValidateResult>();
                }
                public ValidateResultComposite(IEnumerable<IValidateResult> l)
                    : this()
                {
                    (from r in l select r).ToList().ForEach(r => _results.Add(r));
                }
                public ValidateResultComposite(IValidateResult r)
                    : this(new [] { r }){}
                
        #endregion

        public ValidateResultComposite Add(IValidateResult v)
        {
            _results.Add(v);
            return (this);
        }
        public IList<IValidateFailure> Errors
        {
            
            get
            {
                List<IValidateFailure> e = new List<IValidateFailure>();
                _results.ForEach(r => e.AddRange(r.Errors));
                return e;
            }
        }

        public bool IsValid
        {
            get {  return(_results.All (r =>  r.IsValid)); }
        }
    }
}
