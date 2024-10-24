using System.Text;

using Expenso.Shared.Tests.Utils.UnitTests.Assertions;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference;
using Expenso.UserPreferences.Shared.DTO.API.CreatePreference.Request;

using FluentValidation.Results;

namespace Expenso.UserPreferences.Tests.UnitTests.Application.Preferences.Write.CreatePreference.
    CreatePreferenceCommandValidator;

[TestFixture]
internal sealed class Validate : CreatePreferenceCommandValidatorTestBase
{
    [Test]
    public void Should_ReturnNoErrors_When_UserIdIsNotEmpty()
    {
        // Arrange
        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: _createPreferenceCommand);

        // Assert
        validationResult.AssertNoErrors();
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_PayloadIsEmpty()
    {
        // Arrange
        CreatePreferenceCommand command = new(MessageContext: MessageContextFactoryMock.Object.Current(),
            Payload: null);

        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: command);

        // Assert
        validationResult.AssertSingleError(propertyName: nameof(command.Payload),
            errorMessage: "The command payload must not be null.");
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
        validationResult.AssertSingleError(
            propertyName: new StringBuilder()
                .Append(value: nameof(command.Payload))
                .Append(value: ".")
                .Append(value: nameof(command.Payload.UserId))
                .ToString(), errorMessage: "The user ID must not be empty.");
    }
}