namespace Expenso.Shared.Domain.Types.Rules;

public sealed record BusinessRuleCheck(IBusinessRule BusinessRule, bool ThrowException = false);