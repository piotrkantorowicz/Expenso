using Expenso.Shared.Tests.Utils.UnitTests;
using Expenso.TimeManagement.Core.Application.Shared.Settings;

using Microsoft.Extensions.Logging;

using Moq;

namespace Expenso.TimeManagement.Tests.UnitTests.Application.JobEntries.Shared.BackgroundJobs.Events.EventTypeResolver;

[TestFixture]
internal abstract class
    EventTypeResolverTestBase : TestBase<Core.Application.JobEntries.Shared.BackgroundJobs.Events.EventTypeResolver>
{
    [SetUp]
    public void Setup()
    {
        _loggerMock = new Mock<ILogger<Core.Application.JobEntries.Shared.BackgroundJobs.Events.EventTypeResolver>>();

        _timeManagementSettings = new TimeManagementSettings
        {
            AllowedEvents = DefaultAllowedEvents
        };

        TestCandidate = new Core.Application.JobEntries.Shared.BackgroundJobs.Events.EventTypeResolver(
            logger: _loggerMock.Object, timeManagementSettings: _timeManagementSettings);
    }

    [TearDown]
    public void TearDown()
    {
        _loggerMock.Reset();
        _loggerMock = null!;
        _timeManagementSettings = null!;
        TestCandidate = null!;
    }

    private static readonly AllowedEventType[] DefaultAllowedEvents = [AllowedEventType.BudgetPermissionRequestExpired];

    private Mock<ILogger<Core.Application.JobEntries.Shared.BackgroundJobs.Events.EventTypeResolver>> _loggerMock =
        null!;

    private TimeManagementSettings _timeManagementSettings = null!;
}