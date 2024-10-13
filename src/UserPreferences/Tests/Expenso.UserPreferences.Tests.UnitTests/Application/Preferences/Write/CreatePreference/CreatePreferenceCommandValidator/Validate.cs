using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference;
using Expenso.UserPreferences.Shared.DTO.API.CreatePreference.Request;

using FluentValidation.Results;

namespace Expenso.UserPreferences.Tests.UnitTests.Application.Preferences.Write.CreatePreference.
    CreatePreferenceCommandValidator;

internal sealed class Validate : CreatePreferenceCommandValidatorTestBase
{
    [Test]
    public void Should_ReturnNoErrors_When_UserIdIsNotEmpty()
    {
        // Arrange
        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _createPreferenceCommand);

        // Assert
        validationResult.Should().NotBeNull();
        validationResult.Errors.Should().BeNullOrEmpty();
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_UserIdIsEmpty()
    {
        // Arrange
        Guid userId = Guid.Empty;

        CreatePreferenceCommand command = new(MessageContext: MessageContextFactoryMock.Object.Current(),
            Payload: new CreatePreferenceRequest(UserId: userId));

        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: command);

        // Assert
        validationResult.Should().NotBeNull();
        validationResult.Errors.Should().NotBeNullOrEmpty();
        validationResult.Errors.Should().HaveCount(expected: 1);
        ValidationFailure validationError = validationResult.Errors.First();
        validationError.PropertyName.Should().Be(expected: "Payload.UserId");
        validationError.ErrorMessage.Should().Be(expected: "The user id must not be empty.");
    }
}