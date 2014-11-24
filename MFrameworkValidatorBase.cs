using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Fasterflect;
using MFramework.Common.Core.Collections.Extensions;
using MFramework.Common.Core.Extensions;
using MFramework.Common.Core.Types.Extensions;
using MFramework.Common.Validation;
using MFramework.Common.Validation.Castle;
using MFramework.Common.Validation.Composite;
using MFramework.Common.Validation.DataAnnotations;
using MFramework.Common.Validation.Fluent;

namespace MFramework.Common.Validation
{
    public abstract class MFrameworkValidatorBase<TValidatable> : ValidatorFluent<TValidatable> where TValidatable : class,IValidatable
    {
        #region Members Variable
        public  interface UseValidator<TValidator> where TValidator:IValidator<TValidatable>{ }
        
        private ValidatorComposite<TValidatable> _v;
        private bool _onValidationCall; 
        #endregion
#region constructors
        protected MFrameworkValidatorBase()
        {
            _v = new ValidatorComposite<TValidatable>(GetValidators(this));
            _onValidationCall = false;
        }
#endregion
        public override IValidateResult DoValidate(TValidatable t)
        {
            if (!_onValidationCall)
            {
                _onValidationCall = !_onValidationCall;
                return (_v.Validate(t));
            }
            _onValidationCall = !_onValidationCall;
            return(base.DoValidate(t));
        }

        /// <summary>
        /// restituisce i tipi di validator da gestire
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        private static IEnumerable<IValidator<TValidatable>> GetValidators(MFrameworkValidatorBase<TValidatable> instance)
        {
            
            return ExtractValidatorsType(instance.GetType()).Select(t => t.CreateInstance()).Union(new[] { instance }).Cast<IValidator<TValidatable>>();
        }
        /// <summary>
        /// Estrae dalla definizione della classe Validator tramite le interfacce UseValidator i
        /// validator richiesti
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static IEnumerable<Type> ExtractValidatorsType(Type type)
        {

            return (from interfaceType in type.GetInterfaces()
                where interfaceType.IsGenericType
                let baseInterface = interfaceType.GetGenericTypeDefinition()
                where baseInterface == typeof (UseValidator<>)
                select
                    interfaceType.GetGenericArguments()
                        .First(
                            t =>
                                t.GetInterfaces()
                                    .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof (IValidator<>))));
                


        }
        
    }
}
