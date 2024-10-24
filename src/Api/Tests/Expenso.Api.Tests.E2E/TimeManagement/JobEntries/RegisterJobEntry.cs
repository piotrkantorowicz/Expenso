using System.Text.Json;

using Expenso.BudgetSharing.Shared.DTO.MessageBus.BudgetPermissionRequests.ExpireAssigningParticipant;
using Expenso.BudgetSharing.Shared.DTO.MessageBus.BudgetPermissionRequests.ExpireAssigningParticipant.Payload;
using Expenso.TimeManagement.Core.Application.Jobs.Shared.BackgroundJobs.Events;
using Expenso.TimeManagement.Shared.DTO.Request;
using Expenso.TimeManagement.Shared.DTO.Response;

namespace Expenso.Api.Tests.E2E.TimeManagement.JobEntries;

[TestFixture]
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
            new RegisterJobEntryRequest_JobEntryTrigger(EventType: AllowedEvents.BudgetPermissionRequestExpired,
                EventData: JsonSerializer.Serialize(value: new BudgetPermissionRequestExpiredIntegrationEvent(
                    MessageContext: MessageContextFactoryMock.Object.Current(),
                    Payload: new BudgetPermissionRequestExpiredPayload(BudgetPermissionRequestId: Guid.NewGuid()))))
        ], Interval: null, RunAt: _clockMock.Object.UtcNow.AddSeconds(seconds: 5));

        // Act
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync(requestUri: requestPath,
            value: jobEntryRequest);

        // Assert
        AssertResponseOk(response: response);

        RegisterJobEntryResponse? responseContent =
            await response.Content.ReadFromJsonAsync<RegisterJobEntryResponse>();

        responseContent.Should().NotBeNull();
    }

    [Test]
    public async Task Should_Return401_When_NoAccessTokenProvided()
    {
        // Arrange
        const string requestPath = "time-management/register-job";

        // Act
        HttpResponseMessage response = await _httpClient.PostAsync(requestUri: requestPath, content: null);

        // Assert
        AssertResponseUnauthroised(response: response);
    }

    [Test]
    public void Should_RegisterJobEntry_ViaProxy()
    {
        // Arrange
        RegisterJobEntryRequest jobEntryRequest = new(MaxRetries: 5, JobEntryTriggers:
        [
            new RegisterJobEntryRequest_JobEntryTrigger(EventType: AllowedEvents.BudgetPermissionRequestExpired,
                EventData: JsonSerializer.Serialize(value: new BudgetPermissionRequestExpiredIntegrationEvent(
                    MessageContext: MessageContextFactoryMock.Object.Current(),
                    Payload: new BudgetPermissionRequestExpiredPayload(BudgetPermissionRequestId: Guid.NewGuid()))))
        ], Interval: null, RunAt: _clockMock.Object.UtcNow.AddSeconds(seconds: 5));

        // Act
        // Assert
        Assert.DoesNotThrowAsync(code: () => _timeManagementProxy.RegisterJobEntry(jobEntryRequest: jobEntryRequest));
    }
}