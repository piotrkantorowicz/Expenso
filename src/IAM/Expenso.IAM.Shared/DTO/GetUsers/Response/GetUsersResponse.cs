using Expenso.Shared.System.Types.Helpers;

namespace Expenso.IAM.Shared.DTO.GetUsers.Response;

public sealed record GetUsersResponse(
    string? UserId,
    string? Firstname,
    string? Lastname,
    string? Username,
    string? Email)
{
    public string Fullname => StringHelper.SafelyJoin(separator: " ", Firstname, Lastname);
}