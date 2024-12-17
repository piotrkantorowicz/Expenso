using System.Text.Json;

using Expenso.Api.Tests.E2E.TestData.BudgetSharing;
using Expenso.BudgetSharing.Shared.DTO.MessageBus.BudgetPermissionRequests.ExpireAssigningParticipant;
using Expenso.BudgetSharing.Shared.DTO.MessageBus.BudgetPermissionRequests.ExpireAssigningParticipant.Payload;
using Expenso.Shared.System.Modules.Constants;
using Expenso.Shared.System.Types.Clock;
using Expenso.Shared.System.Types.Messages;
using Expenso.TimeManagement.Shared;
using Expenso.TimeManagement.Shared.DTO.RegisterJobEntry.Request;
using Expenso.TimeManagement.Shared.DTO.RegisterJobEntry.Response;

namespace Expenso.Api.Tests.E2E.TestData.TimeManagement;

internal static class TimeManagementDataInitializer
{
    private const int NumberOfEntries = 3;
    public static readonly IList<Guid> JobEntriesIds = new List<Guid>();

    public static async Task InitializeAsync(ITimeManagementProxy timeManagementProxy, IClock clock,
        CancellationToken cancellationToken)
    {
        Guid correlationId = Guid.NewGuid();

        MessageContext messageContext = new(messageId: Guid.NewGuid(), correlationId: correlationId,
            requestedBy: TestClient.ClientId, timestamp: clock.UtcNow, module: ModuleNames.TimeManagementModule);

        for (int i = 0; i < NumberOfEntries; i++)
        {
            RegisterJobEntryRequest registerJobEntryRequest = new(MaxRetries: 5, JobEntryTriggers:
            [
                new RegisterJobEntryRequestJobEntryTrigger(
                    EventType: RegisterJobEntryRequestJobEntryTriggerAllowedEventType.BudgetPermissionRequestExpired,
                    EventData: JsonSerializer.Serialize(value: new BudgetPermissionRequestExpiredIntegrationEvent(
                        MessageContext: new MessageContext(messageId: Guid.NewGuid(), correlationId: Guid.NewGuid(),
                            requestedBy: TestClient.ClientId, timestamp: clock.UtcNow,
                            module: ModuleNames.BudgetSharingModule),
                        Payload: new BudgetPermissionRequestExpiredPayload(
                            BudgetPermissionRequestId: BudgetPermissionDataInitializer.BudgetPermissionRequestIds[
                                index: i]))))
            ], Interval: null, RunAt: clock.UtcNow.AddHours(hours: 5));

            RegisterJobEntryResponse? registerJobEntryResponse = await timeManagementProxy.RegisterJobEntry(
                jobEntryRequest: registerJobEntryRequest, messageContext: messageContext,
                cancellationToken: cancellationToken);

            JobEntriesIds.Add(item: registerJobEntryResponse!.JobEntryId);
        }
    }
}