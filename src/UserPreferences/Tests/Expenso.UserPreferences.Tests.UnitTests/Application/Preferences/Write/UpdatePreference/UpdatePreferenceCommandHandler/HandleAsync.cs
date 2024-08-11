using Expenso.Shared.System.Types.Exceptions;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.UpdatePreference;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.UpdatePreference.DTO.Request;
using Expenso.UserPreferences.Core.Domain.Preferences.Model;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories.Filters;
using Expenso.UserPreferences.Proxy.DTO.MessageBus.UpdatePreference.FinancePreferences;
using Expenso.UserPreferences.Proxy.DTO.MessageBus.UpdatePreference.GeneralPreferences;
using Expenso.UserPreferences.Proxy.DTO.MessageBus.UpdatePreference.NotificationPreferences;

namespace Expenso.UserPreferences.Tests.UnitTests.Application.Preferences.Write.UpdatePreference.
    UpdatePreferenceCommandHandler;

internal sealed class HandleAsync : UpdatePreferenceCommandHandlerTestBase
{
    [Test]
    public async Task Should_UpdatePreference()
    {
        // Arrange
        UpdatePreferenceCommand command = new(MessageContext: MessageContextFactoryMock.Object.Current(),
            PreferenceOrUserId: _userId,
            Preference: new UpdatePreferenceRequest(
                FinancePreference: new UpdatePreferenceRequest_FinancePreference(AllowAddFinancePlanSubOwners: false,
                    MaxNumberOfSubFinancePlanSubOwners: 0, AllowAddFinancePlanReviewers: true,
                    MaxNumberOfFinancePlanReviewers: 2),
                NotificationPreference: new UpdatePreferenceRequest_NotificationPreference(
                    SendFinanceReportEnabled: true, SendFinanceReportInterval: 5),
                GeneralPreference: new UpdatePreferenceRequest_GeneralPreference(UseDarkMode: true)));

        _preferenceRepositoryMock
            .Setup(expression: x => x.GetAsync(new PreferenceFilter(null, _userId, true, true, true, true),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: _preference);

        _preferenceRepositoryMock.Setup(expression: x => x.UpdateAsync(_preference, It.IsAny<CancellationToken>()));

        // Act
        await TestCandidate.HandleAsync(command: command, cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        _preferenceRepositoryMock.Verify(
            expression: x => x.GetAsync(new PreferenceFilter(null, _userId, true, true, true, true),
                It.IsAny<CancellationToken>()), times: Times.Once);

        _preferenceRepositoryMock.Verify(expression: x => x.UpdateAsync(_preference, It.IsAny<CancellationToken>()),
            times: Times.Once);

        _messageBrokerMock.Verify(
            expression: x =>
                x.PublishAsync(It.IsAny<GeneralPreferenceUpdatedIntegrationEvent>(), It.IsAny<CancellationToken>()),
            times: Times.Once);

        _messageBrokerMock.Verify(
            expression: x =>
                x.PublishAsync(It.IsAny<FinancePreferenceUpdatedIntegrationEvent>(), It.IsAny<CancellationToken>()),
            times: Times.Once());

        _messageBrokerMock.Verify(
            expression: x => x.PublishAsync(It.IsAny<NotificationPreferenceUpdatedIntegrationEvent>(),
                It.IsAny<CancellationToken>()), times: Times.Once());
    }

    [Test]
    public void Should_ThrowConflictException_When_CreatingPreferenceAndPreferenceAlreadyExists()
    {
        // Arrange
        UpdatePreferenceCommand command = new(MessageContext: MessageContextFactoryMock.Object.Current(),
            PreferenceOrUserId: _userId,
            Preference: new UpdatePreferenceRequest(
                FinancePreference: new UpdatePreferenceRequest_FinancePreference(AllowAddFinancePlanSubOwners: false,
                    MaxNumberOfSubFinancePlanSubOwners: 0, AllowAddFinancePlanReviewers: true,
                    MaxNumberOfFinancePlanReviewers: 2),
                NotificationPreference: new UpdatePreferenceRequest_NotificationPreference(
                    SendFinanceReportEnabled: true, SendFinanceReportInterval: 5),
                GeneralPreference: new UpdatePreferenceRequest_GeneralPreference(UseDarkMode: true)));

        _preferenceRepositoryMock
            .Setup(expression: x => x.GetAsync(new PreferenceFilter(null, _userId, true, true, true, true),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: (Preference?)null);

        // Act
        // Assert
        ConflictException? exception = Assert.ThrowsAsync<ConflictException>(code: () =>
            TestCandidate.HandleAsync(command: command, cancellationToken: It.IsAny<CancellationToken>()));

        string expectedExceptionMessage =
            $"User preferences for user with id {command.PreferenceOrUserId} or with own id: {command.PreferenceOrUserId} haven't been found.";

        exception?.Message.Should().Be(expected: expectedExceptionMessage);

        _preferenceRepositoryMock.Verify(
            expression: x => x.GetAsync(new PreferenceFilter(null, _userId, true, true, true, true),
                It.IsAny<CancellationToken>()), times: Times.Once);

        _preferenceRepositoryMock.Verify(expression: x => x.UpdateAsync(_preference, It.IsAny<CancellationToken>()),
            times: Times.Never);

        _messageBrokerMock.Verify(
            expression: x =>
                x.PublishAsync(It.IsAny<GeneralPreferenceUpdatedIntegrationEvent>(), It.IsAny<CancellationToken>()),
            times: Times.Never);

        _messageBrokerMock.Verify(
            expression: x =>
                x.PublishAsync(It.IsAny<FinancePreferenceUpdatedIntegrationEvent>(), It.IsAny<CancellationToken>()),
            times: Times.Never());

        _messageBrokerMock.Verify(
            expression: x => x.PublishAsync(It.IsAny<NotificationPreferenceUpdatedIntegrationEvent>(),
                It.IsAny<CancellationToken>()), times: Times.Never());
    }
}