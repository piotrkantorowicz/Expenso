namespace Expenso.IAM.Shared.DTO.GetUserByEmail.Response;

public sealed record GetUserByEmailResponse(
    string UserId,
    string? Firstname,
    string? Lastname,
    string Username,
    string Email)
{
    public string Fullname => $"{Firstname} {Lastname}";
}