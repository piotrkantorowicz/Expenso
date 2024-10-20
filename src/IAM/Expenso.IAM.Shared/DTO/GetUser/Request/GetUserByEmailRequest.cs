namespace Expenso.IAM.Shared.DTO.GetUser.Request;

public sealed record GetUserByEmailRequest(string Email) : GetUserRequest;