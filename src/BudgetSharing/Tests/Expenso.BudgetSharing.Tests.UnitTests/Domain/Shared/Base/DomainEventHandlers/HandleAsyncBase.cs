using Expenso.BudgetSharing.Domain.Shared.Shared.Notifications.Models;
using Expenso.Communication.Proxy.DTO.API.SendNotification;
using Expenso.IAM.Proxy.DTO.GetUser;
using Expenso.Shared.Domain.Events;
using Expenso.Shared.Domain.Types.Events;

using Moq;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.Shared.Base.DomainEventHandlers;

internal abstract class HandleAsyncBase<T, TEvent> : EventHandlerTestBase<T, TEvent>
    where T : class, IDomainEventHandler<TEvent> where TEvent : class, IDomainEvent
{
    [Test]
    public void Should_NotThrow()
    {
        // Arrange
        _iIamProxyServiceMock
            .Setup(expression: x => x.GetUserNotificationAvailability(_defaultOwnerId, new[]
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
            .Setup(expression: x => x.GetUserNotificationAvailability(_defaultOwnerId, new[]
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
            .Setup(expression: x => x.GetUserNotificationAvailability(_defaultOwnerId, new[]
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
            .Setup(expression: x => x.GetUserNotificationAvailability(_defaultOwnerId, new[]
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

    protected abstract TEvent CreateEvent();
}