using System.Text;

using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Events;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects;
using Expenso.BudgetSharing.Domain.BudgetPermissions.Events;
using Expenso.Shared.Domain.Types.Events;
using Expenso.Shared.Domain.Types.Exceptions;
using Expenso.Shared.System.Types.Exceptions;
using Expenso.UserPreferences.Proxy.DTO.API.GetPreference.Response;

using FluentAssertions;

using Moq;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.BudgetPermissionRequests.Services.
    ConfirmParticipationDomainService;

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
            .Setup(expression: x => x.GetUserPreferencesAsync(_budgetPermission.OwnerId.Value, true, false, false,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: _getPreferenceResponse);

        // Act
        await TestCandidate.ConfirmParticipantAsync(budgetPermissionRequestId: _budgetPermissionRequestId.Value,
            cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        _budgetPermissionRequest.Status.Should().Be(expected: BudgetPermissionRequestStatus.Confirmed);

        _budgetPermission
            .Permissions.Should()
            .ContainSingle(predicate: x =>
                x.ParticipantId == _budgetPermissionRequest.ParticipantId &&
                x.PermissionType == _budgetPermissionRequest.PermissionType);

        AssertDomainEventPublished(aggregateRoot: _budgetPermissionRequest, expectedDomainEvents: new IDomainEvent[]
        {
            new BudgetPermissionRequestConfirmedEvent(MessageContext: MessageContextFactoryMock.Object.Current(),
                OwnerId: _budgetPermissionRequest.OwnerId, ParticipantId: _budgetPermissionRequest.ParticipantId,
                PermissionType: _budgetPermissionRequest.PermissionType)
        });

        AssertDomainEventPublished(aggregateRoot: _budgetPermission, expectedDomainEvents: new IDomainEvent[]
        {
            new BudgetPermissionGrantedEvent(MessageContext: MessageContextFactoryMock.Object.Current(),
                OwnerId: _budgetPermission.OwnerId, ParticipantId: _budgetPermissionRequest.ParticipantId,
                PermissionType: _budgetPermissionRequest.PermissionType)
        });
    }

    [Test]
    public async Task Should_ThrowNotFoundException_When_BudgetPermissionRequest_IsNotFound()
    {
        // Arrange
        _budgetPermissionRequestRepositoryMock
            .Setup(expression: x => x.GetByIdAsync(_budgetPermissionRequestId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: null);

        // Act
        Func<Task> act = async () => await TestCandidate.ConfirmParticipantAsync(
            budgetPermissionRequestId: _budgetPermissionRequestId.Value,
            cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        await act
            .Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage(
                expectedWildcardPattern:
                $"Budget permission request with id {_budgetPermissionRequestId} hasn't been found");
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
        Func<Task> act = async () => await TestCandidate.ConfirmParticipantAsync(
            budgetPermissionRequestId: _budgetPermissionRequestId.Value,
            cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        await act
            .Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage(
                expectedWildcardPattern:
                $"Budget permission with id {_budgetPermissionRequest.BudgetId} hasn't been found");
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
            .Setup(expression: x => x.GetUserPreferencesAsync(_budgetPermission.OwnerId.Value, true, false, false,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: null);

        // Act
        Func<Task> act = async () => await TestCandidate.ConfirmParticipantAsync(
            budgetPermissionRequestId: _budgetPermissionRequestId.Value,
            cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        await act
            .Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage(
                expectedWildcardPattern:
                $"Finance preferences for user {_budgetPermission.OwnerId} haven't been found");
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
            .Setup(expression: x => x.GetUserPreferencesAsync(_budgetPermission.OwnerId.Value, true, false, false,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: _getPreferenceResponse with
            {
                FinancePreference = new GetPreferenceResponse_FinancePreference(AllowAddFinancePlanSubOwners: false,
                    MaxNumberOfSubFinancePlanSubOwners: 0, AllowAddFinancePlanReviewers: false,
                    MaxNumberOfFinancePlanReviewers: 0)
            });

        // Act
        Func<Task> act = async () => await TestCandidate.ConfirmParticipantAsync(
            budgetPermissionRequestId: _budgetPermissionRequestId.Value,
            cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        await act
            .Should()
            .ThrowAsync<DomainRuleValidationException>()
            .WithMessage(expectedWildcardPattern: new StringBuilder()
                .Append(value: "Permission of type ")
                .Append(value: _budgetPermissionRequest.PermissionType)
                .Append(value: " can't be assigned to budget with id ")
                .Append(value: _budgetPermission.BudgetId)
                .Append(value: ", because permission type is not valid or budget owner with id: ")
                .Append(value: _budgetPermission.OwnerId)
                .Append(value: " don't allow any or more participants")
                .ToString());
    }
}