using Expenso.Shared.Domain.Types.Exceptions;
using Expenso.Shared.Domain.Types.Rules;

namespace Expenso.BudgetSharing.Domain.Shared.Model.Base;

internal sealed class DomainModelState
{
    private readonly List<IBusinessRule> _brokenRules = [];

    public void CheckBusinessRules(IEnumerable<IBusinessRule> rules, bool throwException = true)
    {
        foreach (IBusinessRule rule in rules)
        {
            _brokenRules.Add(rule);
        }

        if (_brokenRules.Count != 0 && throwException)
        {
            IBusinessRule[] brokenRules = _brokenRules.ToArray();
            _brokenRules.Clear();

            throw new DomainRuleValidationException(brokenRules);
        }
    }
}
