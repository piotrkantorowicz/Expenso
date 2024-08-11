namespace Expenso.Shared.Domain.Types.Rules;

public interface IBusinessRule
{
    string Message { get; }

    bool IsBroken();
}