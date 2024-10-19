namespace Expenso.IAM.Shared.DTO.GetUser.Request;

public sealed record GetUserRequest(string? UserId = null, string? Email = null);