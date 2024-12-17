using Expenso.BudgetSharing.Domain.BudgetPermissionRequests;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Events;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.IAM.Shared.DTO.GetUserByEmail.Request;
using Expenso.Shared.Domain.Types.Exceptions;
using Expenso.Shared.System.Types.Exceptions;
using Expenso.Shared.System.Types.Exceptions.Models;
using Expenso.Shared.System.Types.Messages.Interfaces;

using FluentAssertions;

using Moq;

using NUnit.Framework;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.BudgetPermissionRequests.Services.
    AssignParticipantionDomainService;

[TestFixture]
internal sealed class AssignParticipationAsync : AssignParticipationDomainServiceTestBase
{
    [Test]
    public async Task Should_CreateBudgetPermissionRequest_InPositiveCase()
    {
        // Arrange
        _iamProxyMock
            .Setup(expression: x => x.GetUserByEmailAsync(new GetUserByEmailRequest(_email),
                It.IsAny<IMessageContext>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: _getUserByEmailResponse);

        _budgetPermissionRepositoryMock
            .Setup(expression: x => x.GetByBudgetIdAsync(_budgetId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: _budgetPermission);

        _budgetPermissionRequestRepositoryMock
            .Setup(expression: x =>
                x.GetUncompletedByPersonIdAsync(_budgetId, _participantId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: []);

        // Act
        BudgetPermissionRequest budgetPermissionRequest = await TestCandidate.AssignParticipantAsync(
            budgetPermissionRequestId: _budgetPermissionRequestId, budgetId: _budgetId, email: _email,
            permissionType: _permissionType, expirationDays: ExpirationDays,
            cancellationToken: It.IsAny<CancellationToken>());

        // Assert
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
                PermissionType: budgetPermissionRequest.PermissionType, BudgetCode: budgetPermissionRequest.BudgetCode,
                SubmissionDate: budgetPermissionRequest.StatusTracker.SubmissionDate)
        ]);
    }

