namespace Expenso.IAM.Core.DTO;

public sealed record UserDto(string UserId, string? Firstname, string? Lastname, string Username, string Email);