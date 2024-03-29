﻿using Expenso.Communication.Core.Application.Notifications.Write.Commands.SendNotification;
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
        SendNotificationCommand command = new(MessageContextFactoryMock.Object.Current(), null!);

        // Act
        IDictionary<string, string> result = TestCandidate.Validate(command);

        // Assert
        result.Should().ContainKey(nameof(command.SendNotificationRequest));
        result[nameof(command.SendNotificationRequest)].Should().Be("Send notification request is required");
    }

    [Test]
    public void Should_ReturnError_When_NotificationContextIsNull()
    {
        // Arrange
        SendNotificationCommand command = new(MessageContextFactoryMock.Object.Current(),
            new SendNotificationRequest("Subject", "Content", null,
                new SendNotificationRequest_NotificationType(true, false, false)));

        // Act
        IDictionary<string, string> result = TestCandidate.Validate(command);

        // Assert
        result.Should().ContainKey(nameof(command.SendNotificationRequest.NotificationContext));

        result[nameof(command.SendNotificationRequest.NotificationContext)]
            .Should()
            .Be("Notification context is required");
    }

    [Test]
    public void Should_ReturnError_When_NotificationTypeIsNull()
    {
        // Arrange
        SendNotificationCommand command = new(MessageContextFactoryMock.Object.Current(),
            new SendNotificationRequest("Subject", "Content",
                new SendNotificationRequest_NotificationContext("From", "To"), null!));

        // Act
        IDictionary<string, string> result = TestCandidate.Validate(command);

        // Assert
        result.Should().ContainKey(nameof(command.SendNotificationRequest.NotificationType));
        result[nameof(command.SendNotificationRequest.NotificationType)].Should().Be("Notification type is required");
    }

    [Test]
    public void Should_ReturnError_When_ToIsNull()
    {
        // Arrange
        SendNotificationCommand command = new(MessageContextFactoryMock.Object.Current(),
            new SendNotificationRequest("Subject", "Content",
                new SendNotificationRequest_NotificationContext("From", string.Empty),
                new SendNotificationRequest_NotificationType(true, false, false)));

        // Act
        IDictionary<string, string> result = TestCandidate.Validate(command);

        // Assert
        result.Should().ContainKey(nameof(command.SendNotificationRequest.NotificationContext.To));
        result[nameof(command.SendNotificationRequest.NotificationContext.To)].Should().Be("To is required");
    }

    [Test]
    public void Should_ReturnError_When_FromIsNull()
    {
        // Arrange
        SendNotificationCommand command = new(MessageContextFactoryMock.Object.Current(),
            new SendNotificationRequest("Subject", "Content",
                new SendNotificationRequest_NotificationContext(string.Empty, "To"),
                new SendNotificationRequest_NotificationType(true, false, false)));

        // Act
        IDictionary<string, string> result = TestCandidate.Validate(command);

        // Assert
        result.Should().ContainKey(nameof(command.SendNotificationRequest.NotificationContext.From));
        result[nameof(command.SendNotificationRequest.NotificationContext.From)].Should().Be("From is required");
    }

    [Test]
    public void Should_ReturnError_When_ContentIsNull()
    {
        // Arrange
        SendNotificationCommand command = new(MessageContextFactoryMock.Object.Current(),
            new SendNotificationRequest("Subject", string.Empty,
                new SendNotificationRequest_NotificationContext("From", "To"),
                new SendNotificationRequest_NotificationType(true, false, false)));

        // Act
        IDictionary<string, string> result = TestCandidate.Validate(command);

        // Assert
        result.Should().ContainKey(nameof(command.SendNotificationRequest.Content));

        result[nameof(command.SendNotificationRequest.Content)]
            .Should()
            .Be("Content is required and must be less than 2500 characters");
    }

    [Test]
    public void Should_ReturnError_When_ContentLengthIsGreaterThan2500()
    {
        // Arrange
        SendNotificationCommand command = new(MessageContextFactoryMock.Object.Current(),
            new SendNotificationRequest("Subject", new string('a', 2501),
                new SendNotificationRequest_NotificationContext("From", "To"),
                new SendNotificationRequest_NotificationType(true, false, false)));

        // Act
        IDictionary<string, string> result = TestCandidate.Validate(command);

        // Assert
        result.Should().ContainKey(nameof(command.SendNotificationRequest.Content));

        result[nameof(command.SendNotificationRequest.Content)]
            .Should()
            .Be("Content is required and must be less than 2500 characters");
    }

    [Test]
    public void Should_ReturnError_When_NotificationTypeIsNotSet()
    {
        // Arrange
        SendNotificationCommand command = new(MessageContextFactoryMock.Object.Current(),
            new SendNotificationRequest("Subject", "Content",
                new SendNotificationRequest_NotificationContext("From", "To"),
                new SendNotificationRequest_NotificationType(false, false, false)));

        // Act
        IDictionary<string, string> result = TestCandidate.Validate(command);

        // Assert
        result.Should().ContainKey(nameof(command.SendNotificationRequest.NotificationType));

        result[nameof(command.SendNotificationRequest.NotificationType)]
            .Should()
            .Be("At least one notification type is required");
    }
}