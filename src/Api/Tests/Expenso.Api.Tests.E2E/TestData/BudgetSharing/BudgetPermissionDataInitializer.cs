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
using Expenso.Shared.System.Types.Clock;
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
        Guid correlationId = Guid.NewGuid();

        IList<(Guid budgetId, string email, AssignParticipantRequestPermissionType permissionType)>
            budgetPermissionModels =
            [
                (new Guid(g: "527336da-3371-45a9-9b9f-bbd42d01ffc2"), FakeIamProxy.ExistingEmails[1],
                    AssignParticipantRequestPermissionType.SubOwner),
                (new Guid(g: "e33f3920-d004-4702-a876-f723f6a61cf3"), FakeIamProxy.ExistingEmails[1],
                    AssignParticipantRequestPermissionType.Reviewer),
                (new Guid(g: "8663a59b-396e-41b9-9aee-163a6d51bcf9"), FakeIamProxy.ExistingEmails[2],
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
                            messageId: Guid.NewGuid(), correlationId: correlationId,
                            requestedBy: TestClient.ClientId, timestamp: clock.UtcNow,
                            module: ModuleNames.BudgetSharingModule),
                        Payload: new CreateBudgetPermissionRequest(BudgetPermissionId: null, BudgetId: budgetId,
                            OwnerId: UserDataInitializer.UserIds[index: 0], BudgetCode: $"BDGT/{iteration}/12/2024")),
                    cancellationToken: cancellationToken);

            AssignParticipantResponse? assignParticipantResponse =
                await commandDispatcher.SendAsync<AssignParticipantCommand, AssignParticipantResponse>(
                    command: new AssignParticipantCommand(MessageContext: new MessageContext(messageId: Guid.NewGuid(),
                            correlationId: correlationId,
                            requestedBy: TestClient.ClientId, timestamp: clock.UtcNow,
                            module: ModuleNames.BudgetSharingModule),
                        Payload: new AssignParticipantRequest(BudgetPermissionRequestId: Guid.NewGuid(),
                            BudgetId: budgetId, Email: email, PermissionType: permissionType)),
                    cancellationToken: cancellationToken);

            BudgetPermissionIds.Add(item: createBudgetPermissionResponse!.BudgetPermissionId);
            BudgetPermissionRequestIds.Add(item: assignParticipantResponse!.BudgetPermissionRequestId);
        }

        await commandDispatcher.SendAsync(command: new AddPermissionCommand(
                MessageContext: new MessageContext(messageId: Guid.NewGuid(), correlationId: correlationId,
                    requestedBy: TestClient.ClientId, timestamp: clock.UtcNow, module: ModuleNames.BudgetSharingModule),
                Payload: new AddPermissionRequest(BudgetPermissionId: BudgetPermissionIds[index: 0],
                    ParticipantId: UserDataInitializer.UserIds[index: 3],
                    PermissionType: AddPermissionRequestPermissionType.Reviewer)),
            cancellationToken: cancellationToken);

        await commandDispatcher.SendAsync(command: new DeleteBudgetPermissionCommand(
                MessageContext: new MessageContext(messageId: Guid.NewGuid(), correlationId: correlationId,
                    requestedBy: TestClient.ClientId, timestamp: clock.UtcNow, module: ModuleNames.BudgetSharingModule),
                Payload: new DeleteBudgetPermissionRequest(BudgetPermissionId: BudgetPermissionIds[index: 2])),
            cancellationToken: cancellationToken);
    }
}