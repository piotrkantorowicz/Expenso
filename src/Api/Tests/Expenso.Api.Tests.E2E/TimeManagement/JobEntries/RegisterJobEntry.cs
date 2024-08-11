using System.Text.Json;

using Expenso.BudgetSharing.Proxy.DTO.MessageBus.BudgetPermissionRequests;
using Expenso.TimeManagement.Proxy.DTO.Request;

namespace Expenso.Api.Tests.E2E.TimeManagement.JobEntries;

internal sealed class RegisterJobEntry : JobEntriesTestBase
{
    [Test]
    public void Should_RegisterJobEntry()
    {
        // Arrange
        AddJobEntryRequest jobEntryRequest = new(MaxRetries: 5, JobEntryTriggers:
        [
            new AddJobEntryRequest_JobEntryTrigger(
                EventType: typeof(BudgetPermissionRequestExpiredIntergrationEvent).AssemblyQualifiedName,
                EventData: JsonSerializer.Serialize(
                    value: new BudgetPermissionRequestExpiredIntergrationEvent(MessageContext: null!,
                        BudgetPermissionRequestId: Guid.NewGuid())))
        ], Interval: null, RunAt: _clockMock.Object.UtcNow.AddSeconds(seconds: 5));

        // Act
        // Assert
        Assert.DoesNotThrowAsync(code: () => _timeManagementProxy.RegisterJobEntry(jobEntryRequest: jobEntryRequest));
    }
}