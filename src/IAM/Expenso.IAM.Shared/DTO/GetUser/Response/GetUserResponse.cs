namespace Expenso.IAM.Shared.DTO.GetUser.Response;

public sealed record GetUserResponse(string UserId, string? Firstname, string? Lastname, string Username, string Email)
{
    public string Fullname => $"{Firstname} {Lastname}";
}