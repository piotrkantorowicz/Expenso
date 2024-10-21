using Expenso.Shared.Domain.Types.Rules;

namespace Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Rules;

internal sealed class OnlyExistingUserCanBeAssignedAsBudgetParticipant : IBusinessRule
{
    private readonly string _email;
    private readonly string? _userId;

    public OnlyExistingUserCanBeAssignedAsBudgetParticipant(string email, string? userId)
    {
        _email = email;
        _userId = userId;
    }

    public string Message =>
        $"Budget participant must be the existing system user, but provided user with email {_email} hasn't been found in the system.";

    public bool IsBroken()
    {
        return !Guid.TryParse(input: _userId, result: out _);
    }
}