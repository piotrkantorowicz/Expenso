using Expenso.Shared.Domain.Types.Exceptions;

using Shouldly;

namespace Expenso.Shared.Tests.Utils.UnitTests.Assertions;

public static class ExceptionAssertions
{
    public static void AssertDomainRuleValidationException(this Action action, string expectedDetails)
    {
        DomainRuleValidationException? exception = action.ShouldThrow<DomainRuleValidationException>();
        exception.Message.ShouldBe(expected: "Business rule validation failed.");
        exception.Details.ShouldBe(expected: expectedDetails);
    }

    public static async Task AssertDomainRuleValidationExceptionAsync(this Func<Task> action, string expectedDetails)
    {
        DomainRuleValidationException? exception = await action.ShouldThrowAsync<DomainRuleValidationException>();
        exception.Message.ShouldBe(expected: "Business rule validation failed.");
        exception.Details.ShouldBe(expected: expectedDetails);
    }
}