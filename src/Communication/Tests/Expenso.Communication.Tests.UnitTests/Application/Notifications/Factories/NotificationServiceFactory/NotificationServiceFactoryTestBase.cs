using Expenso.Communication.Core.Application.Notifications.Factories.Interfaces;
using Expenso.Communication.Core.Application.Notifications.Services;
using Expenso.Communication.Core.Application.Notifications.Services.Emails;
using Expenso.Communication.Core.Application.Notifications.Services.InApp;
using Expenso.Communication.Core.Application.Notifications.Services.Push;
using Expenso.Shared.Tests.Utils.UnitTests;

using Moq;

using NUnit.Framework;

namespace Expenso.Communication.Tests.UnitTests.Application.Notifications.Factories.NotificationServiceFactory;

[TestFixture]
internal abstract class NotificationServiceFactoryTestBase : TestBase<INotificationServiceFactory>
{
    [SetUp]
    public void Setup()
    {
        _inAppService = new Mock<IInAppService>();
        _emailService = new Mock<IEmailService>();
        _pushService = new Mock<IPushService>();

        _servicesDictionary = new Dictionary<string, INotificationService>
        {
            { nameof(IInAppService), _inAppService.Object },
            { nameof(IEmailService), _emailService.Object },
            { nameof(IPushService), _pushService.Object }
        };

        TestCandidate =
            new Core.Application.Notifications.Factories.NotificationServiceFactory(
                servicesDictionary: _servicesDictionary);
    }

    [TearDown]
    public void TearDown()
    {
        _inAppService.Reset();
        _emailService.Reset();
        _pushService.Reset();
        _inAppService = null!;
        _emailService = null!;
        _pushService = null!;
        TestCandidate = null!;
    }

    private Mock<IInAppService> _inAppService = null!;
    private Mock<IEmailService> _emailService = null!;
    private Mock<IPushService> _pushService = null!;
    protected IDictionary<string, INotificationService> _servicesDictionary = null!;
}