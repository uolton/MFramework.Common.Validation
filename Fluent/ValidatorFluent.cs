using System;
using System.Linq.Expressions;
using FluentValidation;
using FluentValidation.Results;
using MFramework.Common.Specifications;

namespace MFramework.Common.Validation.Fluent
{
    public  abstract class ValidatorFluent<TValidatable> :ValidatorBase<TValidatable> where TValidatable : class,IValidatable
    {
        #region Attributes

        private AbstractValidator<TValidatable> _validator;
        #endregion
        #region Constructor

        protected ValidatorFluent()
        {
            _validator = new InlineValidator<TValidatable>();
            
        }
        #endregion
        #region Protected Interface
        protected void AddRules(IValidationRule r)
        {
            _validator.AddRule(r);
        }
        protected IRuleBuilderInitial<TValidatable, TProperty> RuleFor<TProperty>(Expression<Func<TValidatable, TProperty>> expression)
        {
            return(_validator.RuleFor(expression));
            
        }
        protected CustomRule Rule(Expression<Func<TValidatable, bool>> rule)
        {
            return new CustomRule(this, rule);
        }

        protected CustomRule Rule()
        {
            return new CustomRule(this);
        }
        #endregion
        public override IValidateResult DoValidate(TValidatable t)
        {
            return (new ValidateResultFluent(_validator.Validate(t)));
        
        }

        protected class CustomRule
        {
            private ValidatorFluent<TValidatable> _validator;
            private ISpecification<TValidatable> _rule;
            private string _ruleName;
            private string _error;
            public CustomRule(ValidatorFluent<TValidatable> validator):this(validator,Specification<TValidatable>.True)
            {
            
            }
            public CustomRule(ValidatorFluent<TValidatable> validator, Expression<Func<TValidatable, bool>> rule):this(validator,new Specification<TValidatable>(rule))
            {
            }
            public CustomRule(ValidatorFluent<TValidatable> validator, ISpecification<TValidatable>  rule)
            {
                _validator = validator;
                _rule=rule;
            }
            public CustomRule Must(
                Expression<Func<TValidatable, bool>> rule)
            {
                _rule=new Specification<TValidatable>(rule);
                return this;
            }
            public CustomRule Is(
                Action<ISpecification<TValidatable>> addRuleAction)
            {
                addRuleAction(_rule);
                return this;
            }

            public CustomRule RuleName(string name)
            {
                _ruleName = name;
                return this;
            }

            public CustomRule Error(string error)
            {
                _error = error;
                return this;
            }

            public void Add()
            {
                _validator._validator.Custom(
                                validatable => _rule.IsSatisfiedBy(validatable) ? null : new ValidationFailure(_ruleName, _error));                
            }

        }
    }
}
