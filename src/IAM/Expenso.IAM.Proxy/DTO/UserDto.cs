namespace Expenso.IAM.Proxy.DTO;

public sealed record UserDto(string UserId, string? Firstname, string? Lastname, string Username, string Email);