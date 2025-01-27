using Expenso.Api.Tests.E2E.IAM;
using Expenso.Api.Tests.E2E.TestData.IAM;
using Expenso.BudgetSharing.Application.BudgetPermissionRequests.Write.AssignParticipant;
using Expenso.BudgetSharing.Application.BudgetPermissionRequests.Write.AssignParticipant.DTO.Request;
using Expenso.BudgetSharing.Application.BudgetPermissionRequests.Write.AssignParticipant.DTO.Response;
using Expenso.BudgetSharing.Application.BudgetPermissions.Write.AddPermission;
using Expenso.BudgetSharing.Application.BudgetPermissions.Write.AddPermission.DTO.Request;
using Expenso.BudgetSharing.Application.BudgetPermissions.Write.CreateBudgetPermission;
using Expenso.BudgetSharing.Application.BudgetPermissions.Write.CreateBudgetPermission.DTO.Request;
using Expenso.BudgetSharing.Application.BudgetPermissions.Write.CreateBudgetPermission.DTO.Response;
using Expenso.BudgetSharing.Application.BudgetPermissions.Write.DeleteBudgetPermission;
using Expenso.BudgetSharing.Application.BudgetPermissions.Write.DeleteBudgetPermission.DTO.Request;
using Expenso.Shared.Commands.Dispatchers;
using Expenso.Shared.System.Modules.Constants;
using Expenso.Shared.System.Time;
using Expenso.Shared.System.Types.Messages;

namespace Expenso.Api.Tests.E2E.TestData.BudgetSharing;

internal static class BudgetPermissionDataInitializer
{
    public static readonly List<Guid> BudgetPermissionRequestIds = [];
    public static readonly List<Guid> BudgetPermissionIds = [];
    public static readonly List<Guid> BudgetIds = [];

    // TODO: allow using proxy there
    public static async Task InitializeAsync(ICommandDispatcher commandDispatcher, IClock clock,
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
                await commandDispatcher.SendAsync<CreateBudgetPermissionCommand, CreateBudgetPermissionResponse>(
                    command: new CreateBudgetPermissionCommand(MessageContext: new MessageContext(
                            messageId: Guid.CreateVersion7(), correlationId: correlationId,
                            requestedBy: TestClient.ClientId, timestamp: clock.UtcNow,
                            module: ModuleNames.BudgetSharingModule),
                        Payload: new CreateBudgetPermissionRequest(BudgetPermissionId: null, BudgetId: budgetId,
                            OwnerId: UserDataInitializer.UserIds[index: 0], BudgetCode: $"BDGT/{iteration}/12/2024")),
                    cancellationToken: cancellationToken);

            AssignParticipantResponse? assignParticipantResponse =
                await commandDispatcher.SendAsync<AssignParticipantCommand, AssignParticipantResponse>(
                    command: new AssignParticipantCommand(MessageContext: new MessageContext(
                            messageId: Guid.CreateVersion7(),
                            correlationId: correlationId,
                            requestedBy: TestClient.ClientId, timestamp: clock.UtcNow,
                            module: ModuleNames.BudgetSharingModule), Payload: new AssignParticipantRequest(
                        BudgetPermissionRequestId: Guid.CreateVersion7(),
                            BudgetId: budgetId, Email: email, PermissionType: permissionType)),
                    cancellationToken: cancellationToken);

            BudgetPermissionIds.Add(item: createBudgetPermissionResponse!.BudgetPermissionId);
            BudgetPermissionRequestIds.Add(item: assignParticipantResponse!.BudgetPermissionRequestId);
        }

        await commandDispatcher.SendAsync(command: new AddPermissionCommand(MessageContext: new MessageContext(
                    messageId: Guid.CreateVersion7(), correlationId: correlationId,
                    requestedBy: TestClient.ClientId, timestamp: clock.UtcNow, module: ModuleNames.BudgetSharingModule),
                Payload: new AddPermissionRequest(BudgetPermissionId: BudgetPermissionIds[index: 0],
                    ParticipantId: UserDataInitializer.UserIds[index: 3],
                    PermissionType: AddPermissionRequestPermissionType.Reviewer)),
            cancellationToken: cancellationToken);

        await commandDispatcher.SendAsync(command: new DeleteBudgetPermissionCommand(MessageContext: new MessageContext(
                    messageId: Guid.CreateVersion7(), correlationId: correlationId,
                    requestedBy: TestClient.ClientId, timestamp: clock.UtcNow, module: ModuleNames.BudgetSharingModule),
                Payload: new DeleteBudgetPermissionRequest(BudgetPermissionId: BudgetPermissionIds[index: 2])),
            cancellationToken: cancellationToken);
    }
}