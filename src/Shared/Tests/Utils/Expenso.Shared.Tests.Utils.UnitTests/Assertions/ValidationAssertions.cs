using FluentValidation.Results;

using Shouldly;

namespace Expenso.Shared.Tests.Utils.UnitTests.Assertions;

public static class ValidationAssertions
{
    public static void AssertNoErrors(this ValidationResult validationResult)
    {
        validationResult.ShouldNotBeNull();
        validationResult.Errors.ShouldBeEmpty();
    }

    public static void AssertIsSingleError(this ValidationResult validationResult, string propertyName,
        string errorMessage)
    {
        validationResult.ShouldNotBeNull();
        validationResult.IsValid.ShouldBeFalse();
        validationResult.Errors.ShouldNotBeEmpty();
        validationResult.Errors.Count.ShouldBe(expected: 1);
        validationResult.Errors[index: 0].PropertyName.ShouldBe(expected: propertyName);
        validationResult.Errors[index: 0].ErrorMessage.ShouldBe(expected: errorMessage);
    }

    public static void AssertContainsSingleError(this ValidationResult validationResult, string propertyName,
        string errorMessage)
    {
        validationResult.ShouldNotBeNull();
        validationResult.IsValid.ShouldBeFalse();
        validationResult.Errors.ShouldNotBeEmpty();
        validationResult.Errors.Count.ShouldBeGreaterThan(expected: 0);

        validationResult.Errors.ShouldContain(elementPredicate: x =>
            x.PropertyName == propertyName && x.ErrorMessage == errorMessage);
    }

    public static void AssertManyErrors(this ValidationResult validationResult, IDictionary<string, string> errors)
    {
        validationResult.ShouldNotBeNull();
        validationResult.IsValid.ShouldBeFalse();
        validationResult.Errors.ShouldNotBeEmpty();
        validationResult.Errors.Count.ShouldBe(expected: errors.Count);

        foreach (KeyValuePair<string, string> error in errors)
        {
            validationResult.Errors.ShouldContain(elementPredicate: e =>
                e.PropertyName == error.Key && e.ErrorMessage == error.Value);
        }
    }
}