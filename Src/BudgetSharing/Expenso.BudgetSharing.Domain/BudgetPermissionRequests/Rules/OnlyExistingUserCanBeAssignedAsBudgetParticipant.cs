using System.Text;

using Expenso.IAM.Proxy.DTO.GetUser;
using Expenso.Shared.Domain.Types.Rules;

namespace Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Rules;

internal sealed class OnlyExistingUserCanBeAssignedAsBudgetParticipant(Guid userId, GetUserInternalResponse? user)
    : IBusinessRule
{
    public string Message =>
        new StringBuilder()
            .Append("Budget participant must be the existing system user, but provided user with id ")
            .Append(userId)
            .Append(" hasn't been found in the system")
            .ToString();

    public bool IsBroken()
    {
        return user is null;
    }
}