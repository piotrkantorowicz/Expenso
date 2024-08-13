using System.Text.Json;

using Expenso.BudgetSharing.Proxy.DTO.MessageBus.BudgetPermissionRequests;
using Expenso.TimeManagement.Proxy.DTO.Request;
using Expenso.TimeManagement.Proxy.DTO.Response;

namespace Expenso.Api.Tests.E2E.TimeManagement.JobEntries;

internal sealed class RegisterJobEntry : JobEntriesTestBase
{
    [Test]
    public async Task Should_RegisterJobEntry()
    {
        // Arrange
        _httpClient.SetFakeBearerToken(token: _claims);
        const string requestPath = "time-management/register-job";

        RegisterJobEntryRequest jobEntryRequest = new(MaxRetries: 5, JobEntryTriggers:
        [
            new RegisterJobEntryRequest_JobEntryTrigger(
                EventType: typeof(BudgetPermissionRequestExpiredIntergrationEvent).AssemblyQualifiedName,
                EventData: JsonSerializer.Serialize(
                    value: new BudgetPermissionRequestExpiredIntergrationEvent(MessageContext: null!,
                        BudgetPermissionRequestId: Guid.NewGuid())))
        ], Interval: null, RunAt: _clockMock.Object.UtcNow.AddSeconds(seconds: 5));

        // Act
        HttpResponseMessage testResult = await _httpClient.PostAsJsonAsync(requestUri: requestPath,
            value: jobEntryRequest);

        // Assert
        testResult.StatusCode.Should().Be(expected: HttpStatusCode.OK);

        RegisterJobEntryResponse? testResultContent =
            await testResult.Content.ReadFromJsonAsync<RegisterJobEntryResponse>();

        testResultContent.Should().NotBeNull();
    }

    [Test]
    public async Task Should_Return401_When_NoAccessTokenProvided()
    {
        // Arrange
        const string requestPath = "time-management/register-job";

        // Act
        HttpResponseMessage testResult = await _httpClient.PostAsync(requestUri: requestPath, content: null);

        // Assert
        testResult.StatusCode.Should().Be(expected: HttpStatusCode.Unauthorized);
    }

    [Test]
    public void Should_RegisterJobEntry_ViaProxy()
    {
        // Arrange
        RegisterJobEntryRequest jobEntryRequest = new(MaxRetries: 5, JobEntryTriggers:
        [
            new RegisterJobEntryRequest_JobEntryTrigger(
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