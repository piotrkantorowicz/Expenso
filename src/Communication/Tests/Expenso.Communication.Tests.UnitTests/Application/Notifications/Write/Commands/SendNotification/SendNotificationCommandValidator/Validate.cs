using Expenso.Communication.Core.Application.Notifications.Write.Commands.SendNotification;
using Expenso.Communication.Proxy.DTO.API.SendNotification;

using FluentAssertions;

namespace Expenso.Communication.Tests.UnitTests.Application.Notifications.Write.Commands.SendNotification.
    SendNotificationCommandValidator;

internal sealed class Validate : SendNotificationCommandValidatorTestBase
{
    [Test]
    public void Should_ReturnError_When_SendNotificationRequestIsNull()
    {
        // Arrange
        SendNotificationCommand command = new(MessageContext: MessageContextFactoryMock.Object.Current(),
            SendNotificationRequest: null!);

        // Act
        IDictionary<string, string> result = TestCandidate.Validate(command: command);

        // Assert
        result.Should().ContainKey(expected: nameof(command.SendNotificationRequest));

        result[key: nameof(command.SendNotificationRequest)]
            .Should()
            .Be(expected: "Send notification request is required.");
    }

    [Test]
    public void Should_ReturnError_When_NotificationContextIsNull()
    {
        // Arrange
        SendNotificationCommand command = new(MessageContext: MessageContextFactoryMock.Object.Current(),
            SendNotificationRequest: new SendNotificationRequest(Subject: "Subject", Content: "Content",
                NotificationContext: null,
                NotificationType: new SendNotificationRequest_NotificationType(Email: true, Push: false,
                    InApp: false)));

        // Act
        IDictionary<string, string> result = TestCandidate.Validate(command: command);

        // Assert
        result.Should().ContainKey(expected: nameof(command.SendNotificationRequest.NotificationContext));

        result[key: nameof(command.SendNotificationRequest.NotificationContext)]
            .Should()
            .Be(expected: "Notification context is required.");
    }

    [Test]
    public void Should_ReturnError_When_NotificationTypeIsNull()
    {
        // Arrange
        SendNotificationCommand command = new(MessageContext: MessageContextFactoryMock.Object.Current(),
            SendNotificationRequest: new SendNotificationRequest(Subject: "Subject", Content: "Content",
                NotificationContext: new SendNotificationRequest_NotificationContext(From: "From", To: "To"),
                NotificationType: null!));

        // Act
        IDictionary<string, string> result = TestCandidate.Validate(command: command);

        // Assert
        result.Should().ContainKey(expected: nameof(command.SendNotificationRequest.NotificationType));

        result[key: nameof(command.SendNotificationRequest.NotificationType)]
            .Should()
            .Be(expected: "Notification type is required.");
    }

    [Test]
    public void Should_ReturnError_When_ToIsNull()
    {
        // Arrange
        SendNotificationCommand command = new(MessageContext: MessageContextFactoryMock.Object.Current(),
            SendNotificationRequest: new SendNotificationRequest(Subject: "Subject", Content: "Content",
                NotificationContext: new SendNotificationRequest_NotificationContext(From: "From", To: string.Empty),
                NotificationType: new SendNotificationRequest_NotificationType(Email: true, Push: false,
                    InApp: false)));

        // Act
        IDictionary<string, string> result = TestCandidate.Validate(command: command);

        // Assert
        result.Should().ContainKey(expected: nameof(command.SendNotificationRequest.NotificationContext.To));

        result[key: nameof(command.SendNotificationRequest.NotificationContext.To)]
            .Should()
            .Be(expected: "To is required.");
    }

    [Test]
    public void Should_ReturnError_When_FromIsNull()
    {
        // Arrange
        SendNotificationCommand command = new(MessageContext: MessageContextFactoryMock.Object.Current(),
            SendNotificationRequest: new SendNotificationRequest(Subject: "Subject", Content: "Content",
                NotificationContext: new SendNotificationRequest_NotificationContext(From: string.Empty, To: "To"),
                NotificationType: new SendNotificationRequest_NotificationType(Email: true, Push: false,
                    InApp: false)));

        // Act
        IDictionary<string, string> result = TestCandidate.Validate(command: command);

        // Assert
        result.Should().ContainKey(expected: nameof(command.SendNotificationRequest.NotificationContext.From));

        result[key: nameof(command.SendNotificationRequest.NotificationContext.From)]
            .Should()
            .Be(expected: "From is required.");
    }

    [Test]
    public void Should_ReturnError_When_ContentIsNull()
    {
        // Arrange
        SendNotificationCommand command = new(MessageContext: MessageContextFactoryMock.Object.Current(),
            SendNotificationRequest: new SendNotificationRequest(Subject: "Subject", Content: string.Empty,
                NotificationContext: new SendNotificationRequest_NotificationContext(From: "From", To: "To"),
                NotificationType: new SendNotificationRequest_NotificationType(Email: true, Push: false,
                    InApp: false)));

        // Act
        IDictionary<string, string> result = TestCandidate.Validate(command: command);

        // Assert
        result.Should().ContainKey(expected: nameof(command.SendNotificationRequest.Content));

        result[key: nameof(command.SendNotificationRequest.Content)]
            .Should()
            .Be(expected: "Content is required and must be less than 2500 characters.");
    }

    [Test]
    public void Should_ReturnError_When_ContentLengthIsGreaterThan2500()
    {
        // Arrange
        SendNotificationCommand command = new(MessageContext: MessageContextFactoryMock.Object.Current(),
            SendNotificationRequest: new SendNotificationRequest(Subject: "Subject",
                Content: new string(c: 'a', count: 2501),
                NotificationContext: new SendNotificationRequest_NotificationContext(From: "From", To: "To"),
                NotificationType: new SendNotificationRequest_NotificationType(Email: true, Push: false,
                    InApp: false)));

        // Act
        IDictionary<string, string> result = TestCandidate.Validate(command: command);

        // Assert
        result.Should().ContainKey(expected: nameof(command.SendNotificationRequest.Content));

        result[key: nameof(command.SendNotificationRequest.Content)]
            .Should()
            .Be(expected: "Content is required and must be less than 2500 characters.");
    }

    [Test]
    public void Should_ReturnError_When_NotificationTypeIsNotSet()
    {
        // Arrange
        SendNotificationCommand command = new(MessageContext: MessageContextFactoryMock.Object.Current(),
            SendNotificationRequest: new SendNotificationRequest(Subject: "Subject", Content: "Content",
                NotificationContext: new SendNotificationRequest_NotificationContext(From: "From", To: "To"),
                NotificationType: new SendNotificationRequest_NotificationType(Email: false, Push: false,
                    InApp: false)));

        // Act
        IDictionary<string, string> result = TestCandidate.Validate(command: command);

        // Assert
        result.Should().ContainKey(expected: nameof(command.SendNotificationRequest.NotificationType));

        result[key: nameof(command.SendNotificationRequest.NotificationType)]
            .Should()
            .Be(expected: "At least one notification type is required.");
    }
}