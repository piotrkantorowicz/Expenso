using System.Collections.ObjectModel;

using Expenso.Shared.Domain.Types.Rules;

namespace Expenso.Shared.Domain.Types.Exceptions;

public class DomainRuleValidationException : Exception
{
    private readonly IEnumerable<IBusinessRule> _brokenRules;

    public DomainRuleValidationException(IReadOnlyCollection<IBusinessRule>? brokenRules) : base(
        message: "Business rule validation failed")
    {
        _brokenRules = brokenRules ?? new List<IBusinessRule>();
    }

    public string Details =>
        string.Join(separator: Environment.NewLine, values: GetMessages(brokenRules: _brokenRules));

    private static ReadOnlyCollection<string> GetMessages(IEnumerable<IBusinessRule> brokenRules)
    {
        return brokenRules.Select(selector: x => x.Message).ToList().AsReadOnly();
    }
}