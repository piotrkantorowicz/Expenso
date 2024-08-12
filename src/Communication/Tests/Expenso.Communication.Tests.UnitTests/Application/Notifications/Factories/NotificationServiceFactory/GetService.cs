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
        IInAppService result = TestCandidate.GetService<IInAppService>();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(expectedType: _servicesDictionary[key: nameof(IInAppService)].GetType());
    }

    [Test]
    public void Should_ReturnEmailService()
    {
        // Arrange
        // Act
        IEmailService result = TestCandidate.GetService<IEmailService>();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(expectedType: _servicesDictionary[key: nameof(IEmailService)].GetType());
    }

    [Test]
    public void Should_ReturnPushService()
    {
        // Arrange
        // Act
        IPushService result = TestCandidate.GetService<IPushService>();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(expectedType: _servicesDictionary[key: nameof(IPushService)].GetType());
    }

    [Test]
    public void Should_ThrowInvalidOperationException_When_ServiceNotFound()
    {
        // Arrange
        _servicesDictionary.Clear();

        // Act
        InvalidOperationException? exception =
            Assert.Throws<InvalidOperationException>(code: () => TestCandidate.GetService<IPushService>());

        // Assert
        exception.Should().NotBeNull();

        exception
            ?.Message.Should()
            .Be(expected:
                "Notification service Expenso.Communication.Core.Application.Notifications.Services.Push.IPushService hasn't been found");
    }

    [Test]
    public void Should_ThrowInvalidOperationException_When_ServiceIsNotOfRequestedType()
    {
        // Arrange
        _servicesDictionary.Clear();
        _servicesDictionary.Add(key: nameof(IEmailService), value: new Mock<IPushService>().Object);

        // Act
        InvalidOperationException? exception =
            Assert.Throws<InvalidOperationException>(code: () => TestCandidate.GetService<IEmailService>());

        // Assert
        exception.Should().NotBeNull();

        exception
            ?.Message.Should()
            .Be(expected:
                "Notification service is not of requested type Expenso.Communication.Core.Application.Notifications.Services.Emails.IEmailService");
    }
}