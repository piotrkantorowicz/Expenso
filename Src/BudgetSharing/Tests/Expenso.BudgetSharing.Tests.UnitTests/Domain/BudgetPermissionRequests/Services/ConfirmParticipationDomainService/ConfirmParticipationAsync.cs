using System.Text;

using Expenso.BudgetSharing.Domain.BudgetPermissionRequests;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Events;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects;
using Expenso.BudgetSharing.Domain.BudgetPermissions;
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
            .Setup(x => x.GetByIdAsync(_budgetPermissionRequestId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_budgetPermissionRequest);

        _budgetPermissionRepositoryMock
            .Setup(x => x.GetByBudgetIdAsync(_budgetId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_budgetPermission);

        _userPreferencesProxyMock
            .Setup(x => x.GetUserPreferencesAsync(_budgetPermission.OwnerId.Value, true, false, false,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(_getPreferenceResponse);

        // Act
        await TestCandidate.ConfirmParticipationAsync(_budgetPermissionRequestId.Value, It.IsAny<CancellationToken>());

        // Assert
        _budgetPermissionRequest.Status.Should().Be(BudgetPermissionRequestStatus.Confirmed);

        _budgetPermission
            .Permissions.Should()
            .ContainSingle(x =>
                x.ParticipantId == _budgetPermissionRequest.ParticipantId &&
                x.PermissionType == _budgetPermissionRequest.PermissionType);

        AssertDomainEventPublished(_budgetPermissionRequest, new IDomainEvent[]
        {
            new BudgetPermissionRequestConfirmedEvent(MessageContextFactoryMock.Object.Current(),
                _budgetPermissionRequest.BudgetId, _budgetPermissionRequest.ParticipantId,
                _budgetPermissionRequest.PermissionType)
        });

        AssertDomainEventPublished(_budgetPermission, new IDomainEvent[]
        {
            new BudgetPermissionGrantedEvent(MessageContextFactoryMock.Object.Current(), _budgetPermission.Id,
                _budgetPermission.BudgetId, _budgetPermissionRequest.ParticipantId,
                _budgetPermissionRequest.PermissionType)
        });
    }

    [Test]
    public async Task Should_ThrowNotFoundException_When_BudgetPermissionRequest_IsNotFound()
    {
        // Arrange
        _budgetPermissionRequestRepositoryMock
            .Setup(x => x.GetByIdAsync(_budgetPermissionRequestId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((BudgetPermissionRequest?)null);

        // Act
        Func<Task> act = async () =>
            await TestCandidate.ConfirmParticipationAsync(_budgetPermissionRequestId.Value,
                It.IsAny<CancellationToken>());

        // Assert
        await act
            .Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage($"Budget permission request with id {_budgetPermissionRequestId} hasn't been found.");
    }

    [Test]
    public async Task Should_ThrowNotFoundException_When_BudgetPermission_IsNotFound()
    {
        // Arrange
        _budgetPermissionRequestRepositoryMock
            .Setup(x => x.GetByIdAsync(_budgetPermissionRequestId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_budgetPermissionRequest);

        _budgetPermissionRepositoryMock
            .Setup(x => x.GetByBudgetIdAsync(_budgetId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((BudgetPermission?)null);

        // Act
        Func<Task> act = async () =>
            await TestCandidate.ConfirmParticipationAsync(_budgetPermissionRequestId.Value,
                It.IsAny<CancellationToken>());

        // Assert
        await act
            .Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage($"Budget permission with id {_budgetPermissionRequest.BudgetId} hasn't been found.");
    }

    [Test]
    public async Task Should_ThrowNotFoundException_When_FinancePreferences_IsNotFound()
    {
        // Arrange
        _budgetPermissionRequestRepositoryMock
            .Setup(x => x.GetByIdAsync(_budgetPermissionRequestId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_budgetPermissionRequest);

        _budgetPermissionRepositoryMock
            .Setup(x => x.GetByBudgetIdAsync(_budgetId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_budgetPermission);

        _userPreferencesProxyMock
            .Setup(x => x.GetUserPreferencesAsync(_budgetPermission.OwnerId.Value, true, false, false,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((GetPreferenceResponse?)null);

        // Act
        Func<Task> act = async () =>
            await TestCandidate.ConfirmParticipationAsync(_budgetPermissionRequestId.Value,
                It.IsAny<CancellationToken>());

        // Assert
        await act
            .Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage($"Finance preferences for user {_budgetPermission.OwnerId} haven't been found.");
    }

    [Test]
    public async Task Should_ThrowDomainRuleValidationException_When_BusinessRulesAreNotMet()
    {
        // Arrange
        _budgetPermissionRequestRepositoryMock
            .Setup(x => x.GetByIdAsync(_budgetPermissionRequestId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_budgetPermissionRequest);

        _budgetPermissionRepositoryMock
            .Setup(x => x.GetByBudgetIdAsync(_budgetId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_budgetPermission);

        _userPreferencesProxyMock
            .Setup(x => x.GetUserPreferencesAsync(_budgetPermission.OwnerId.Value, true, false, false,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(_getPreferenceResponse with
            {
                FinancePreference = new GetPreferenceResponse_FinancePreference(false, 0, false, 0)
            });

        // Act
        Func<Task> act = async () =>
            await TestCandidate.ConfirmParticipationAsync(_budgetPermissionRequestId.Value,
                It.IsAny<CancellationToken>());

        // Assert
        await act
            .Should()
            .ThrowAsync<DomainRuleValidationException>()
            .WithMessage(new StringBuilder()
                .Append("Permission of type ")
                .Append(_budgetPermissionRequest.PermissionType)
                .Append(" can't be assigned to budget with id ")
                .Append(_budgetPermission.BudgetId)
                .Append(", because permission type is not valid or budget owner with id: ")
                .Append(_budgetPermission.OwnerId)
                .Append(" don't allow any or more participants.")
                .ToString());
    }
}