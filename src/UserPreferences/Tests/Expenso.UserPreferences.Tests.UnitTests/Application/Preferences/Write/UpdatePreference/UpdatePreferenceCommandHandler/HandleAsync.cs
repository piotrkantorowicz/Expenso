using Expenso.Shared.System.Types.Exceptions;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.UpdatePreference;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.UpdatePreference.DTO.Request;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories.Filters;
using Expenso.UserPreferences.Shared.DTO.MessageBus.UpdatePreference.FinancePreferences;
using Expenso.UserPreferences.Shared.DTO.MessageBus.UpdatePreference.GeneralPreferences;
using Expenso.UserPreferences.Shared.DTO.MessageBus.UpdatePreference.NotificationPreferences;

namespace Expenso.UserPreferences.Tests.UnitTests.Application.Preferences.Write.UpdatePreference.
    UpdatePreferenceCommandHandler;

[TestFixture]
internal sealed class HandleAsync : UpdatePreferenceCommandHandlerTestBase
{
    [Test]
    public async Task Should_UpdatePreference()
    {
        // Arrange
        UpdatePreferenceCommand command = new(MessageContext: MessageContextFactoryMock.Object.Current(),
            PreferenceId: _id, Payload: new UpdatePreferenceRequest(
                FinancePreference: new UpdatePreferenceRequest_FinancePreference(AllowAddFinancePlanSubOwners: false,
                    MaxNumberOfSubFinancePlanSubOwners: 0, AllowAddFinancePlanReviewers: true,
                    MaxNumberOfFinancePlanReviewers: 2),
                NotificationPreference: new UpdatePreferenceRequest_NotificationPreference(
                    SendFinanceReportEnabled: true, SendFinanceReportInterval: 5),
                GeneralPreference: new UpdatePreferenceRequest_GeneralPreference(UseDarkMode: true)));

        PreferenceQuerySpecification preferenceQuerySpecification =
            new(PreferenceId: _id, UseTracking: true, PreferenceType: PreferenceTypes.All);

        _preferenceRepositoryMock
            .Setup(expression: x => x.GetAsync(preferenceQuerySpecification, It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: _preference);

        _preferenceRepositoryMock.Setup(expression: x => x.UpdateAsync(_preference, It.IsAny<CancellationToken>()));

        // Act
        await TestCandidate.HandleAsync(command: command, cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        _preferenceRepositoryMock.Verify(
            expression: x => x.GetAsync(preferenceQuerySpecification, It.IsAny<CancellationToken>()),
            times: Times.Once);

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
            PreferenceId: _id, Payload: new UpdatePreferenceRequest(
                FinancePreference: new UpdatePreferenceRequest_FinancePreference(AllowAddFinancePlanSubOwners: false,
                    MaxNumberOfSubFinancePlanSubOwners: 0, AllowAddFinancePlanReviewers: true,
                    MaxNumberOfFinancePlanReviewers: 2),
                NotificationPreference: new UpdatePreferenceRequest_NotificationPreference(
                    SendFinanceReportEnabled: true, SendFinanceReportInterval: 5),
                GeneralPreference: new UpdatePreferenceRequest_GeneralPreference(UseDarkMode: true)));

        PreferenceQuerySpecification preferenceQuerySpecification =
            new(PreferenceId: _id, UseTracking: true, PreferenceType: PreferenceTypes.All);

        _preferenceRepositoryMock
            .Setup(expression: x => x.GetAsync(preferenceQuerySpecification, It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: null);

        // Act
        // Assert
        Func<Task> act = () =>
            TestCandidate.HandleAsync(command: command, cancellationToken: It.IsAny<CancellationToken>());

        act
            .Should()
            .ThrowAsync<ConflictException>()
            .WithMessage(
                expectedWildcardPattern:
                $"User preferences for user with ID {command.PreferenceId} or with own ID: {command.PreferenceId} haven't been found.");

        _preferenceRepositoryMock.Verify(
            expression: x => x.GetAsync(preferenceQuerySpecification, It.IsAny<CancellationToken>()),
            times: Times.Once);

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