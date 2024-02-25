using Expenso.IAM.Proxy.DTO.GetUser;
using Expenso.Shared.Domain.Types.Rules;

namespace Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Rules;

internal sealed class OnlyExistingUserCanBeAssignedAsBudgetParticipant(Guid userId, GetUserExternalResponse? user)
    : IBusinessRule
{
    public string Message =>
        $"Budget participant must be the existing system user, but provided user with id {userId} hasn't been found in the system";

    public bool IsBroken()
    {
        return user is null;
    }
}