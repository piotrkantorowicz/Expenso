using Expenso.Shared.Domain.Types.Rules;

namespace Expenso.Shared.Domain.Types.Exceptions;

public class DomainRuleValidationException(IEnumerable<IBusinessRule> brokenRules) : Exception(
    message: string.Join(separator: Environment.NewLine, values: brokenRules.Select(selector: x => x.Message)))
{
    public IReadOnlyCollection<IBusinessRule> BrokenRules => brokenRules.ToList();

    public string Details =>
        string.Join(separator: Environment.NewLine, values: BrokenRules.Select(selector: x => x.Message));

    public override string ToString()
    {
        return string.Join(separator: Environment.NewLine, values: BrokenRules.Select(selector: x => x.Message));
    }
}