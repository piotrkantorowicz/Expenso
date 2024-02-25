namespace Expenso.IAM.Proxy.DTO.GetUser;

public sealed record GetUserExternalResponse(
    string UserId,
    string? Firstname,
    string? Lastname,
    string Username,
    string Email);