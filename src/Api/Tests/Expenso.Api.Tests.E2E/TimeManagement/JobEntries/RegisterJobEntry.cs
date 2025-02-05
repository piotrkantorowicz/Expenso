using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

using Expenso.Api.Tests.E2E.TestData;
using Expenso.BudgetSharing.Shared.DTO.MessageBus.BudgetPermissionRequests.ExpireAssigningParticipant;
using Expenso.BudgetSharing.Shared.DTO.MessageBus.BudgetPermissionRequests.ExpireAssigningParticipant.Payload;
using Expenso.Shared.System.Modules.Constants;
using Expenso.Shared.System.Types.Messages;
using Expenso.TimeManagement.Shared.DTO.RegisterJobEntry.Request;
using Expenso.TimeManagement.Shared.DTO.RegisterJobEntry.Response;

using NUnit.Framework;

using Shouldly;

namespace Expenso.Api.Tests.E2E.TimeManagement.JobEntries;

[TestFixture]
internal sealed class RegisterJobEntry : JobEntriesTestBase
{
    [Test]
    public async Task Should_Returns201_When_InputIsValid()
    {
        // Arrange
        _httpClient.SetFakeBearerToken(token: _claimsService.GetClaims());

        // Act
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync(requestUri: ApiRequestUrl,
            value: CreateTestRequest());

        // Assert
        AssertResponse(response: response, statusCode: HttpStatusCode.Created);

        RegisterJobEntryResponse? responseContent =
            await response.Content.ReadFromJsonAsync<RegisterJobEntryResponse>();

        responseContent.ShouldNotBeNull();
    }

    [Test]
    public async Task Should_Return401_When_NoAccessTokenProvided()
    {
        // Arrange
        // Act
        HttpResponseMessage response = await _httpClient.PostAsync(requestUri: ApiRequestUrl, content: null);

        // Assert
        AssertResponseStatusCode(response: response, statusCode: HttpStatusCode.Unauthorized);
    }

    [Test]
    public async Task Should_RegisterJobEntry_ViaProxy()
    {
        // Arrange
        // Act
        Func<Task> action = () => _timeManagementProxy.RegisterJobEntry(jobEntryRequest: CreateTestRequest(),
            messageContext: new MessageContext(messageId: Guid.CreateVersion7(), correlationId: Guid.CreateVersion7(),
                requestedBy: TestClient.ClientId, timestamp: _clock.UtcNow, module: ModuleNames.TimeManagementModule));

        // Assert
        await action.ShouldNotThrowAsync();
    }

    private RegisterJobEntryRequest CreateTestRequest()
    {
        return new RegisterJobEntryRequest(MaxRetries: 5, JobEntryTriggers:
        [
            new RegisterJobEntryRequestJobEntryTrigger(
                EventType: RegisterJobEntryRequestJobEntryTriggerAllowedEventType.BudgetPermissionRequestExpired,
                EventData: JsonSerializer.Serialize(value: new BudgetPermissionRequestExpiredIntegrationEvent(
                    MessageContext: new MessageContext(messageId: Guid.CreateVersion7(),
                        correlationId: Guid.CreateVersion7(), requestedBy: TestClient.ClientId,
                        timestamp: _clock.UtcNow, module: ModuleNames.BudgetSharingModule),
                    Payload: new BudgetPermissionRequestExpiredPayload(
                        BudgetPermissionRequestId: Guid.CreateVersion7()))))
        ], Interval: null, RunAt: _clock.UtcNow.AddSeconds(seconds: 30));
    }
}