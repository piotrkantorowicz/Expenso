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
            .Setup(x => x.GetUserByEmailAsync(_email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_getUserResponse);

        // Act
        BudgetPermissionRequest budgetPermissionRequest = await TestCandidate.AssignParticipantAsync(_budgetId.Value,
            _email, _permissionType, ExpirationDays, It.IsAny<CancellationToken>());

        // Assert
        budgetPermissionRequest.Id.Should().NotBeNull();
        budgetPermissionRequest.BudgetId.Should().Be(_budgetId);
        budgetPermissionRequest.ParticipantId.Should().Be(_participantId);
        budgetPermissionRequest.PermissionType.Should().Be(_permissionType);

        budgetPermissionRequest
            .ExpirationDate?.Value.Should()
            .BeCloseTo(_clockMock.Object.UtcNow.AddDays(ExpirationDays), TimeSpan.FromMilliseconds(500));

        AssertDomainEventPublished(budgetPermissionRequest, new IDomainEvent[]
        {
            new BudgetPermissionRequestedEvent(MessageContextFactoryMock.Object.Current(),
                budgetPermissionRequest.BudgetId, budgetPermissionRequest.ParticipantId,
                budgetPermissionRequest.PermissionType)
        });
    }

    [Test]
    public void Should_ThrowNotFoundException_When_UserIsNotFound()
    {
        // Arrange
        _iamProxyMock
            .Setup(x => x.GetUserByEmailAsync(_email, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new NotFoundException($"User with email {_email} not found."));

        // Act
        // Assert
        NotFoundException? exception = Assert.ThrowsAsync<NotFoundException>(() =>
            TestCandidate.AssignParticipantAsync(_budgetId.Value, _email, _permissionType, ExpirationDays,
                It.IsAny<CancellationToken>()));

        string expectedExceptionMessage = $"User with email {_email} not found.";
        exception?.Message.Should().Be(expectedExceptionMessage);
    }

    [Test]
    public void Should_ThrowDomainRuleValidationException_When_UnknownUserIdentifierReturned()
    {
        // Arrange
        _iamProxyMock
            .Setup(x => x.GetUserByEmailAsync(_email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_getUserResponse with
            {
                UserId = "db9aUuZIcbRkWg3"
            });

        // Act
        // Assert
        DomainRuleValidationException? exception = Assert.ThrowsAsync<DomainRuleValidationException>(() =>
            TestCandidate.AssignParticipantAsync(_budgetId.Value, _email, _permissionType, ExpirationDays,
                It.IsAny<CancellationToken>()));

        string expectedExceptionMessage =
            $"Budget participant must be the existing system user, but provided user with id {_getUserResponse.Email} hasn't been found in the system";

        exception?.Message.Should().Be(expectedExceptionMessage);
    }
}