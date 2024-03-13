using Expenso.Communication.Core.Application.Notifications.Factories.Interfaces;
using Expenso.Communication.Core.Application.Notifications.Services;
using Expenso.Communication.Core.Application.Notifications.Services.Emails;
using Expenso.Communication.Core.Application.Notifications.Services.InApp;
using Expenso.Communication.Core.Application.Notifications.Services.Push;
using Expenso.Shared.Tests.Utils.UnitTests;

using Moq;

using TestCandidate = Expenso.Communication.Core.Application.Notifications.Factories.NotificationServiceFactory;

namespace Expenso.Communication.Tests.UnitTests.Application.Notifications.Factories.NotificationServiceFactory;

internal abstract class NotificationServiceFactoryTestBase : TestBase<INotificationServiceFactory>
{
    protected readonly IDictionary<string, INotificationService> _servicesDictionary =
        new Dictionary<string, INotificationService>
        {
            { nameof(IInAppService), new Mock<IInAppService>().Object },
            { nameof(IEmailService), new Mock<IEmailService>().Object },
            { nameof(IPushService), new Mock<IPushService>().Object }
        };

    [SetUp]
    public void Setup()
    {
        TestCandidate = new TestCandidate(_servicesDictionary);
    }
}