using System.Text;

using Expenso.Shared.Types.Exceptions;
using Expenso.UserPreferences.Core.Models;

namespace Expenso.UserPreferences.Tests.UnitTests.Validators.Cases;

internal sealed class ValidateCreateAsync : PreferenceValidatorTestBase
{
    [Test]
    public void Should_NotThrowValidationException_When_UserIdIsNotEmpty()
    {
        // Arrange
        Guid userId = Guid.NewGuid();

        // Act
        // Assert
        Assert.DoesNotThrowAsync(() => TestCandidate.ValidateCreateAsync(userId, default));
    }

    [Test]
    public void Should_ThrowValidationException_When_UserIdIsEmpty()
    {
        // Arrange
        Guid userId = Guid.Empty;

        // Act
        // Assert
        ValidationException? exception =
            Assert.ThrowsAsync<ValidationException>(() => TestCandidate.ValidateCreateAsync(userId, default));

        exception?.Message.Should().Be("One or more validation failures have occurred.");
        exception?.Details.Should().Be(new StringBuilder().AppendLine("UserId: User id cannot be empty.").ToString());
    }

    [Test]
    public void Should_ThrowConflictException_When_PreferenceAlreadyExists()
    {
        // Arrange
        Guid userId = Guid.NewGuid();

        PreferencesRepositoryMock
            .Setup(x => x.GetByUserIdAsync(userId, true, default))
            .ReturnsAsync(Preference.CreateDefault(userId));

        // Act
        // Assert
        ConflictException? exception =
            Assert.ThrowsAsync<ConflictException>(() => TestCandidate.ValidateCreateAsync(userId, default));

        exception
            ?.Message.Should()
            .Be(new StringBuilder()
                .Append("Preferences for user with id ")
                .Append(userId)
                .Append(" already exists.")
                .ToString());
    }

    [Test]
    public void Should_NotThrowAnyException_When_PreferenceDoesNotExist()
    {
        // Arrange
        Guid userId = Guid.NewGuid();
        PreferencesRepositoryMock.Setup(x => x.GetByUserIdAsync(userId, true, default)).ReturnsAsync((Preference?)null);

        // Act
        // Assert
        Assert.DoesNotThrowAsync(() => TestCandidate.ValidateCreateAsync(userId, default));
    }
}