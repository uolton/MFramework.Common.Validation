namespace MFramework.Common.Validation
{
    /// <summary>
    ///     Interfaccia base Entita per  <see cref="EntityBase{TEntity,TValidator}" /> and 
    ///     Basato su architettura  S#arp https://github.com/sharparchitecture/Sharp-Architecture
    /// </summary>
    public interface IValidatable
    {
        IValidateResult  Validate();
        bool IsValid();

    }
}
