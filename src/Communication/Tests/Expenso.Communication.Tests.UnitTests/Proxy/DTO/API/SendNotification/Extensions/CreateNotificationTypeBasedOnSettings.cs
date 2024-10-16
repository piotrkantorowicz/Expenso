using Expenso.Communication.Shared.DTO.API.SendNotification;
using Expenso.Communication.Shared.DTO.API.SendNotification.Extensions;
using Expenso.Communication.Shared.DTO.Settings;
using Expenso.Communication.Shared.DTO.Settings.InApp;
using Expenso.Communication.Shared.DTO.Settings.Push;

using FluentAssertions;

namespace Expenso.Communication.Tests.UnitTests.Proxy.DTO.API.SendNotification.Extensions;

internal sealed class CreateNotificationTypeBasedOnSettings : SendNotificationRequest_NotificationTypeExtensionsTestBase
{
    [Test]
    public void Should_Return_DisabledNotification_When_SettingsAreNull()
    {
        // Arrange
        NotificationSettings? settings = null;

        // Act
        SendNotificationRequest_NotificationType result = settings.CreateNotificationTypeBasedOnSettings();

        // Assert
        result.Email.Should().BeFalse();
        result.Push.Should().BeFalse();
        result.InApp.Should().BeFalse();
    }

    [Test]
    public void Should_Return_DisabledNotification_When_SettingsAreDisabled()
    {
        // Arrange
        _settings = _settings with
        {
            Enabled = false
        };

        // Act
        SendNotificationRequest_NotificationType result = _settings.CreateNotificationTypeBasedOnSettings();

        // Assert
        result.Email.Should().BeFalse();
        result.Push.Should().BeFalse();
        result.InApp.Should().BeFalse();
    }

    [Test]
    public void Should_Return_CorrectNotificationType_When_InAppSettingsDisabled()
    {
        // Arrange
        _settings = _settings with
        {
            InApp = new InAppNotificationSettings(Enabled: false)
        };

        // Act
        SendNotificationRequest_NotificationType result = _settings.CreateNotificationTypeBasedOnSettings();

        // Assert
        result.Email.Should().BeTrue();
        result.Push.Should().BeTrue();
        result.InApp.Should().BeFalse();
    }

    [Test]
    public void Should_Return_CorrectNotificationType_When_EmailSettingsDisabledEnabled()
    {
        // Arrange
        _settings = _settings with
        {
            Email = _settings.Email! with
            {
                Enabled = false
            }
        };

        // Act
        SendNotificationRequest_NotificationType result = _settings.CreateNotificationTypeBasedOnSettings();

        // Assert
        result.Email.Should().BeFalse();
        result.Push.Should().BeTrue();
        result.InApp.Should().BeTrue();
    }

    [Test]
    public void Should_Return_CorrectNotificationType_When_PushSettingsDisabledEnabled()
    {
        // Arrange
        _settings = _settings with
        {
            Push = new PushNotificationSettings(Enabled: false)
        };

        // Act
        SendNotificationRequest_NotificationType result = _settings.CreateNotificationTypeBasedOnSettings();

        // Assert
        result.Email.Should().BeTrue();
        result.Push.Should().BeFalse();
        result.InApp.Should().BeTrue();
    }
}