    [Test]
    public async Task Should_ThrowDomainRuleValidationException_When_BudgetPermissionHasNotExists()
    {
        // Arrange
        _budgetPermissionRepositoryMock
            .Setup(expression: x => x.GetByBudgetIdAsync(_budgetId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: null);

        // Act
        Func<Task> action = () => TestCandidate.AssignParticipantAsync(
            budgetPermissionRequestId: _budgetPermissionRequestId, budgetId: _budgetId, email: _email,
            permissionType: _permissionType, expirationDays: ExpirationDays,
            cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        await action
            .Should()
            .ThrowAsync<DomainRuleValidationException>()
            .WithMessage(expectedWildcardPattern: "Business rule validation failed.")
            .WithDetailsAsync(
                expectedWildcardPattern:
                $"Unable to create budget permission request for not existent budget permission. Budget {_budgetId}.");
    }

    [Test]
    public async Task Should_ThrowNotFoundException_When_UserIsNotFound()
    {
        // Arrange
        _budgetPermissionRepositoryMock
            .Setup(expression: x => x.GetByBudgetIdAsync(_budgetId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: _budgetPermission);

        _iamProxyMock
            .Setup(expression: x => x.GetUserByEmailAsync(new GetUserByEmailRequest(_email),
                It.IsAny<IMessageContext>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(exception: new NotFoundException(resourceName: "User", identifierType: IdentifierType.Email(),
                identifier: _email));

        // Act
        Func<Task> action = () => TestCandidate.AssignParticipantAsync(
            budgetPermissionRequestId: _budgetPermissionRequestId, budgetId: _budgetId, email: _email,
            permissionType: _permissionType, expirationDays: ExpirationDays,
            cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        await action
            .Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage(expectedWildcardPattern: $"User with email {_email} hasn't been found.")
            .Where(exceptionExpression: x => x.ResourceName == "User" && x.IdentifierType == IdentifierType.Email() &&
                                             (string?)x.Identifier == _email);
    }

    [Test]
    public async Task Should_ThrowDomainRuleValidationException_When_UnknownUserIdentifierReturned()
    {
        // Arrange
        _budgetPermissionRepositoryMock
            .Setup(expression: x => x.GetByBudgetIdAsync(_budgetId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: _budgetPermission);

        _iamProxyMock
            .Setup(expression: x => x.GetUserByEmailAsync(new GetUserByEmailRequest(_email),
                It.IsAny<IMessageContext>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: _getUserByEmailResponse with
            {
                UserId = "db9aUuZIcbRkWg3"
            });

        // Act
        Func<Task> action = () => TestCandidate.AssignParticipantAsync(
            budgetPermissionRequestId: _budgetPermissionRequestId, budgetId: _budgetId, email: _email,
            permissionType: _permissionType, expirationDays: ExpirationDays,
            cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        await action
            .Should()
            .ThrowAsync<DomainRuleValidationException>()
            .WithMessage(expectedWildcardPattern: "Business rule validation failed.")
            .WithDetailsAsync(
                expectedWildcardPattern:
                $"Budget participant must be the existing system user, but provided user with ID {_getUserByEmailResponse.Email} hasn't been found in the system.");
    }

    [Test]
    public async Task Should_ThrowDomainRuleValidationException_When_MemberHasAlreadyAssignedToRequestedBudget()
    {
        // Arrange
        _iamProxyMock
            .Setup(expression: x => x.GetUserByEmailAsync(new GetUserByEmailRequest(_email),
                It.IsAny<IMessageContext>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: _getUserByEmailResponse);

        _budgetPermission.AddPermission(participantId: _participantId, permissionType: _permissionType);

        _budgetPermissionRepositoryMock
            .Setup(expression: x => x.GetByBudgetIdAsync(_budgetId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: _budgetPermission);

        // Act
        Func<Task> action = () => TestCandidate.AssignParticipantAsync(
            budgetPermissionRequestId: _budgetPermissionRequestId, budgetId: _budgetId, email: _email,
            permissionType: _permissionType, expirationDays: ExpirationDays,
            cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        await action
            .Should()
            .ThrowAsync<DomainRuleValidationException>()
            .WithMessage(expectedWildcardPattern: "Business rule validation failed.")
            .WithDetailsAsync(
                expectedWildcardPattern:
                $"Participant {_participantId} has already budget permission for budget {_budgetPermission.BudgetId}.");
    }

    [Test, TestCaseSource(sourceName: nameof(PermissionTypes))]
    public async Task
        Should_ThrowDomainRuleValidationException_When_MemberHasAlreadyOpenedBudgetPermissionRequestsWithSamePermission(
            PermissionType permissionType)
    {
        // Arrange
        _iamProxyMock
            .Setup(expression: x => x.GetUserByEmailAsync(new GetUserByEmailRequest(_email),
                It.IsAny<IMessageContext>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: _getUserByEmailResponse);

        _budgetPermissionRepositoryMock
            .Setup(expression: x => x.GetByBudgetIdAsync(_budgetId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: _budgetPermission);

        BudgetPermissionRequest otherBudgetPermissionRequest = BudgetPermissionRequest.Create(
            budgetPermissionRequestId: _budgetPermissionRequestId, budgetId: _budgetId,
            budgetCode: _budgetCode, ownerId: _ownerId, personId: _participantId, permissionType: permissionType,
            expirationDate: _clockMock.Object.UtcNow.AddDays(days: 10), submissionDate: _clockMock.Object.UtcNow);

        _budgetPermissionRequestRepositoryMock
            .Setup(expression: x =>
                x.GetUncompletedByPersonIdAsync(_budgetId, _participantId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: [otherBudgetPermissionRequest]);

        // Act
        Func<Task> action = () => TestCandidate.AssignParticipantAsync(
            budgetPermissionRequestId: _budgetPermissionRequestId, budgetId: _budgetId, email: _email,
            permissionType: permissionType, expirationDays: ExpirationDays,
            cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        await action
            .Should()
            .ThrowAsync<DomainRuleValidationException>()
            .WithMessage(expectedWildcardPattern: "Business rule validation failed.")
            .WithDetailsAsync(
                expectedWildcardPattern:
                $"Member has already opened requests {otherBudgetPermissionRequest.Id} for this budget {_budgetId} with same permission {permissionType}.");
    }

    [Test]
    public async Task
        Should_CreateBudgetPermissionRequest_When_UserisAssigningAsOwnerButHasOpenedRequestsForReviewerOrSubOwner()
    {
        // Arrange
        _iamProxyMock
            .Setup(expression: x => x.GetUserByEmailAsync(new GetUserByEmailRequest(_email),
                It.IsAny<IMessageContext>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: _getUserByEmailResponse);

        _budgetPermissionRepositoryMock
            .Setup(expression: x => x.GetByBudgetIdAsync(_budgetId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: _budgetPermission);

        IReadOnlyCollection<BudgetPermissionRequest> otherBudgetPermissionRequests =
        [
            BudgetPermissionRequest.Create(budgetPermissionRequestId: _budgetPermissionRequestId, budgetId: _budgetId,
                ownerId: _ownerId, personId: _participantId,
                budgetCode: _budgetCode, permissionType: PermissionType.Reviewer,
                expirationDate: _clockMock.Object.UtcNow.AddDays(days: 4), submissionDate: _clockMock.Object.UtcNow),
            BudgetPermissionRequest.Create(budgetPermissionRequestId: _budgetPermissionRequestId, budgetId: _budgetId,
                ownerId: _ownerId, personId: _participantId,
                budgetCode: _budgetCode, permissionType: PermissionType.SubOwner,
                expirationDate: _clockMock.Object.UtcNow.AddDays(days: 7), submissionDate: _clockMock.Object.UtcNow)
        ];

        _budgetPermissionRequestRepositoryMock
            .Setup(expression: x =>
                x.GetUncompletedByPersonIdAsync(_budgetId, _participantId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: otherBudgetPermissionRequests);

        // Act
        Func<Task> action = () => TestCandidate.AssignParticipantAsync(
            budgetPermissionRequestId: _budgetPermissionRequestId, budgetId: _budgetId, email: _email,
            permissionType: PermissionType.Owner, expirationDays: ExpirationDays,
            cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        await action.Should().NotThrowAsync();
    }

    [Test, TestCaseSource(sourceName: nameof(NoOwnerPermissionTypes))]
    public async Task
        Should_CreateBudgetPermissionRequest_When_UserisAssigningAsSubOwnerOrReviewerButHasOpenedRequestsForOwner(
            PermissionType permissionType)
    {
        // Arrange
        _iamProxyMock
            .Setup(expression: x => x.GetUserByEmailAsync(new GetUserByEmailRequest(_email),
                It.IsAny<IMessageContext>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: _getUserByEmailResponse);

        _budgetPermissionRepositoryMock
            .Setup(expression: x => x.GetByBudgetIdAsync(_budgetId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: _budgetPermission);

        _budgetPermissionRequestRepositoryMock
            .Setup(expression: x =>
                x.GetUncompletedByPersonIdAsync(_budgetId, _participantId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(value:
            [
                BudgetPermissionRequest.Create(budgetPermissionRequestId: _budgetPermissionRequestId,
                    budgetId: _budgetId, ownerId: _ownerId, personId: _participantId,
                    budgetCode: _budgetCode, permissionType: PermissionType.Owner,
                    expirationDate: _clockMock.Object.UtcNow.AddDays(days: 4), submissionDate: _clockMock.Object.UtcNow)
            ]);

        // Act
        Func<Task> action = () => TestCandidate.AssignParticipantAsync(
            budgetPermissionRequestId: _budgetPermissionRequestId, budgetId: _budgetId, email: _email,
            permissionType: permissionType, expirationDays: ExpirationDays,
            cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        await action.Should().NotThrowAsync();
    }
}