namespace Expenso.IAM.Proxy.Contracts;

public sealed record UserContract(string UserId, string? Firstname, string? Lastname, string Username, string Email);