using Expenso.BudgetSharing.Domain.BudgetPermissionRequests;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects;
using Expenso.Shared.System.Types.Exceptions;
using Expenso.Shared.System.Types.Exceptions.Models;

using FluentAssertions;

using Moq;

using NUnit.Framework;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.BudgetPermissionRequests.Services.
    BudgetPermissionRequestExpirationDomainService;

[TestFixture]
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
    public async Task Should_ThrowNotFoundException_When_BudgetPermissionRequestIsNull()
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
        await action
            .Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage(
                expectedWildcardPattern:
                $"{nameof(BudgetPermissionRequest)} with ID {budgetPermissionRequestId} hasn't been found.")
            .Where(exceptionExpression: x => x.ResourceName == nameof(BudgetPermissionRequest) &&
                                             x.IdentifierType == IdentifierType.PrimaryId() &&
                                             (BudgetPermissionRequestId?)x.Identifier == budgetPermissionRequestId);
    }
}