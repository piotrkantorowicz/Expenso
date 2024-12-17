using Expenso.BudgetSharing.Application.Shared.Settings;
using Expenso.Shared.Tests.Utils.UnitTests.Assertions;

using FluentValidation.Results;

using NUnit.Framework;

namespace Expenso.Api.Tests.UnitTests.Configuration.Settings.Services.Validators.BudgetSharingValidator;

[TestFixture]
internal sealed class Validate : BudgetSharingValidatorTestBase
{
    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_ExpirationDaysIsNotProvided()
    {
        // Arrange
        BudgetSharingSettings settings = new()
        {
            ExpirationDays = null
        };

        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: settings);

        // Assert
        validationResult.AssertIsSingleError(propertyName: nameof(settings.ExpirationDays),
            errorMessage: "Expiration days must be provided.");
    }

    [Test, TestCase(arg: 0), TestCase(arg: 8), TestCase(arg: -1), TestCase(arg: 10)]
    public void Should_ReturnValidationResultWithCorrectMessage_When_ExpirationDaysIsOutOfRange(int? invalidValue)
    {
        // Arrange
        BudgetSharingSettings settings = new()
        {
            ExpirationDays = invalidValue
        };

        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: settings);

        // Assert
        validationResult.AssertIsSingleError(propertyName: nameof(settings.ExpirationDays),
            errorMessage: "Expiration days must be between 1 and 7 days.");
    }

    [Test]
    public void Should_ReturnEmptyValidationResult_When_ExpirationDaysIsWithinRange()
    {
        // Arrange
        BudgetSharingSettings settings = new()
        {
            ExpirationDays = 5
        };

        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: settings);

        // Assert
        validationResult.AssertNoErrors();
    }
}