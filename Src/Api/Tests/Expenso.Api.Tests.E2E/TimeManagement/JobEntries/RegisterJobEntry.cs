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
        var jobEntryRequest = new AddJobEntryRequest(5, [
            new AddJobEntryRequest_JobEntryTrigger(
                typeof(BudgetPermissionRequestExpiredIntergrationEvent).AssemblyQualifiedName,
                JsonSerializer.Serialize(new BudgetPermissionRequestExpiredIntergrationEvent(null!, Guid.NewGuid())))
        ], null, _clockMock.Object.UtcNow.AddSeconds(5));

        // Act
        // Assert
        Assert.DoesNotThrowAsync(() => _timeManagementProxy.RegisterJobEntry(jobEntryRequest));
    }
}