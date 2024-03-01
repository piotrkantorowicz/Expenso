using Expenso.Api.Tests.E2E.IAM;
using Expenso.Api.Tests.E2E.TestData.Preferences;
using Expenso.BudgetSharing.Application.BudgetPermissionRequests.Write.AssignParticipant;
using Expenso.BudgetSharing.Application.BudgetPermissionRequests.Write.AssignParticipant.DTO.Request;
using Expenso.BudgetSharing.Application.BudgetPermissionRequests.Write.AssignParticipant.DTO.Response;
using Expenso.BudgetSharing.Application.BudgetPermissions.Write.CreateBudgetPermission;
using Expenso.BudgetSharing.Proxy.DTO.API.CreateBudgetPermission.Request;
using Expenso.BudgetSharing.Proxy.DTO.API.CreateBudgetPermission.Response;
using Expenso.Shared.Commands.Dispatchers;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.Api.Tests.E2E.TestData.BudgetSharing;

internal static class BudgetPermissionRequestsDataProvider
{
    public static readonly List<Guid> BudgetPermissionRequestIds = [];
    public static readonly List<Guid> BudgetPermissionIds = [];
    public static readonly List<Guid> BudgetIds = [];

    public static async Task Initialize(ICommandDispatcher commandDispatcher,
        IMessageContextFactory messageContextFactory, CancellationToken cancellationToken)
    {
        IList<(Guid budgetId, string email, AssignParticipantRequest_PermissionType permissionType, int expiration)>
            budgetPermissionRequestIds =
            [
                (new Guid("527336da-3371-45a9-9b9f-bbd42d01ffc2"), FakeIamProxy.ExistingEmails[1],
                    AssignParticipantRequest_PermissionType.SubOwner, 3),
                (new Guid("e33f3920-d004-4702-a876-f723f6a61cf3"), FakeIamProxy.ExistingEmails[1],
                    AssignParticipantRequest_PermissionType.Reviewer, 2),
                (new Guid("8663a59b-396e-41b9-9aee-163a6d51bcf9"), FakeIamProxy.ExistingEmails[2],
                    AssignParticipantRequest_PermissionType.SubOwner, 5)
            ];

        BudgetIds.AddRange(budgetPermissionRequestIds.Select(x => x.budgetId));

        foreach ((Guid budgetId, string email, AssignParticipantRequest_PermissionType permissionType, int expiration)
                 in budgetPermissionRequestIds)
        {
            CreateBudgetPermissionResponse? createBudgetPermissionResponse =
                await commandDispatcher.SendAsync<CreateBudgetPermissionCommand, CreateBudgetPermissionResponse>(
                    new CreateBudgetPermissionCommand(messageContextFactory.Current(),
                        new CreateBudgetPermissionRequest(null, budgetId, PreferencesDataProvider.UserIds[0])),
                    cancellationToken);

            AssignParticipantResponse? assignParticipantResponse =
                await commandDispatcher.SendAsync<AssignParticipantCommand, AssignParticipantResponse>(
                    new AssignParticipantCommand(messageContextFactory.Current(),
                        new AssignParticipantRequest(budgetId, email, permissionType, expiration)), cancellationToken);

            BudgetPermissionIds.Add(createBudgetPermissionResponse!.BudgetPermissionId);
            BudgetPermissionRequestIds.Add(assignParticipantResponse!.BudgetPermissionRequestId);
        }
    }
}