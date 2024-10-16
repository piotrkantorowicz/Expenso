using Expenso.IAM.Shared.DTO.GetUser;
using Expenso.Shared.Domain.Types.Rules;

namespace Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Rules;

internal sealed class OnlyExistingUserCanBeAssignedAsBudgetParticipant : IBusinessRule
{
    private readonly string _email;
    private readonly GetUserResponse? _user;

    public OnlyExistingUserCanBeAssignedAsBudgetParticipant(string email, GetUserResponse? user)
    {
        _email = email;
        _user = user;
    }

    public string Message =>
        $"Budget participant must be the existing system user, but provided user with email {_email} hasn't been found in the system.";

    public bool IsBroken()
    {
        return !Guid.TryParse(input: _user?.UserId, result: out _);
    }
}