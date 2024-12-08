using Expenso.Shared.System.Types.Exceptions;
using Expenso.Shared.System.Types.Exceptions.Models;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.UpdatePreference;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.UpdatePreference.DTO.Request;
using Expenso.UserPreferences.Core.Domain.Preferences.Model;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories.Specifications;
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
            PreferenceId: _id,
            Payload: new UpdatePreferenceRequest(FinancePreference: new UpdatePreferenceRequestFinancePreference(
                    AllowAddFinancePlanSubOwners: false,
                    MaxNumberOfSubFinancePlanSubOwners: 0, AllowAddFinancePlanReviewers: true,
                    MaxNumberOfFinancePlanReviewers: 2),
                NotificationPreference: new UpdatePreferenceRequestNotificationPreference(
                    SendFinanceReportEnabled: true, SendFinanceReportInterval: 5),
                GeneralPreference: new UpdatePreferenceRequestGeneralPreference(UseDarkMode: true)));

        PreferenceQuerySpecification preferenceQuerySpecification =
            new(PreferenceId: _id, UseTracking: true, Includes: PreferenceIncludes.All);

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
    public async Task Should_ThrowNotFoundException_When_PreferenceDoesNotExist()
    {
        // Arrange
        UpdatePreferenceCommand command = new(MessageContext: MessageContextFactoryMock.Object.Current(),
            PreferenceId: _id,
            Payload: new UpdatePreferenceRequest(FinancePreference: new UpdatePreferenceRequestFinancePreference(
                    AllowAddFinancePlanSubOwners: false,
                    MaxNumberOfSubFinancePlanSubOwners: 0, AllowAddFinancePlanReviewers: true,
                    MaxNumberOfFinancePlanReviewers: 2),
                NotificationPreference: new UpdatePreferenceRequestNotificationPreference(
                    SendFinanceReportEnabled: true, SendFinanceReportInterval: 5),
                GeneralPreference: new UpdatePreferenceRequestGeneralPreference(UseDarkMode: true)));

        PreferenceQuerySpecification preferenceQuerySpecification =
            new(PreferenceId: _id, UseTracking: true, Includes: PreferenceIncludes.All);

        _preferenceRepositoryMock
            .Setup(expression: x => x.GetAsync(preferenceQuerySpecification, It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: null);

        // Act
        // Assert
        Func<Task> act = () =>
            TestCandidate.HandleAsync(command: command, cancellationToken: It.IsAny<CancellationToken>());

        await act
            .Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage(
                expectedWildcardPattern:
                $"{nameof(Preference)} with query {preferenceQuerySpecification} hasn't been found.")
            .Where(exceptionExpression: x =>
                x.ResourceName == nameof(Preference) && x.IdentifierType == IdentifierType.Query() &&
                (PreferenceQuerySpecification?)x.Identifier == preferenceQuerySpecification);

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