namespace Expenso.IAM.Core.Users.Queries.GetUser.DTO.Response;

public sealed record GetUserResponse(string UserId, string? Firstname, string? Lastname, string Username, string Email);