namespace Expenso.IAM.Core.Users.DTO.GetUser;

public sealed record GetUserResponse(string UserId, string? Firstname, string? Lastname, string Username, string Email);