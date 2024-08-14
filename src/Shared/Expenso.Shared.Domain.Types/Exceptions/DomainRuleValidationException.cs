using Expenso.Shared.Domain.Types.Rules;

namespace Expenso.Shared.Domain.Types.Exceptions;

public class DomainRuleValidationException : Exception
{
    private readonly IEnumerable<IBusinessRule> _brokenRules;

    public DomainRuleValidationException(IEnumerable<IBusinessRule> brokenRules) : base(
        message: string.Join(separator: Environment.NewLine, values: brokenRules.Select(selector: x => x.Message)))
    {
        _brokenRules = brokenRules;
    }

    public IReadOnlyCollection<IBusinessRule> BrokenRules => _brokenRules.ToList();

    public string Details =>
        string.Join(separator: Environment.NewLine, values: BrokenRules.Select(selector: x => x.Message));

    public override string ToString()
    {
        return string.Join(separator: Environment.NewLine, values: BrokenRules.Select(selector: x => x.Message));
    }
}