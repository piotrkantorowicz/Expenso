using Expenso.Communication.Core.Application.Notifications.Write.Commands.SendNotification;
using Expenso.Communication.Shared.DTO.API.SendNotification;
using Expenso.Shared.Tests.Utils.UnitTests.Assertions;

using FluentValidation.Results;

namespace Expenso.Communication.Tests.UnitTests.Application.Notifications.Write.Commands.SendNotification.
    SendNotificationCommandValidator;

[TestFixture]
internal sealed class Validate : SendNotificationCommandValidatorTestBase
{
    [Test]
    public void Should_ReturnError_When_SendNotificationRequestIsNull()
    {
        // Arrange
        SendNotificationCommand command = new(MessageContext: MessageContextFactoryMock.Object.Current(),
            Payload: null);

        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: command);

        // Assert
        validationResult.AssertSingleError(propertyName: nameof(SendNotificationCommand.Payload),
            errorMessage: "Send notification request is required.");
    }

    [Test]
    public void Should_ReturnError_When_NotificationContextIsNull()
    {
        // Arrange
        SendNotificationCommand command = new(MessageContext: MessageContextFactoryMock.Object.Current(),
            Payload: new SendNotificationRequest(Subject: "Subject", Content: "Content", NotificationContext: null,
                NotificationType: new SendNotificationRequest_NotificationType(Email: true, Push: false,
                    InApp: false)));

        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: command);

        // Assert
        validationResult.AssertSingleError(
            propertyName: $"{nameof(command.Payload)}.{nameof(SendNotificationRequest.NotificationContext)}",
            errorMessage: "Notification context is required.");
    }

    [Test]
    public void Should_ReturnError_When_NotificationTypeIsNull()
    {
        // Arrange
        SendNotificationCommand command = new(MessageContext: MessageContextFactoryMock.Object.Current(),
            Payload: new SendNotificationRequest(Subject: "Subject", Content: "Content",
                NotificationContext: new SendNotificationRequest_NotificationContext(From: "From", To: "To"),
                NotificationType: null!));

        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: command);

        // Assert
        validationResult.AssertSingleError(
            propertyName: $"{nameof(command.Payload)}.{nameof(SendNotificationRequest.NotificationType)}",
            errorMessage: "Notification type is required.");
    }

    [Test]
    public void Should_ReturnError_When_ToIsNull()
    {
        // Arrange
        SendNotificationCommand command = new(MessageContext: MessageContextFactoryMock.Object.Current(),
            Payload: new SendNotificationRequest(Subject: "Subject", Content: "Content",
                NotificationContext: new SendNotificationRequest_NotificationContext(From: "From", To: string.Empty),
                NotificationType: new SendNotificationRequest_NotificationType(Email: true, Push: false,
                    InApp: false)));

        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: command);

        // Assert
        validationResult.AssertSingleError(
            propertyName:
            $"{nameof(command.Payload)}.{nameof(SendNotificationRequest.NotificationContext)}.{nameof(SendNotificationRequest.NotificationContext.To)}",
            errorMessage: "To is required.");
    }

    [Test]
    public void Should_ReturnError_When_FromIsNull()
    {
        // Arrange
        SendNotificationCommand command = new(MessageContext: MessageContextFactoryMock.Object.Current(),
            Payload: new SendNotificationRequest(Subject: "Subject", Content: "Content",
                NotificationContext: new SendNotificationRequest_NotificationContext(From: string.Empty, To: "To"),
                NotificationType: new SendNotificationRequest_NotificationType(Email: true, Push: false,
                    InApp: false)));

        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: command);

        // Assert
        validationResult.AssertSingleError(
            propertyName:
            $"{nameof(command.Payload)}.{nameof(SendNotificationRequest.NotificationContext)}.{nameof(SendNotificationRequest.NotificationContext.From)}",
            errorMessage: "From is required.");
    }

    [Test, TestCase(arguments: null), TestCase(arg: "")]
    public void Should_ReturnError_When_ContentIsNull(string content)
    {
        // Arrange
        SendNotificationCommand command = new(MessageContext: MessageContextFactoryMock.Object.Current(),
            Payload: new SendNotificationRequest(Subject: "Subject", Content: content,
                NotificationContext: new SendNotificationRequest_NotificationContext(From: "From", To: "To"),
                NotificationType: new SendNotificationRequest_NotificationType(Email: true, Push: false,
                    InApp: false)));

        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: command);

        // Assert
        validationResult.AssertSingleError(propertyName: $"{nameof(command.Payload)}.{nameof(command.Payload.Content)}",
            errorMessage: "Content is required.");
    }

    [Test]
    public void Should_ReturnError_When_ContentLengthIsGreaterThan2500()
    {
        // Arrange
        SendNotificationCommand command = new(MessageContext: MessageContextFactoryMock.Object.Current(),
            Payload: new SendNotificationRequest(Subject: "Subject", Content: new string(c: 'a', count: 2501),
                NotificationContext: new SendNotificationRequest_NotificationContext(From: "From", To: "To"),
                NotificationType: new SendNotificationRequest_NotificationType(Email: true, Push: false,
                    InApp: false)));

        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: command);

        // Assert
        validationResult.AssertSingleError(propertyName: $"{nameof(command.Payload)}.{nameof(command.Payload.Content)}",
            errorMessage: "Content must be less than 2500 characters.");
    }

    [Test]
    public void Should_ReturnError_When_NotificationTypeIsNotSet()
    {
        // Arrange
        SendNotificationCommand command = new(MessageContext: MessageContextFactoryMock.Object.Current(),
            Payload: new SendNotificationRequest(Subject: "Subject", Content: "Content",
                NotificationContext: new SendNotificationRequest_NotificationContext(From: "From", To: "To"),
                NotificationType: new SendNotificationRequest_NotificationType(Email: false, Push: false,
                    InApp: false)));

        // Act
        ValidationResult validationResult = TestCandidate.Validate(instance: command);

        // Assert
        validationResult.AssertSingleError(
            propertyName: $"{nameof(command.Payload)}.{nameof(command.Payload.NotificationType)}",
            errorMessage: "At least one notification type is required.");
    }
}