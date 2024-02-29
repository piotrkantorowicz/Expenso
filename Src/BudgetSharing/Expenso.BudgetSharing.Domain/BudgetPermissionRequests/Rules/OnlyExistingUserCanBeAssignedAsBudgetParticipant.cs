using Expenso.IAM.Proxy.DTO.GetUser;
using Expenso.Shared.Domain.Types.Rules;

namespace Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Rules;

internal sealed class OnlyExistingUserCanBeAssignedAsBudgetParticipant(string email, GetUserResponse? user)
    : IBusinessRule
{
    public string Message =>
        $"Budget participant must be the existing system user, but provided user with id {email} hasn't been found in the system";

    public bool IsBroken()
    {
        return !Guid.TryParse(user?.UserId, out _);
    }
}