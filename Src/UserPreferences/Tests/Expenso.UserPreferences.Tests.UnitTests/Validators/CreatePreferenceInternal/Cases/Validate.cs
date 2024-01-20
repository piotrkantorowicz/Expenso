using Expenso.UserPreferences.Core.Application.Preferences.Internal.Commands.CreatePreference;
using Expenso.UserPreferences.Proxy.DTO.API.CreatePreference.Request;

namespace Expenso.UserPreferences.Tests.UnitTests.Validators.CreatePreferenceInternal.Cases;

internal sealed class Validate : CreatePreferenceInternalCommandValidatorTestBase
{
    [Test]
    public void Should_ReturnEmptyValidationResult_When_UserIdIsNotEmpty()
    {
        // Arrange
        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(_createPreferenceInternalCommand);

        // Assert
        validationResult.Should().BeNullOrEmpty();
    }

    [Test]
    public void Should_ReturnValidationResultWithCorrectMessage_When_UserIdIsEmpty()
    {
        // Arrange
        Guid userId = Guid.Empty;

        CreatePreferenceInternalCommand command =
            new CreatePreferenceInternalCommand(new CreatePreferenceInternalRequest(userId));

        // Act
        IDictionary<string, string> validationResult = TestCandidate.Validate(command);

        // Assert
        validationResult.Should().NotBeNullOrEmpty();
        const string expectedValidationMessage = "User id cannot be empty.";
        string error = validationResult[nameof(command.Preference.UserId)];
        error.Should().Be(expectedValidationMessage);
    }
}