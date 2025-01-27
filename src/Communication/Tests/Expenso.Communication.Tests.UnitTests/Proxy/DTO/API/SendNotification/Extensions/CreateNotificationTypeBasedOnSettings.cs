using Expenso.Communication.Shared.DTO.API.SendNotification;
using Expenso.Communication.Shared.DTO.API.SendNotification.Extensions;
using Expenso.Communication.Shared.DTO.Settings;
using Expenso.Communication.Shared.DTO.Settings.InApp;
using Expenso.Communication.Shared.DTO.Settings.Push;

using NUnit.Framework;

using Shouldly;

namespace Expenso.Communication.Tests.UnitTests.Proxy.DTO.API.SendNotification.Extensions;

[TestFixture]
internal sealed class CreateNotificationTypeBasedOnSettings : SendNotificationRequestNotificationTypeExtensionsTestBase
{
    [Test]
    public void Should_Return_DisabledNotification_When_SettingsAreNull()
    {
        // Arrange
        NotificationSettings? settings = null;

        // Act
        SendNotificationRequestNotificationType result = settings.CreateNotificationTypeBasedOnSettings();

        // Assert
        result.ShouldNotBeNull();
        result.Email!.Value.ShouldBeFalse();
        result.Push!.Value.ShouldBeFalse();
        result.InApp!.Value.ShouldBeFalse();
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
        SendNotificationRequestNotificationType result = _settings.CreateNotificationTypeBasedOnSettings();

        // Assert
        result.ShouldNotBeNull();
        result.Email!.Value.ShouldBeFalse();
        result.Push!.Value.ShouldBeFalse();
        result.InApp!.Value.ShouldBeFalse();
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
        SendNotificationRequestNotificationType result = _settings.CreateNotificationTypeBasedOnSettings();

        // Assert
        result.ShouldNotBeNull();
        result.Email!.Value.ShouldBeTrue();
        result.Push!.Value.ShouldBeTrue();
        result.InApp!.Value.ShouldBeFalse();
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
        SendNotificationRequestNotificationType result = _settings.CreateNotificationTypeBasedOnSettings();

        // Assert
        result.ShouldNotBeNull();
        result.Email!.Value.ShouldBeFalse();
        result.Push!.Value.ShouldBeTrue();
        result.InApp!.Value.ShouldBeTrue();
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
        SendNotificationRequestNotificationType result = _settings.CreateNotificationTypeBasedOnSettings();

        // Assert
        result.ShouldNotBeNull();
        result.Email!.Value.ShouldBeTrue();
        result.Push!.Value.ShouldBeFalse();
        result.InApp!.Value.ShouldBeTrue();
    }
}