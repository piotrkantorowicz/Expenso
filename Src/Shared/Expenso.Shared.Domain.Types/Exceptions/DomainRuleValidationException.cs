using Expenso.Shared.Domain.Types.Rules;

namespace Expenso.Shared.Domain.Types.Exceptions;

public class DomainRuleValidationException(IEnumerable<IBusinessRule> brokenRules)
    : Exception(string.Join(Environment.NewLine, brokenRules.Select(x => x.Message)))
{
    public IReadOnlyCollection<IBusinessRule> BrokenRules => brokenRules.ToList();

    public string Details => string.Join(Environment.NewLine, BrokenRules.Select(x => x.Message));

    public override string ToString()
    {
        return string.Join(Environment.NewLine, BrokenRules.Select(x => x.Message));
    }
}