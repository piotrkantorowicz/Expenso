using System.Text;

using Expenso.Shared.Types.Exceptions;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.UpdatePreference;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.UpdatePreference.DTO.Request;
using Expenso.UserPreferences.Core.Domain.Preferences.Model;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories.Filters;
using Expenso.UserPreferences.Proxy.DTO.MessageBus.FinancePreferences;
using Expenso.UserPreferences.Proxy.DTO.MessageBus.GeneralPreferences;
using Expenso.UserPreferences.Proxy.DTO.MessageBus.NotificationPreferences;

namespace Expenso.UserPreferences.Tests.UnitTests.Application.Preferences.Write.UpdatePreference.
    UpdatePreferenceCommandHandler;

internal sealed class HandleAsync : UpdatePreferenceCommandHandlerTestBase
{
    [Test]
    public async Task Should_UpdatePreference()
    {
        // Arrange
        UpdatePreferenceCommand command = new(_userId,
            new UpdatePreferenceRequest(new UpdateFinancePreferenceRequest(false, 0, true, 2),
                new UpdateNotificationPreferenceRequest(true, 5), new UpdateGeneralPreferenceRequest(true)));

        _preferenceRepositoryMock
            .Setup(x => x.GetAsync(new PreferenceFilter(null, _userId, true, true, true, true),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(_preference);

        _preferenceRepositoryMock.Setup(x => x.UpdateAsync(_preference, It.IsAny<CancellationToken>()));

        // Act
        await TestCandidate.HandleAsync(command);

        // Assert
        _preferenceRepositoryMock.Verify(
            x => x.GetAsync(new PreferenceFilter(null, _userId, true, true, true, true), It.IsAny<CancellationToken>()),
            Times.Once);

        _preferenceRepositoryMock.Verify(x => x.UpdateAsync(_preference, It.IsAny<CancellationToken>()), Times.Once);

        _messageBrokerMock.Verify(
            x => x.PublishAsync(It.IsAny<GeneralPreferenceUpdatedIntegrationEvent>(), It.IsAny<CancellationToken>()),
            Times.Once);

        _messageBrokerMock.Verify(
            x => x.PublishAsync(It.IsAny<FinancePreferenceUpdatedIntegrationEvent>(), It.IsAny<CancellationToken>()),
            Times.Once());

        _messageBrokerMock.Verify(
            x => x.PublishAsync(It.IsAny<NotificationPreferenceUpdatedIntegrationEvent>(),
                It.IsAny<CancellationToken>()), Times.Once());
    }

    [Test]
    public void Should_ThrowConflictException_When_CreatingPreferenceAndPreferenceAlreadyExists()
    {
        // Arrange
        UpdatePreferenceCommand command = new(_userId,
            new UpdatePreferenceRequest(new UpdateFinancePreferenceRequest(false, 0, true, 2),
                new UpdateNotificationPreferenceRequest(true, 5), new UpdateGeneralPreferenceRequest(true)));

        _preferenceRepositoryMock
            .Setup(x => x.GetAsync(new PreferenceFilter(null, _userId, true, true, true, true),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((Preference?)null);

        // Act
        // Assert
        ConflictException? exception = Assert.ThrowsAsync<ConflictException>(() => TestCandidate.HandleAsync(command));

        string expectedExceptionMessage = new StringBuilder()
            .Append("User preferences for user with id ")
            .Append(command.PreferenceOrUserId)
            .Append(" or with own id: ")
            .Append(command.PreferenceOrUserId)
            .Append(" haven't been found")
            .ToString();

        exception?.Message.Should().Be(expectedExceptionMessage);

        _preferenceRepositoryMock.Verify(
            x => x.GetAsync(new PreferenceFilter(null, _userId, true, true, true, true), It.IsAny<CancellationToken>()),
            Times.Once);

        _preferenceRepositoryMock.Verify(x => x.UpdateAsync(_preference, It.IsAny<CancellationToken>()), Times.Never);

        _messageBrokerMock.Verify(
            x => x.PublishAsync(It.IsAny<GeneralPreferenceUpdatedIntegrationEvent>(), It.IsAny<CancellationToken>()),
            Times.Never);

        _messageBrokerMock.Verify(
            x => x.PublishAsync(It.IsAny<FinancePreferenceUpdatedIntegrationEvent>(), It.IsAny<CancellationToken>()),
            Times.Never());

        _messageBrokerMock.Verify(
            x => x.PublishAsync(It.IsAny<NotificationPreferenceUpdatedIntegrationEvent>(),
                It.IsAny<CancellationToken>()), Times.Never());
    }
}