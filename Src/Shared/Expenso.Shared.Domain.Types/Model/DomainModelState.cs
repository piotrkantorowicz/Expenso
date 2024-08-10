using Expenso.Shared.Domain.Types.Exceptions;
using Expenso.Shared.Domain.Types.Rules;

namespace Expenso.Shared.Domain.Types.Model;

public static class DomainModelState
{
    private static readonly List<IBusinessRule> BrokenRules = [];

    public static void CheckBusinessRules(IEnumerable<(IBusinessRule rule, bool throwException)> businessRules,
        bool throwAfterAll = true)
    {
        IBusinessRule[] brokenRules;

        foreach ((IBusinessRule? rule, bool throwException) in businessRules)
        {
            if (!rule.IsBroken())
            {
                continue;
            }

            BrokenRules.Add(item: rule);

            if (!throwException)
            {
                continue;
            }

            brokenRules = BrokenRules.ToArray();
            BrokenRules.Clear();

            throw new DomainRuleValidationException(brokenRules: brokenRules);
        }

        if (!throwAfterAll || BrokenRules.Count == 0)
        {
            return;
        }

        brokenRules = BrokenRules.ToArray();
        BrokenRules.Clear();

        throw new DomainRuleValidationException(brokenRules: brokenRules);
    }
}