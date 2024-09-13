using Expenso.Shared.Domain.Types.Exceptions;

using FluentAssertions;
using FluentAssertions.Specialized;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain;

public static class ExceptionAssertionsExtensions
{
    public static ExceptionAssertions<DomainRuleValidationException> WithDetails(
        this ExceptionAssertions<DomainRuleValidationException> assertion, string expectedWildcardPattern,
        string because = "", params object[] becauseArgs)
    {
        assertion.Where(exceptionExpression: x => x.Details == expectedWildcardPattern);

        return assertion;
    }

    public static Task<ExceptionAssertions<DomainRuleValidationException>> WithDetailsAsync(
        this Task<ExceptionAssertions<DomainRuleValidationException>> assertion, string expectedWildcardPattern,
        string because = "", params object[] becauseArgs)
    {
        assertion.Where(exceptionExpression: x => x.Details == expectedWildcardPattern);

        return assertion;
    }
}