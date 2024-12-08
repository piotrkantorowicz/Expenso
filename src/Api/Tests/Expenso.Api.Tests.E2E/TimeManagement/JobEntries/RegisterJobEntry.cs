using System.Text.Json;

using Expenso.Api.Tests.E2E.TestData;
using Expenso.BudgetSharing.Shared.DTO.MessageBus.BudgetPermissionRequests.ExpireAssigningParticipant;
using Expenso.BudgetSharing.Shared.DTO.MessageBus.BudgetPermissionRequests.ExpireAssigningParticipant.Payload;
using Expenso.Shared.System.Modules.Constants;
using Expenso.Shared.System.Types.Messages;
using Expenso.TimeManagement.Shared.DTO.RegisterJobEntry.Request;
using Expenso.TimeManagement.Shared.DTO.RegisterJobEntry.Response;

namespace Expenso.Api.Tests.E2E.TimeManagement.JobEntries;

[TestFixture]
internal sealed class RegisterJobEntry : JobEntriesTestBase
{
    [Test]
    public async Task Should_RegisterJobEntry()
    {
        // Arrange
        _httpClient.SetFakeBearerToken(token: _claims);
        const string requestPath = "time-management/job-entries";

        // Act
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync(requestUri: requestPath,
            value: CreateTestRequest());

        // Assert
        AssertResponseCreated(response: response);

        RegisterJobEntryResponse? responseContent =
            await response.Content.ReadFromJsonAsync<RegisterJobEntryResponse>();

        responseContent.Should().NotBeNull();
    }

    [Test]
    public async Task Should_Return401_When_NoAccessTokenProvided()
    {
        // Arrange
        const string requestPath = "time-management/job-entries";

        // Act
        HttpResponseMessage response = await _httpClient.PostAsync(requestUri: requestPath, content: null);

        // Assert
        AssertResponseUnauthroised(response: response);
    }

    [Test]
    public async Task Should_RegisterJobEntry_ViaProxy()
    {
        // Arrange
        // Act
        Func<Task> action = () => _timeManagementProxy.RegisterJobEntry(jobEntryRequest: CreateTestRequest(),
            messageContext: new MessageContext(messageId: Guid.NewGuid(), correlationId: Guid.NewGuid(),
                requestedBy: TestClient.ClientId, timestamp: _clock.UtcNow, module: ModuleNames.TimeManagementModule));

        // Assert
        await action.Should().NotThrowAsync();
    }

    private RegisterJobEntryRequest CreateTestRequest()
    {
        return new RegisterJobEntryRequest(MaxRetries: 5, JobEntryTriggers:
        [
            new RegisterJobEntryRequestJobEntryTrigger(
                EventType: RegisterJobEntryRequestJobEntryTriggerAllowedEventType.BudgetPermissionRequestExpired,
                EventData: JsonSerializer.Serialize(value: new BudgetPermissionRequestExpiredIntegrationEvent(
                    MessageContext: new MessageContext(messageId: Guid.NewGuid(), correlationId: Guid.NewGuid(),
                        requestedBy: TestClient.ClientId, timestamp: _clock.UtcNow,
                        module: ModuleNames.BudgetSharingModule),
                    Payload: new BudgetPermissionRequestExpiredPayload(BudgetPermissionRequestId: Guid.NewGuid()))))
        ], Interval: null, RunAt: _clock.UtcNow.AddSeconds(seconds: 5));
    }
}