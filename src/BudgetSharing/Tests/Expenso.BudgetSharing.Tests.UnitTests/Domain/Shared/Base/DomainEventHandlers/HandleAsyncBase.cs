using Expenso.BudgetSharing.Domain.Shared.Shared.Notifications.Models;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.Communication.Shared.DTO.API.SendNotification;
using Expenso.Shared.Domain.Events;
using Expenso.Shared.Domain.Types.Events;
using Expenso.Shared.System.Types.Messages.Interfaces;

using Moq;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.Shared.Base.DomainEventHandlers;

[TestFixture]
internal abstract class HandleAsyncBase<T, TEvent> : EventHandlerTestBase<T, TEvent>
    where T : class, IDomainEventHandler<TEvent> where TEvent : class, IDomainEvent
{
    [Test]
    public void Should_NotThrow()
    {
        // Arrange
        _iIamProxyServiceMock
            .Setup(expression: x => x.GetUserNotificationAvailability(MessageContextFactoryMock.Object.Current(null),
                _defaultOwnerId, new[]
                    {
                        _defaultParticipantId
                    }
                    .ToList()
                    .AsReadOnly(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: _defaultNotificationRecipients);

        TEvent @event = CreateEvent();

        // Assert
        Assert.DoesNotThrowAsync(code: () => TestCandidate.HandleAsync(@event: @event, cancellationToken: default));
    }

    [Test]
    public virtual async Task Should_SendNotification_For_Recipients()
    {
        await Should_SendNotification_For_Recipients_Internal(notificationCount: Times.Exactly(callCount: 2));
    }

    [Test]
    public async Task Should_SendNotificationToOwner_When_ParticipantHasNotBeenFound()
    {
        // Arrange
        _iIamProxyServiceMock
            .Setup(expression: x => x.GetUserNotificationAvailability(MessageContextFactoryMock.Object.Current(null),
                _defaultOwnerId, new[]
                    {
                        _defaultParticipantId
                    }
                    .ToList()
                    .AsReadOnly(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: _defaultNotificationRecipients with
            {
                Participants = new List<NotificationRecipient>()
            });

        TEvent @event = CreateEvent();

        // Act
        await TestCandidate.HandleAsync(@event: @event, cancellationToken: default);

        // Assert
        _communicationProxyMock.Verify(
            expression: x =>
                x.SendNotificationAsync(It.IsAny<SendNotificationRequest>(), It.IsAny<CancellationToken>()),
            times: Times.Once);
    }

    [Test]
    public virtual async Task Should_Call_GetUserNotificationAvailability_With_MessageContext()
    {
        await Should_Call_GetUserNotificationAvailability_With_MessageContext_Internal(
            notificationCount: Times.Exactly(callCount: 2));
    }

    protected abstract TEvent CreateEvent();

    protected async Task Should_SendNotification_For_Recipients_Internal(Times notificationCount)
    {
        // Arrange
        _iIamProxyServiceMock
            .Setup(expression: x => x.GetUserNotificationAvailability(MessageContextFactoryMock.Object.Current(null),
                _defaultOwnerId, new[]
                    {
                        _defaultParticipantId
                    }
                    .ToList()
                    .AsReadOnly(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: _defaultNotificationRecipients);

        TEvent @event = CreateEvent();

        // Act
        await TestCandidate.HandleAsync(@event: @event, cancellationToken: default);

        // Assert
        _communicationProxyMock.Verify(
            expression: x =>
                x.SendNotificationAsync(It.IsAny<SendNotificationRequest>(), It.IsAny<CancellationToken>()),
            times: notificationCount);
    }

    protected async Task Should_Call_GetUserNotificationAvailability_With_MessageContext_Internal(
        Times notificationCount)
    {
        // Arrange
        TEvent @event = CreateEvent();

        _iIamProxyServiceMock
            .Setup(expression: x =>
                x.GetUserNotificationAvailability(It.Is<IMessageContext>(mc => mc == @event.MessageContext),
                    _defaultOwnerId, It.IsAny<IReadOnlyCollection<PersonId>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: _defaultNotificationRecipients);

        // Act
        await TestCandidate.HandleAsync(@event: @event, cancellationToken: CancellationToken.None);

        // Assert
        _iIamProxyServiceMock.Verify(
            expression: x =>
                x.GetUserNotificationAvailability(It.Is<IMessageContext>(mc => mc == @event.MessageContext),
                    _defaultOwnerId, It.IsAny<IReadOnlyCollection<PersonId>>(), It.IsAny<CancellationToken>()),
            times: Times.Once);

        _communicationProxyMock.Verify(
            expression: x =>
                x.SendNotificationAsync(It.IsAny<SendNotificationRequest>(), It.IsAny<CancellationToken>()),
            times: notificationCount);
    }
}