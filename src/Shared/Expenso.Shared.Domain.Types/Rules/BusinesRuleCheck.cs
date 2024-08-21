namespace Expenso.Shared.Domain.Types.Rules;

public sealed record BusinesRuleCheck(IBusinessRule BusinessRule, bool ThrowException = false);