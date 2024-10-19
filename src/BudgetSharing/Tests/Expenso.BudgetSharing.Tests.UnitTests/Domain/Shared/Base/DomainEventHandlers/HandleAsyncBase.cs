using Expenso.BudgetSharing.Domain.Shared.Shared.Notifications.Models;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.Communication.Shared.DTO.API.SendNotification;
using Expenso.IAM.Shared.DTO.GetUser.Response;
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
            .ReturnsAsync(value: _defaultNotificationModel);

        TEvent @event = CreateEvent();

        // Assert
        Assert.DoesNotThrowAsync(code: () => TestCandidate.HandleAsync(@event: @event, cancellationToken: default));
    }

    [Test]
    public async Task Should_SendNotification_For_BothPersons()
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
            .ReturnsAsync(value: _defaultNotificationModel);

        TEvent @event = CreateEvent();

        // Act
        await TestCandidate.HandleAsync(@event: @event, cancellationToken: default);

        // Assert
        _communicationProxyMock.Verify(
            expression: x =>
                x.SendNotificationAsync(It.IsAny<SendNotificationRequest>(), It.IsAny<CancellationToken>()),
            times: Times.Exactly(callCount: 2));
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
            .ReturnsAsync(value: _defaultNotificationModel with
            {
                Participants = new List<PersonNotificationModel>()
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
    public async Task Should_SendNotificationToOwner_When_ParticipantHasDifferentId()
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
            .ReturnsAsync(value: _defaultNotificationModel with
            {
                Participants =
                [
                    new PersonNotificationModel(
                        Person: new GetUserResponse(UserId: Guid.NewGuid().ToString(), Firstname: "Francisco",
                            Lastname: "Yue", Username: "francisco224", Email: "francisco224@email.com"),
                        CanSendNotification: true)
                ]
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
    public async Task Should_Call_GetUserNotificationAvailability_With_MessageContext()
    {
        // Arrange
        TEvent @event = CreateEvent();

        _iIamProxyServiceMock
            .Setup(expression: x =>
                x.GetUserNotificationAvailability(It.Is<IMessageContext>(mc => mc == @event.MessageContext),
                    _defaultOwnerId, It.IsAny<IReadOnlyCollection<PersonId>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: _defaultNotificationModel);

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
            times: Times.Exactly(callCount: 2));
    }

    protected abstract TEvent CreateEvent();
}