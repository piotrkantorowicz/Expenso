using FluentValidation;

namespace Expenso.Shared.Commands.Validation.Rules;

public static class GuidRuleExtensions
{
    public static IRuleBuilderOptions<T, Guid?> NotNullOrEmpty<T>(this IRuleBuilder<T, Guid?> ruleBuilder)
    {
        return ruleBuilder.Must(predicate: id => id is not null && id != Guid.Empty);
    }
}