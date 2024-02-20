using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference.External;
using Expenso.UserPreferences.Proxy.DTO.API.CreatePreference.Request;

namespace Expenso.UserPreferences.Tests.UnitTests.Application.Preferences.Write.CreatePreference.External.
    CreatePreferenceCommandValidator;

internal sealed class Validate : CreatePreferenceCommandValidatorTestBase
{
    [Test]
    public void Should_ReturnEmptyValidationResult_When_UserIdIsNotEmpty()
    {
        // Arrange
        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(_createPreferenceCommand);

        // Assert
        validationResult.Should().BeNullOrEmpty();
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_UserIdIsEmpty()
    {
        // Arrange
        Guid userId = Guid.Empty;

        CreatePreferenceCommand command = new(MessageContextFactoryMock.Object.Current(),
            new CreatePreferenceRequest(userId));

        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(command);

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "User id cannot be empty";
        string error = validationResult[nameof(command.Preference.UserId)];
        error.Should().Be(expectedValidationMessage);
    }
}