namespace Expenso.IAM.Shared.DTO.GetUsers.Request;

public sealed record GetUsersRequest(
    string? UserId = null,
    string? Email = null,
    string? Username = null,
    string? Firstname = null,
    string? Lastname = null,
    int? Limit = null,
    bool Exact = true);