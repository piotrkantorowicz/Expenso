using Expenso.Shared.Domain.Types.Exceptions;
using Expenso.Shared.Domain.Types.Rules;

namespace Expenso.Shared.Domain.Types.Model;

public static class DomainModelState
{
    private static readonly List<IBusinessRule> BrokenRules = [];

    public static void CheckBusinessRules(IEnumerable<IBusinessRule> rules, bool throwException = true)
    {
        foreach (IBusinessRule rule in rules)
        {
            if (rule.IsBroken())
            {
                BrokenRules.Add(rule);
            }
        }

        if (BrokenRules.Count != 0 && throwException)
        {
            IBusinessRule[] brokenRules = BrokenRules.ToArray();
            DomainRuleValidationException exception = new(brokenRules);
            BrokenRules.Clear();

            throw exception;
        }
    }
}