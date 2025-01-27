using Expenso.Communication.Core.Application.Notifications.Services.Emails;
using Expenso.Communication.Core.Application.Notifications.Services.InApp;
using Expenso.Communication.Core.Application.Notifications.Services.Push;

using Moq;

using NUnit.Framework;

using Shouldly;

namespace Expenso.Communication.Tests.UnitTests.Application.Notifications.Factories.NotificationServiceFactory;

[TestFixture]
internal sealed class GetService : NotificationServiceFactoryTestBase
{
    [Test]
    public void Should_ReturnInAppService()
    {
        // Arrange
        // Act
        IInAppService result = TestCandidate.GetService<IInAppService>();

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType(expected: _servicesDictionary[key: nameof(IInAppService)].GetType());
    }

    [Test]
    public void Should_ReturnEmailService()
    {
        // Arrange
        // Act
        IEmailService result = TestCandidate.GetService<IEmailService>();

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType(expected: _servicesDictionary[key: nameof(IEmailService)].GetType());
    }

    [Test]
    public void Should_ReturnPushService()
    {
        // Arrange
        // Act
        IPushService result = TestCandidate.GetService<IPushService>();

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType(expected: _servicesDictionary[key: nameof(IPushService)].GetType());
    }

    [Test]
    public void Should_ThrowInvalidOperationException_When_ServiceNotFound()
    {
        // Arrange
        _servicesDictionary.Clear();

        // Act
        Func<IPushService> action = () => TestCandidate.GetService<IPushService>();

        // Assert
        action.ShouldThrow<InvalidOperationException>();
    }

    [Test]
    public void Should_ThrowInvalidOperationException_When_ServiceIsNotOfRequestedType()
    {
        // Arrange
        _servicesDictionary.Clear();
        _servicesDictionary.Add(key: nameof(IEmailService), value: new Mock<IPushService>().Object);

        // Act
        Func<IEmailService> action = () => TestCandidate.GetService<IEmailService>();

        // Assert
        action.ShouldThrow<InvalidOperationException>();
    }
}