using Expenso.Shared.Domain.Types.Rules;

namespace Expenso.Shared.Domain.Types.Exceptions;

public class DomainRuleValidationException(ICollection<IBusinessRule> brokenRules)
    : Exception(string.Join(Environment.NewLine, brokenRules.Select(x => x.Message)))
{
    public IEnumerable<IBusinessRule> BrokenRules { get; } = brokenRules;

    public string Details => string.Join(Environment.NewLine, BrokenRules.Select(x => x.Message));

    public override string ToString()
    {
        return string.Join(Environment.NewLine, BrokenRules.Select(x => x.Message));
    }
}
