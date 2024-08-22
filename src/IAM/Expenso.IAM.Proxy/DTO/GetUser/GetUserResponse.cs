namespace Expenso.IAM.Proxy.DTO.GetUser;

public sealed record GetUserResponse(string UserId, string? Firstname, string? Lastname, string Username, string Email)
{
    public string Fullname => $"{Firstname} {Lastname}";
}