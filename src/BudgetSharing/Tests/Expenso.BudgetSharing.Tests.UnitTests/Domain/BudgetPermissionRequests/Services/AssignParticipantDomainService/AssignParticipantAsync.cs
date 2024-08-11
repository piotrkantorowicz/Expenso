using Expenso.BudgetSharing.Domain.BudgetPermissionRequests;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Events;
using Expenso.Shared.Domain.Types.Events;
using Expenso.Shared.Domain.Types.Exceptions;
using Expenso.Shared.System.Types.Exceptions;

using FluentAssertions;

using Moq;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.BudgetPermissionRequests.Services.AssignParticipantDomainService;

internal sealed class AssignParticipantAsync : AssignParticipantDomainServiceTestBase
{
    [Test]
    public async Task Should_CreateBudgetPermissionRequest_InPositiveCase()
    {
        // Arrange
        _iamProxyMock
            .Setup(expression: x => x.GetUserByEmailAsync(_email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: _getUserResponse);

        // Act
        BudgetPermissionRequest budgetPermissionRequest = await TestCandidate.AssignParticipantAsync(
            budgetId: _budgetId.Value, email: _email, permissionType: _permissionType, expirationDays: ExpirationDays,
            cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        budgetPermissionRequest.Id.Should().NotBeNull();
        budgetPermissionRequest.BudgetId.Should().Be(expected: _budgetId);
        budgetPermissionRequest.ParticipantId.Should().Be(expected: _participantId);
        budgetPermissionRequest.PermissionType.Should().Be(expected: _permissionType);

        budgetPermissionRequest
            .ExpirationDate?.Value.Should()
            .BeCloseTo(nearbyTime: _clockMock.Object.UtcNow.AddDays(days: ExpirationDays),
                precision: TimeSpan.FromMilliseconds(value: 500));

        AssertDomainEventPublished(aggregateRoot: budgetPermissionRequest, expectedDomainEvents: new IDomainEvent[]
        {
            new BudgetPermissionRequestedEvent(MessageContext: MessageContextFactoryMock.Object.Current(),
                BudgetId: budgetPermissionRequest.BudgetId, ParticipantId: budgetPermissionRequest.ParticipantId,
                PermissionType: budgetPermissionRequest.PermissionType)
        });
    }

    [Test]
    public void Should_ThrowNotFoundException_When_UserIsNotFound()
    {
        // Arrange
        _iamProxyMock
            .Setup(expression: x => x.GetUserByEmailAsync(_email, It.IsAny<CancellationToken>()))
            .ThrowsAsync(exception: new NotFoundException(message: $"User with email {_email} not found."));

        // Act
        // Assert
        NotFoundException? exception = Assert.ThrowsAsync<NotFoundException>(code: () =>
            TestCandidate.AssignParticipantAsync(budgetId: _budgetId.Value, email: _email,
                permissionType: _permissionType, expirationDays: ExpirationDays,
                cancellationToken: It.IsAny<CancellationToken>()));

        string expectedExceptionMessage = $"User with email {_email} not found.";
        exception?.Message.Should().Be(expected: expectedExceptionMessage);
    }

    [Test]
    public void Should_ThrowDomainRuleValidationException_When_UnknownUserIdentifierReturned()
    {
        // Arrange
        _iamProxyMock
            .Setup(expression: x => x.GetUserByEmailAsync(_email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: _getUserResponse with
            {
                UserId = "db9aUuZIcbRkWg3"
            });

        // Act
        // Assert
        DomainRuleValidationException? exception = Assert.ThrowsAsync<DomainRuleValidationException>(code: () =>
            TestCandidate.AssignParticipantAsync(budgetId: _budgetId.Value, email: _email,
                permissionType: _permissionType, expirationDays: ExpirationDays,
                cancellationToken: It.IsAny<CancellationToken>()));

        string expectedExceptionMessage =
            $"Budget participant must be the existing system user, but provided user with id {_getUserResponse.Email} hasn't been found in the system";

        exception?.Message.Should().Be(expected: expectedExceptionMessage);
    }
}