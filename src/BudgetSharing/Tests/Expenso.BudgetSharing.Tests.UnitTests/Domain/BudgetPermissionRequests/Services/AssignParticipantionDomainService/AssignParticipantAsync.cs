using Expenso.BudgetSharing.Domain.BudgetPermissionRequests;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Events;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.Shared.Domain.Types.Exceptions;
using Expenso.Shared.System.Types.Exceptions;

using FluentAssertions;

using Moq;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.BudgetPermissionRequests.Services.
    AssignParticipantionDomainService;

internal sealed class AssignParticipantAsync : AssignParticipantDomainServiceTestBase
{
    [Test]
    public async Task Should_CreateBudgetPermissionRequest_InPositiveCase()
    {
        // Arrange
        _iamProxyMock
            .Setup(expression: x => x.GetUserByEmailAsync(_email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: _getUserResponse);

        _budgetPermissionRepositoryMock
            .Setup(expression: x => x.GetByBudgetIdAsync(_budgetId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: _budgetPermission);

        // Act
        BudgetPermissionRequest budgetPermissionRequest = await TestCandidate.AssignParticipantAsync(
            budgetId: _budgetId, email: _email, permissionType: _permissionType, expirationDays: ExpirationDays,
            cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        budgetPermissionRequest.Id.Should().NotBeNull();
        budgetPermissionRequest.BudgetId.Should().Be(expected: _budgetId);
        budgetPermissionRequest.ParticipantId.Should().Be(expected: _participantId);
        budgetPermissionRequest.PermissionType.Should().Be(expected: _permissionType);

        budgetPermissionRequest
            .StatusTracker.ExpirationDate.Value.Should()
            .BeCloseTo(nearbyTime: _clockMock.Object.UtcNow.AddDays(days: ExpirationDays),
                precision: TimeSpan.FromMilliseconds(value: 500));

        AssertDomainEventPublished(aggregateRoot: budgetPermissionRequest, expectedDomainEvents:
        [
            new BudgetPermissionRequestedEvent(MessageContext: MessageContextFactoryMock.Object.Current(),
                OwnerId: budgetPermissionRequest.OwnerId, ParticipantId: budgetPermissionRequest.ParticipantId,
                PermissionType: budgetPermissionRequest.PermissionType,
                SubmissionDate: budgetPermissionRequest.StatusTracker.SubmissionDate)
        ]);
    }

    [Test]
    public void Should_ThrowDomainRuleValidationException_When_BudgetPermissionHasNotExists()
    {
        // Arrange
        _budgetPermissionRepositoryMock
            .Setup(expression: x => x.GetByBudgetIdAsync(_budgetId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: null);

        // Act
        Func<Task> action = () => TestCandidate.AssignParticipantAsync(budgetId: _budgetId, email: _email,
            permissionType: _permissionType, expirationDays: ExpirationDays,
            cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        action
            .Should()
            .ThrowAsync<DomainRuleValidationException>()
            .WithMessage(
                expectedWildcardPattern:
                $"Unable to create budget permission request for not existent budget permission. Budget {_budgetId}.");
    }

    [Test]
    public void Should_ThrowNotFoundException_When_UserIsNotFound()
    {
        // Arrange
        _budgetPermissionRepositoryMock
            .Setup(expression: x => x.GetByBudgetIdAsync(_budgetId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: _budgetPermission);

        _iamProxyMock
            .Setup(expression: x => x.GetUserByEmailAsync(_email, It.IsAny<CancellationToken>()))
            .ThrowsAsync(exception: new NotFoundException(message: $"User with email {_email} not found"));

        // Act
        Func<Task> action = () => TestCandidate.AssignParticipantAsync(budgetId: _budgetId, email: _email,
            permissionType: _permissionType, expirationDays: ExpirationDays,
            cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        action
            .Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage(expectedWildcardPattern: $"User with email {_email} not found.");
    }

    [Test]
    public void Should_ThrowDomainRuleValidationException_When_UnknownUserIdentifierReturned()
    {
        // Arrange
        _budgetPermissionRepositoryMock
            .Setup(expression: x => x.GetByBudgetIdAsync(_budgetId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: _budgetPermission);

        _iamProxyMock
            .Setup(expression: x => x.GetUserByEmailAsync(_email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: _getUserResponse with
            {
                UserId = "db9aUuZIcbRkWg3"
            });

        // Act
        Func<Task> action = () => TestCandidate.AssignParticipantAsync(budgetId: _budgetId, email: _email,
            permissionType: _permissionType, expirationDays: ExpirationDays,
            cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        action
            .Should()
            .ThrowAsync<DomainRuleValidationException>()
            .WithMessage(
                expectedWildcardPattern:
                $"Budget participant must be the existing system user, but provided user with ID {_getUserResponse.Email} hasn't been found in the system.");
    }

    [Test]
    public void Should_ThrowDomainRuleValidationException_When_MemberHasAlreadyAssignedToRequestedBudget()
    {
        // Arrange
        _iamProxyMock
            .Setup(expression: x => x.GetUserByEmailAsync(_email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: _getUserResponse);

        _budgetPermission.AddPermission(participantId: _participantId, permissionType: _permissionType);

        _budgetPermissionRepositoryMock
            .Setup(expression: x => x.GetByBudgetIdAsync(_budgetId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: _budgetPermission);

        // Act
        Func<Task> action = () => TestCandidate.AssignParticipantAsync(budgetId: _budgetId, email: _email,
            permissionType: _permissionType, expirationDays: ExpirationDays,
            cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        action
            .Should()
            .ThrowAsync<DomainRuleValidationException>()
            .WithMessage(
                expectedWildcardPattern:
                $"Participant {_participantId} has already budget permission for budget {_budgetPermission.BudgetId}.");
    }

    [Test, TestCaseSource(sourceName: nameof(PermissionTypes))]
    public void
        Should_ThrowDomainRuleValidationException_When_MemberHasAlreadyOpenedBudgetPermissionRequestsWithSamePermission(
            PermissionType permissionType)
    {
        // Arrange
        _iamProxyMock
            .Setup(expression: x => x.GetUserByEmailAsync(_email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: _getUserResponse);

        _budgetPermissionRepositoryMock
            .Setup(expression: x => x.GetByBudgetIdAsync(_budgetId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: _budgetPermission);

        BudgetPermissionRequest otherBudgetPermissionRequest = BudgetPermissionRequest.Create(budgetId: _budgetId,
            ownerId: _ownerId, personId: _participantId, permissionType: permissionType, expirationDays: 10,
            clock: _clockMock.Object);

        _budgetPermissionRequestRepositoryMock
            .Setup(expression: x =>
                x.GetUncompletedByPersonIdAsync(_budgetId, _participantId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: [otherBudgetPermissionRequest]);

        // Act
        Func<Task> action = () => TestCandidate.AssignParticipantAsync(budgetId: _budgetId, email: _email,
            permissionType: permissionType, expirationDays: ExpirationDays,
            cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        action
            .Should()
            .ThrowAsync<DomainRuleValidationException>()
            .WithMessage(
                expectedWildcardPattern:
                $"Member has already opened requests {otherBudgetPermissionRequest.Id} for this budget {_budgetId} with same permission {permissionType}.");
    }

    [Test]
    public void
        Should_CreateBudgetPermissionRequest_When_UserisAssigningAsOwnerButHasOpenedRequestsForReviewerOrSubOwner()
    {
        // Arrange
        _iamProxyMock
            .Setup(expression: x => x.GetUserByEmailAsync(_email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: _getUserResponse);

        _budgetPermissionRepositoryMock
            .Setup(expression: x => x.GetByBudgetIdAsync(_budgetId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: _budgetPermission);

        IReadOnlyCollection<BudgetPermissionRequest> otherBudgetPermissionRequests =
        [
            BudgetPermissionRequest.Create(budgetId: _budgetId, ownerId: _ownerId, personId: _participantId,
                permissionType: PermissionType.Reviewer, expirationDays: 4, clock: _clockMock.Object),
            BudgetPermissionRequest.Create(budgetId: _budgetId, ownerId: _ownerId, personId: _participantId,
                permissionType: PermissionType.SubOwner, expirationDays: 7, clock: _clockMock.Object)
        ];

        _budgetPermissionRequestRepositoryMock
            .Setup(expression: x =>
                x.GetUncompletedByPersonIdAsync(_budgetId, _participantId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: otherBudgetPermissionRequests);

        // Act
        // Assert
        Assert.DoesNotThrowAsync(code: () => TestCandidate.AssignParticipantAsync(budgetId: _budgetId, email: _email,
            permissionType: PermissionType.Owner, expirationDays: ExpirationDays,
            cancellationToken: It.IsAny<CancellationToken>()));
    }

    [Test, TestCaseSource(sourceName: nameof(NoOwnerPermissionTypes))]
    public void
        Should_CreateBudgetPermissionRequest_When_UserisAssigningAsSubOwnerOrReviewerButHasOpenedRequestsForOwner(
            PermissionType permissionType)
    {
        // Arrange
        _iamProxyMock
            .Setup(expression: x => x.GetUserByEmailAsync(_email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: _getUserResponse);

        _budgetPermissionRepositoryMock
            .Setup(expression: x => x.GetByBudgetIdAsync(_budgetId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: _budgetPermission);

        _budgetPermissionRequestRepositoryMock
            .Setup(expression: x =>
                x.GetUncompletedByPersonIdAsync(_budgetId, _participantId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(value:
            [
                BudgetPermissionRequest.Create(budgetId: _budgetId, ownerId: _ownerId, personId: _participantId,
                    permissionType: PermissionType.Owner, expirationDays: 4, clock: _clockMock.Object)
            ]);

        // Act
        // Assert
        Assert.DoesNotThrowAsync(code: () => TestCandidate.AssignParticipantAsync(budgetId: _budgetId, email: _email,
            permissionType: permissionType, expirationDays: ExpirationDays,
            cancellationToken: It.IsAny<CancellationToken>()));
    }
}