using Expenso.Shared.Domain.Types.Exceptions;
using Expenso.Shared.Domain.Types.Rules;

namespace Expenso.Shared.Domain.Types.Model;

public static class DomainModelState
{
    private static readonly List<IBusinessRule> BrokenRules = [];

    public static void CheckBusinessRules(IEnumerable<BusinesRuleCheck> businessRules, bool throwAfterAll = true)
    {
        IBusinessRule[] brokenRules;

        foreach (BusinesRuleCheck ruleCheck in businessRules)
        {
            if (!ruleCheck.BusinessRule.IsBroken())
            {
                continue;
            }

            BrokenRules.Add(item: ruleCheck.BusinessRule);

            if (!ruleCheck.ThrowException)
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