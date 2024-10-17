using FluentAssertions;

using FluentValidation.Results;

namespace Expenso.Shared.Tests.Utils.UnitTests.Assertions;

public static class CommandValidationAssertions
{
    public static void AssertNoErrors(this ValidationResult validationResult)
    {
        validationResult.Should().NotBeNull();
        validationResult.Errors.Should().BeNullOrEmpty();
    }

    public static void AssertSingleError(this ValidationResult validationResult, string propertyName,
        string errorMessage)
    {
        validationResult.Should().NotBeNull();
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Should().NotBeEmpty();
        validationResult.Errors.Should().HaveCount(expected: 1);
        validationResult.Errors[index: 0].PropertyName.Should().Be(expected: propertyName);
        validationResult.Errors[index: 0].ErrorMessage.Should().Be(expected: errorMessage);
    }

    public static void AssertManyErrors(this ValidationResult validationResult, IDictionary<string, string> errors)
    {
        validationResult.Should().NotBeNull();
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Should().NotBeEmpty();
        validationResult.Errors.Should().HaveCount(expected: errors.Count);

        foreach (KeyValuePair<string, string> error in errors)
        {
            validationResult
                .Errors.Should()
                .ContainSingle(predicate: e => e.PropertyName == error.Key && e.ErrorMessage == error.Value);
        }
    }
}