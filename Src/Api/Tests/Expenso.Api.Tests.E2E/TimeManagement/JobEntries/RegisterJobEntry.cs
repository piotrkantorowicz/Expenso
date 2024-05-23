using Expenso.TimeManagement.Proxy.DTO.Request;

namespace Expenso.Api.Tests.E2E.TimeManagement.JobEntries;

internal sealed class RegisterJobEntry : JobEntriesTestBase
{
    private const string JobTypeNameCode = "BS-REQ-EXP";
    
    [Test]
    public void Should_RegisterJobEntry()
    {
        // Arrange
        var jobEntryRequest = new AddJobEntryRequest(JobTypeNameCode, [
            new AddJobEntryRequest_JobEntryPeriod(
                new AddJobEntryRequest_JobEntryPeriodInterval(Minute: "0", Hour: "0", DayofMonth: "12",
                    DayOfWeek: "Mon"), 3, false)
        ], [
            new AddJobEntryRequest_JobEntryTrigger("TestEvent", "TestEvent")
        ]);
        
        // Act
        // Assert
        Assert.DoesNotThrowAsync(() => _timeManagementProxy.RegisterJobEntry(jobEntryRequest));
    }
}