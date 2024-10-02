using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects;
using Expenso.Shared.System.Types.Exceptions;

using FluentAssertions;

using Moq;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.BudgetPermissionRequests.Services.
    BudgetPermissionRequestExpirationDomainService;

internal sealed class MarkBudgetPermissionRequestAsExpireAsync : BudgetPermissionRequestExpirationDomainServiceTestBase
{
    [Test]
    public async Task Should_MarkBudgetPermissionRequestAsExpired()
    {
        // Arrange
        BudgetPermissionRequestId budgetPermissionRequestId = BudgetPermissionRequestId.New(value: Guid.NewGuid());

        _budgetPermissionRequestRepositoryMock
            .Setup(expression: repo => repo.GetByIdAsync(budgetPermissionRequestId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: _budgetPermissionRequest);

        // Act
        await TestCandidate.MarkBudgetPermissionRequestAsExpireAsync(
            budgetPermissionRequestId: budgetPermissionRequestId.Value, cancellationToken: CancellationToken.None);

        // Assert
        _budgetPermissionRequestRepositoryMock.Verify(
            expression: repo => repo.UpdateAsync(_budgetPermissionRequest, It.IsAny<CancellationToken>()),
            times: Times.Once);
    }

    [Test]
    public void Should_ThrowNotFoundException_When_BudgetPermissionRequestIsNull()
    {
        // Arrange
        BudgetPermissionRequestId budgetPermissionRequestId = BudgetPermissionRequestId.New(value: Guid.NewGuid());

        _budgetPermissionRequestRepositoryMock
            .Setup(expression: repo => repo.GetByIdAsync(budgetPermissionRequestId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: null);

        // Act
        Func<Task> action = async () =>
            await TestCandidate.MarkBudgetPermissionRequestAsExpireAsync(
                budgetPermissionRequestId: budgetPermissionRequestId.Value, cancellationToken: CancellationToken.None);

        // Assert
        action
            .Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage(
                expectedWildcardPattern:
                $"Budget permission request with id {budgetPermissionRequestId} hasn't been found.");
    }
}