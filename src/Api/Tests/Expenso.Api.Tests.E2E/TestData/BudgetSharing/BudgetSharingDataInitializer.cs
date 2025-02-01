using Expenso.Api.Tests.E2E.IAM;
using Expenso.Api.Tests.E2E.TestData.IAM;
using Expenso.BudgetSharing.Shared;
using Expenso.BudgetSharing.Shared.DTO.API.BudgetPermissionRequests.AssignParticipant.Request;
using Expenso.BudgetSharing.Shared.DTO.API.BudgetPermissionRequests.AssignParticipant.Response;
using Expenso.BudgetSharing.Shared.DTO.API.BudgetPermissions.AddPermission.Request;
using Expenso.BudgetSharing.Shared.DTO.API.BudgetPermissions.CreateBudgetPermission.Request;
using Expenso.BudgetSharing.Shared.DTO.API.BudgetPermissions.CreateBudgetPermission.Response;
using Expenso.BudgetSharing.Shared.DTO.API.BudgetPermissions.DeleteBudgetPermission.Request;
using Expenso.Shared.System.Modules.Constants;
using Expenso.Shared.System.Time;
using Expenso.Shared.System.Types.Messages;

namespace Expenso.Api.Tests.E2E.TestData.BudgetSharing;

internal static class BudgetSharingDataInitializer
{
    public static readonly List<Guid> BudgetPermissionRequestIds = [];
    public static readonly List<Guid> BudgetPermissionIds = [];
    public static readonly List<Guid> BudgetIds = [];

    public static async Task InitializeAsync(IClock clock, IBudgetSharingProxy budgetSharingProxy,
        CancellationToken cancellationToken)
    {
        Guid correlationId = Guid.CreateVersion7();

        IList<(Guid budgetId, string email, AssignParticipantRequestPermissionType permissionType)>
            budgetPermissionModels =
            [
                (new Guid(g: "0194a81a-48c6-7573-81d2-e5f66ff83adb"), FakeIamProxy.ExistingEmails[1],
                    AssignParticipantRequestPermissionType.SubOwner),
                (new Guid(g: "0194a81a-48c6-7098-a9cd-5021f7199848"), FakeIamProxy.ExistingEmails[1],
                    AssignParticipantRequestPermissionType.Reviewer),
                (new Guid(g: "0194a81a-48c6-7b6f-80c0-38eed8cfa2be"), FakeIamProxy.ExistingEmails[2],
                    AssignParticipantRequestPermissionType.SubOwner)
            ];

        BudgetIds.AddRange(collection: budgetPermissionModels.Select(selector: x => x.budgetId));
        int iteration = 0;

        foreach ((Guid budgetId, string email, AssignParticipantRequestPermissionType permissionType) in
                 budgetPermissionModels)
        {
            iteration++;

            CreateBudgetPermissionResponse? createBudgetPermissionResponse =
                await budgetSharingProxy.CreateBudgetPermissionAsync(
                    request: new CreateBudgetPermissionRequest(BudgetPermissionId: null, BudgetId: budgetId,
                        OwnerId: UserDataInitializer.UserIds[index: 0], BudgetCode: $"BDGT/{iteration}/12/2024"),
                    messageContext: new MessageContext(messageId: Guid.CreateVersion7(), correlationId: correlationId,
                        requestedBy: TestClient.ClientId, timestamp: clock.UtcNow,
                        module: ModuleNames.BudgetSharingModule), cancellationToken: cancellationToken);

            AssignParticipantResponse? assignParticipantResponse = await budgetSharingProxy.AssignParticipantAsync(
                request: new AssignParticipantRequest(BudgetPermissionRequestId: Guid.CreateVersion7(),
                    BudgetId: budgetId, Email: email, PermissionType: permissionType),
                messageContext: new MessageContext(messageId: Guid.CreateVersion7(), correlationId: correlationId,
                    requestedBy: TestClient.ClientId, timestamp: clock.UtcNow, module: ModuleNames.BudgetSharingModule),
                cancellationToken: cancellationToken);

            BudgetPermissionIds.Add(item: createBudgetPermissionResponse!.BudgetPermissionId);
            BudgetPermissionRequestIds.Add(item: assignParticipantResponse!.BudgetPermissionRequestId);
        }

        await budgetSharingProxy.AddPermissionAsync(
            request: new AddPermissionRequest(BudgetPermissionId: BudgetPermissionIds[index: 0],
                ParticipantId: UserDataInitializer.UserIds[index: 3],
                PermissionType: AddPermissionRequestPermissionType.Reviewer),
            messageContext: new MessageContext(messageId: Guid.CreateVersion7(), correlationId: correlationId,
                requestedBy: TestClient.ClientId, timestamp: clock.UtcNow, module: ModuleNames.BudgetSharingModule),
            cancellationToken: cancellationToken);

        await budgetSharingProxy.DeleteBudgetPermissionAsync(
            request: new DeleteBudgetPermissionRequest(BudgetPermissionId: BudgetPermissionIds[index: 2]),
            messageContext: new MessageContext(messageId: Guid.CreateVersion7(), correlationId: correlationId,
                requestedBy: TestClient.ClientId, timestamp: clock.UtcNow, module: ModuleNames.BudgetSharingModule),
            cancellationToken: cancellationToken);
    }
}