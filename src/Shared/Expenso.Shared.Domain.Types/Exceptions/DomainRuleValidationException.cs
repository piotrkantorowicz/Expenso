using Expenso.Shared.Domain.Types.Rules;
using Expenso.Shared.System.Types.Exceptions.Validation;

namespace Expenso.Shared.Domain.Types.Exceptions;

public class DomainRuleValidationException : Exception
{
    private readonly IEnumerable<IBusinessRule> _brokenRules;

    public DomainRuleValidationException(IReadOnlyCollection<IBusinessRule>? brokenRules) : base(
        message: "Business rule validation failed.")
    {
        _brokenRules = brokenRules ?? new List<IBusinessRule>();
    }

    public string Details =>
        string.Join(separator: Environment.NewLine,
            values: GetErrorDictionary(brokenRules: _brokenRules).Select(selector: x => x.Message));

    public IReadOnlyCollection<ValidationDetailModel> Errors => GetErrorDictionary(brokenRules: _brokenRules);

    private static IReadOnlyCollection<ValidationDetailModel> GetErrorDictionary(IEnumerable<IBusinessRule> brokenRules)
    {
        return brokenRules
            .Select(selector: x => new ValidationDetailModel(Property: x.GetType().Name, Message: x.Message))
            .ToList()
            .AsReadOnly();
    }
}