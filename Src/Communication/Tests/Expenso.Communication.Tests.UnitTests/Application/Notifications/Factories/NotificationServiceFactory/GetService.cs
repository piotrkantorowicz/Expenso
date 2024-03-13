using Expenso.Communication.Core.Application.Notifications.Services.Emails;
using Expenso.Communication.Core.Application.Notifications.Services.InApp;
using Expenso.Communication.Core.Application.Notifications.Services.Push;

using FluentAssertions;

using Moq;

namespace Expenso.Communication.Tests.UnitTests.Application.Notifications.Factories.NotificationServiceFactory;

internal sealed class GetService : NotificationServiceFactoryTestBase
{
    [Test]
    public void Should_ReturnInAppService()
    {
        // Arrange
        // Act
        var result = TestCandidate.GetService<IInAppService>();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(_servicesDictionary[nameof(IInAppService)].GetType());
    }

    [Test]
    public void Should_ReturnEmailService()
    {
        // Arrange
        // Act
        var result = TestCandidate.GetService<IEmailService>();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(_servicesDictionary[nameof(IEmailService)].GetType());
    }

    [Test]
    public void Should_ReturnPushService()
    {
        // Arrange
        // Act
        var result = TestCandidate.GetService<IPushService>();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(_servicesDictionary[nameof(IPushService)].GetType());
    }

    [Test]
    public void Should_ThrowInvalidOperationException_When_ServiceNotFound()
    {
        // Arrange
        _servicesDictionary.Clear();

        // Act
        InvalidOperationException? exception =
            Assert.Throws<InvalidOperationException>(() => TestCandidate.GetService<IPushService>());

        // Assert
        exception.Should().NotBeNull();

        exception
            ?.Message.Should()
            .Be(
                "Notification service Expenso.Communication.Core.Application.Notifications.Services.Push.IPushService hasn't been found.");
    }

    [Test]
    public void Should_ThrowInvalidOperationException_When_ServiceIsNotOfRequestedType()
    {
        // Arrange
        _servicesDictionary.Clear();
        _servicesDictionary.Add(nameof(IEmailService), new Mock<IPushService>().Object);

        // Act
        InvalidOperationException? exception =
            Assert.Throws<InvalidOperationException>(() => TestCandidate.GetService<IEmailService>());

        // Assert
        exception.Should().NotBeNull();

        exception
            ?.Message.Should()
            .Be(
                "Notification service is not of requested type Expenso.Communication.Core.Application.Notifications.Services.Emails.IEmailService.");
    }
}