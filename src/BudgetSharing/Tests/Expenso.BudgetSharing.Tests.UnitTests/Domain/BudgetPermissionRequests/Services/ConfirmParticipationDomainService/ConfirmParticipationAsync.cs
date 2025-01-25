using Expenso.BudgetSharing.Domain.BudgetPermissionRequests;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Events;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects;
using Expenso.BudgetSharing.Domain.BudgetPermissions;
using Expenso.BudgetSharing.Domain.BudgetPermissions.Events;
using Expenso.Shared.System.Types.Exceptions;
using Expenso.Shared.System.Types.Exceptions.Models;
using Expenso.Shared.System.Types.Messages.Interfaces;
using Expenso.Shared.Tests.Utils.UnitTests.Assertions;
using Expenso.UserPreferences.Shared.DTO.API.GetPreference.Request;
using Expenso.UserPreferences.Shared.DTO.API.GetPreference.Response;

using Moq;

using NUnit.Framework;

using Shouldly;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.BudgetPermissionRequests.Services.
    ConfirmParticipationDomainService;

[TestFixture]
internal sealed class ConfirmParticipationAsync : ConfirmParticipationDomainServiceTestBase
{
    [Test]
    public async Task Should_ConfirmRequest_And_AddPermission_InPositiveCase()
    {
        // Arrange
        _budgetPermissionRequestRepositoryMock
            .Setup(expression: x => x.GetByIdAsync(_budgetPermissionRequestId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: _budgetPermissionRequest);

        _budgetPermissionRepositoryMock
            .Setup(expression: x => x.GetByBudgetIdAsync(_budgetId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: _budgetPermission);

        _userPreferencesProxyMock
            .Setup(expression: x =>
                x.GetPreferences(
                    new GetPreferencesRequest(null, _budgetPermission.OwnerId.Value,
                        GetPreferencesRequestPreferenceIncludes.Finance), It.IsAny<IMessageContext>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: _getPreferenceResponse);

        // Act
        await TestCandidate.ConfirmParticipantAsync(budgetPermissionRequestId: _budgetPermissionRequestId.Value,
            cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        _budgetPermissionRequest.StatusTracker.Status.ShouldBe(expected: BudgetPermissionRequestStatus.Confirmed);

        _budgetPermission.Permissions.ShouldContainSingle(predicate: x =>
            x.ParticipantId == _budgetPermissionRequest.ParticipantId &&
            x.PermissionType == _budgetPermissionRequest.PermissionType);

        AssertDomainEventPublished(aggregateRoot: _budgetPermissionRequest, expectedDomainEvents:
        [
            new BudgetPermissionRequestConfirmedEvent(MessageContext: MessageContextFactoryMock.Object.Current(),
                BudgetCode: _budgetPermissionRequest.BudgetCode, OwnerId: _budgetPermissionRequest.OwnerId,
                ParticipantId: _budgetPermissionRequest.ParticipantId,
                PermissionType: _budgetPermissionRequest.PermissionType)
        ]);

        AssertDomainEventPublished(aggregateRoot: _budgetPermission, expectedDomainEvents:
        [
            new BudgetPermissionGrantedEvent(MessageContext: MessageContextFactoryMock.Object.Current(),
                BudgetCode: _budgetPermissionRequest.BudgetCode, OwnerId: _budgetPermission.OwnerId,
                ParticipantId: _budgetPermissionRequest.ParticipantId,
                PermissionType: _budgetPermissionRequest.PermissionType)
        ]);
    }

    [Test]
    public async Task Should_ThrowNotFoundException_When_BudgetPermissionRequest_IsNotFound()
    {
        // Arrange
        _budgetPermissionRequestRepositoryMock
            .Setup(expression: x => x.GetByIdAsync(_budgetPermissionRequestId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: null);

        // Act
        Func<Task> action = () => TestCandidate.ConfirmParticipantAsync(
            budgetPermissionRequestId: _budgetPermissionRequestId.Value,
            cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        NotFoundException? exception = await action.ShouldThrowAsync<NotFoundException>();

        exception.Message.ShouldBe(
            expected: $"{nameof(BudgetPermissionRequest)} with ID {_budgetPermissionRequestId} hasn't been found.");

        exception.ResourceName.ShouldBe(expected: nameof(BudgetPermissionRequest));
        exception.IdentifierType.ShouldBe(expected: IdentifierType.PrimaryId());
        exception.Identifier.ShouldBe(expected: _budgetPermissionRequestId.Value);
    }

    [Test]
    public async Task Should_ThrowNotFoundException_When_BudgetPermission_IsNotFound()
    {
        // Arrange
        _budgetPermissionRequestRepositoryMock
            .Setup(expression: x => x.GetByIdAsync(_budgetPermissionRequestId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: _budgetPermissionRequest);

        _budgetPermissionRepositoryMock
            .Setup(expression: x => x.GetByBudgetIdAsync(_budgetId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: null);

        // Act
        Func<Task> action = () => TestCandidate.ConfirmParticipantAsync(
            budgetPermissionRequestId: _budgetPermissionRequestId.Value,
            cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        NotFoundException? exception = await action.ShouldThrowAsync<NotFoundException>();

        exception.Message.ShouldBe(
            expected: $"{nameof(BudgetPermission)} with Budget ID {_budgetId} hasn't been found.");

        exception.ResourceName.ShouldBe(expected: nameof(BudgetPermission));
        exception.IdentifierType.ShouldBe(expected: IdentifierType.Custom(value: "Budget ID"));
        exception.Identifier.ShouldBe(expected: _budgetId);
    }

    [Test]
    public async Task Should_ThrowNotFoundException_When_FinancePreferences_IsNotFound()
    {
        // Arrange
        _budgetPermissionRequestRepositoryMock
            .Setup(expression: x => x.GetByIdAsync(_budgetPermissionRequestId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: _budgetPermissionRequest);

        _budgetPermissionRepositoryMock
            .Setup(expression: x => x.GetByBudgetIdAsync(_budgetId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: _budgetPermission);

        _userPreferencesProxyMock
            .Setup(expression: x =>
                x.GetPreferences(
                    new GetPreferencesRequest(null, _budgetPermission.OwnerId.Value,
                        GetPreferencesRequestPreferenceIncludes.Finance), It.IsAny<IMessageContext>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: null);

        // Act
        Func<Task> action = () => TestCandidate.ConfirmParticipantAsync(
            budgetPermissionRequestId: _budgetPermissionRequestId.Value,
            cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        NotFoundException? exception = await action.ShouldThrowAsync<NotFoundException>();

        exception.Message.ShouldBe(
            expected: $"FinancePreference with User ID {_budgetPermission.OwnerId} hasn't been found.");

        exception.ResourceName.ShouldBe(expected: "FinancePreference");
        exception.IdentifierType.ShouldBe(expected: IdentifierType.Custom(value: "User ID"));
        exception.Identifier.ShouldBe(expected: _budgetPermission.OwnerId);
    }

    [Test]
    public async Task Should_ThrowDomainRuleValidationException_When_BusinessRulesAreNotMet()
    {
        // Arrange
        _budgetPermissionRequestRepositoryMock
            .Setup(expression: x => x.GetByIdAsync(_budgetPermissionRequestId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: _budgetPermissionRequest);

        _budgetPermissionRepositoryMock
            .Setup(expression: x => x.GetByBudgetIdAsync(_budgetId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: _budgetPermission);

        _userPreferencesProxyMock
            .Setup(expression: x =>
                x.GetPreferences(
                    new GetPreferencesRequest(null, _budgetPermission.OwnerId.Value,
                        GetPreferencesRequestPreferenceIncludes.Finance), It.IsAny<IMessageContext>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: _getPreferenceResponse with
            {
                FinancePreference = new GetPreferencesResponseFinancePreference(AllowAddFinancePlanSubOwners: false,
                    MaxNumberOfSubFinancePlanSubOwners: 0, AllowAddFinancePlanReviewers: false,
                    MaxNumberOfFinancePlanReviewers: 0)
            });

        // Act
        Func<Task> action = () => TestCandidate.ConfirmParticipantAsync(
            budgetPermissionRequestId: _budgetPermissionRequestId.Value,
            cancellationToken: It.IsAny<CancellationToken>());

        // Assert

        await action.AssertDomainRuleValidationExceptionAsync(
            expectedDetails:
            $"Permission of type {_budgetPermissionRequest.PermissionType} can't be assigned to budget with ID {_budgetPermission.BudgetId}, because permission type is not valid or budget owner with ID {_budgetPermission.OwnerId} don't allow any or more participants.");
    }
}