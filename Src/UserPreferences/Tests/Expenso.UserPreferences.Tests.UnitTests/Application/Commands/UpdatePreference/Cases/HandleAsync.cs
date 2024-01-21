using System.Text;

using Expenso.Shared.Types.Exceptions;
using Expenso.UserPreferences.Core.Application.Preferences.Commands.UpdatePreference;
using Expenso.UserPreferences.Core.Application.Preferences.DTO.UpdatePreferences.Request;
using Expenso.UserPreferences.Core.Domain.Preferences.Model;

namespace Expenso.UserPreferences.Tests.UnitTests.Application.Commands.UpdatePreference.Cases;

internal sealed class HandleAsync : UpdatePreferenceCommandHandlerTestBase
{
    [Test]
    public async Task Should_UpdatePreference()
    {
        // Arrange
        UpdatePreferenceCommand command = new(_userId,
            new UpdatePreferenceRequest(new UpdateFinancePreferenceRequest(false, 0, true, 2),
                new UpdateNotificationPreferenceRequest(true, 5), new UpdateGeneralPreferenceRequest(false)));

        _preferenceRepositoryMock
            .Setup(x => x.GetByUserIdAsync(_userId, It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_preference);

        _preferenceRepositoryMock.Setup(x => x.UpdateAsync(_preference, It.IsAny<CancellationToken>()));

        // Act
        await TestCandidate.HandleAsync(command);

        // Assert
        _preferenceRepositoryMock.Verify(
            x => x.GetByUserIdAsync(_userId, It.IsAny<bool>(), It.IsAny<CancellationToken>()), Times.Once);

        _preferenceRepositoryMock.Verify(x => x.UpdateAsync(_preference, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public void Should_ThrowConflictException_When_CreatingPreferenceAndPreferenceAlreadyExists()
    {
        // Arrange
        UpdatePreferenceCommand command = new(_userId,
            new UpdatePreferenceRequest(new UpdateFinancePreferenceRequest(false, 0, true, 2),
                new UpdateNotificationPreferenceRequest(true, 5), new UpdateGeneralPreferenceRequest(false)));

        _preferenceRepositoryMock
            .Setup(x => x.GetByUserIdAsync(_userId, It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Preference?)null);

        // Act
        // Assert
        ConflictException? exception = Assert.ThrowsAsync<ConflictException>(() => TestCandidate.HandleAsync(command));

        string expectedExceptionMessage = new StringBuilder()
            .Append("User preferences for user with id ")
            .Append(command.PreferenceOrUserId)
            .Append(" or with own id: ")
            .Append(command.PreferenceOrUserId)
            .Append(" haven't been found.")
            .ToString();

        exception?.Message.Should().Be(expectedExceptionMessage);
    }
}