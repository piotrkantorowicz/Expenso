using Expenso.Api.Tests.E2E.IAM;
using Expenso.Api.Tests.E2E.TestData.IAM;
using Expenso.BudgetSharing.Application.BudgetPermissionRequests.Write.AssignParticipant;
using Expenso.BudgetSharing.Application.BudgetPermissionRequests.Write.AssignParticipant.DTO.Request;
using Expenso.BudgetSharing.Application.BudgetPermissionRequests.Write.AssignParticipant.DTO.Response;
using Expenso.BudgetSharing.Application.BudgetPermissions.Write.AddPermission;
using Expenso.BudgetSharing.Application.BudgetPermissions.Write.AddPermission.DTO.Request;
using Expenso.BudgetSharing.Application.BudgetPermissions.Write.CreateBudgetPermission;
using Expenso.BudgetSharing.Application.BudgetPermissions.Write.DeleteBudgetPermission;
using Expenso.BudgetSharing.Proxy.DTO.API.CreateBudgetPermission.Request;
using Expenso.BudgetSharing.Proxy.DTO.API.CreateBudgetPermission.Response;
using Expenso.Shared.Commands.Dispatchers;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.Api.Tests.E2E.TestData.BudgetSharing;

internal static class BudgetPermissionDataInitializer
{
    public static readonly List<Guid> BudgetPermissionRequestIds = [];
    public static readonly List<Guid> BudgetPermissionIds = [];
    public static readonly List<Guid> BudgetIds = [];

    public static async Task InitializeAsync(ICommandDispatcher commandDispatcher,
        IMessageContextFactory messageContextFactory, CancellationToken cancellationToken)
    {
        IList<(Guid budgetId, string email, AssignParticipantRequest_PermissionType permissionType, int expiration)>
            budgetPermissionRequestIds =
            [
                (new Guid(g: "527336da-3371-45a9-9b9f-bbd42d01ffc2"), FakeIamProxy.ExistingEmails[1],
                    AssignParticipantRequest_PermissionType.SubOwner, 3),
                (new Guid(g: "e33f3920-d004-4702-a876-f723f6a61cf3"), FakeIamProxy.ExistingEmails[1],
                    AssignParticipantRequest_PermissionType.Reviewer, 2),
                (new Guid(g: "8663a59b-396e-41b9-9aee-163a6d51bcf9"), FakeIamProxy.ExistingEmails[2],
                    AssignParticipantRequest_PermissionType.SubOwner, 5)
            ];

        BudgetIds.AddRange(collection: budgetPermissionRequestIds.Select(selector: x => x.budgetId));

        foreach ((Guid budgetId, string email, AssignParticipantRequest_PermissionType permissionType, int expiration)
                 in budgetPermissionRequestIds)
        {
            CreateBudgetPermissionResponse? createBudgetPermissionResponse =
                await commandDispatcher.SendAsync<CreateBudgetPermissionCommand, CreateBudgetPermissionResponse>(
                    command: new CreateBudgetPermissionCommand(MessageContext: messageContextFactory.Current(),
                        CreateBudgetPermissionRequest: new CreateBudgetPermissionRequest(BudgetPermissionId: null,
                            BudgetId: budgetId, OwnerId: UserDataInitializer.UserIds[index: 0])),
                    cancellationToken: cancellationToken);

            AssignParticipantResponse? assignParticipantResponse =
                await commandDispatcher.SendAsync<AssignParticipantCommand, AssignParticipantResponse>(
                    command: new AssignParticipantCommand(MessageContext: messageContextFactory.Current(),
                        AssignParticipantRequest: new AssignParticipantRequest(BudgetId: budgetId, Email: email,
                            PermissionType: permissionType, ExpirationDays: expiration)),
                    cancellationToken: cancellationToken);

            BudgetPermissionIds.Add(item: createBudgetPermissionResponse!.BudgetPermissionId);
            BudgetPermissionRequestIds.Add(item: assignParticipantResponse!.BudgetPermissionRequestId);
        }

        await commandDispatcher.SendAsync(
            command: new AddPermissionCommand(MessageContext: messageContextFactory.Current(),
                BudgetPermissionId: BudgetPermissionIds[index: 0], ParticipantId: UserDataInitializer.UserIds[index: 3],
                AddPermissionRequest: new AddPermissionRequest(
                    PermissionType: AddPermissionRequest_PermissionType.Reviewer)),
            cancellationToken: cancellationToken);

        await commandDispatcher.SendAsync(
            command: new DeleteBudgetPermissionCommand(MessageContext: messageContextFactory.Current(),
                BudgetPermissionId: BudgetPermissionIds[index: 2]), cancellationToken: cancellationToken);
    }
}