namespace Expenso.IAM.Proxy.DTO.GetUser;

public sealed record GetUserInternalResponse(
    string UserId,
    string? Firstname,
    string? Lastname,
    string Username,
    string Email);