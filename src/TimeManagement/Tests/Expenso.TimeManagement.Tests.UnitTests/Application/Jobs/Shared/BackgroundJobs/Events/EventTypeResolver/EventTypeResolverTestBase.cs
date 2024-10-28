using Expenso.Shared.Tests.Utils.UnitTests;
using Expenso.TimeManagement.Core.Application.Shared.Settings;

using Microsoft.Extensions.Logging;

using Moq;

namespace Expenso.TimeManagement.Tests.UnitTests.Application.Jobs.Shared.BackgroundJobs.Events.EventTypeResolver;

[TestFixture]
internal abstract class
    EventTypeResolverTestBase : TestBase<Core.Application.Jobs.Shared.BackgroundJobs.Events.EventTypeResolver>
{
    [SetUp]
    public void Setup()
    {
        _loggerMock = new Mock<ILogger<Core.Application.Jobs.Shared.BackgroundJobs.Events.EventTypeResolver>>();

        _timeManagementSettings = new TimeManagementSettings
        {
            AllowedEvents =
            [
                AllowedEventType.BudgetPermissionRequestExpired
            ]
        };

        TestCandidate =
            new Core.Application.Jobs.Shared.BackgroundJobs.Events.EventTypeResolver(logger: _loggerMock.Object,
                timeManagementSettings: _timeManagementSettings);
    }

    protected Mock<ILogger<Core.Application.Jobs.Shared.BackgroundJobs.Events.EventTypeResolver>> _loggerMock = null!;
    protected TimeManagementSettings _timeManagementSettings = null!;
}